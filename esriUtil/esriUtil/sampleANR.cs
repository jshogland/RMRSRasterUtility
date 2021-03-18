using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using esriUtil;
using Accord.Statistics.Analysis;

namespace esriUtil
{
    public class sampleANR
    {
        public sampleANR(IFunctionRasterDataset referenceRs, IFunctionRasterDataset transformRs, string mdlPath=null, int percentChange=20, IFeatureClass mask=null,int sampleSize= 1000, int blockSize = 50,  rasterUtil rasterUtility = null, string storeXY=null)
        {
            refRs = referenceRs;
            transRs = transformRs;
            pct = percentChange/200d;
            mskFtrCls = mask;
            rsUtil = rasterUtility;
            n = sampleSize;
            rndPnts = new IPoint[sampleSize];
            refArray = new double[refRs.RasterInfo.BandCount][][];
            tranArray = new double[transRs.RasterInfo.BandCount][][];
            useArray = new bool[transRs.RasterInfo.BandCount][][];
            minArrayRef = new double[tranArray.Length];
            maxArrayRef = new double[tranArray.Length];
            minArrayTran = new double[tranArray.Length];
            maxArrayTran = new double[tranArray.Length];
            setArrayValues();
            outPth = mdlPath;
            bSize = blockSize;
            stXY = storeXY; 
        }

        

        private void setArrayValues()
        {
            for (int i = 0; i < minArrayRef.Length; i++)
            {
                minArrayRef[i] = double.MaxValue;
                minArrayTran[i] = double.MaxValue;
                maxArrayRef[i] = double.MinValue;
                maxArrayTran[i] = double.MinValue;
            }
        }

        private rasterUtil rsUtil = null;
        private IFunctionRasterDataset refRs = null;
        private IFunctionRasterDataset transRs = null;
        private double pct = 0.1;
        private IFeatureClass mskFtrCls = null;
        private IGeometry geo = null;
        private int n = 1000;
        private IPoint[] rndPnts = null;
        private double[] minArrayRef = null;
        private double[] maxArrayRef = null;
        private double[] minArrayTran = null;
        private double[] maxArrayTran = null;
        private double[][] coef = null;
        private string outPth = null;
        private string stXY = null;

        public IFunctionRasterDataset normalize()
        {
            //Console.WriteLine("creating points");
            checkSR();
            if(stXY!=null)
            {
                writeXY();
            }
            //Console.WriteLine("extracting array values");
            getValues();
            //Console.WriteLine("scaling and getting unchanged pixels");
            getUnchangedCells();
            //Console.WriteLine("getting coefficients");
            coef = getCoef();//intercept, slope, sse, r2 by band
            if (outPth != null)
            {
                writeCoef();
            }
            //Console.WriteLine("transforming values");
            IFunctionRasterDataset outRs = transform();
            
            return outRs;
        }

        private void writeXY()
        {
            IRaster2 rs2 = (IRaster2)rsUtil.createRaster(transRs);
            double conv = (refRs.RasterInfo.CellSize.X / transRs.RasterInfo.CellSize.X);
            int offSetCells = System.Convert.ToInt32(conv * bSize / 2);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(stXY))
            {
                sw.WriteLine("X,Y");
                foreach (IPoint p in rndPnts)
                {
                    sw.WriteLine(p.X.ToString() + "," + p.Y.ToString());
                    int c, r;
                    rs2.MapToPixel(p.X, p.Y, out c, out r);
                    double nx, ny;
                    IPnt tl = new PntClass();
                    tl.X = c - offSetCells;
                    tl.Y = r - offSetCells;
                    rs2.PixelToMap(System.Convert.ToInt32(tl.X), System.Convert.ToInt32(tl.Y), out nx, out ny);
                    sw.WriteLine(nx.ToString() + "," + ny.ToString());
                    IPnt br = new PntClass();
                    br.X = c + offSetCells;
                    br.Y = r + offSetCells;
                    rs2.PixelToMap(System.Convert.ToInt32(br.X), System.Convert.ToInt32(br.Y), out nx, out ny);
                    sw.WriteLine(nx.ToString() + "," + ny.ToString());
                    IPnt tr = new PntClass();
                    tr.X = c - offSetCells;
                    tr.Y = r + offSetCells;
                    rs2.PixelToMap(System.Convert.ToInt32(tr.X), System.Convert.ToInt32(tr.Y), out nx, out ny);
                    sw.WriteLine(nx.ToString() + "," + ny.ToString());
                    IPnt bl = new PntClass();
                    bl.X = c + offSetCells;
                    bl.Y = r - offSetCells;
                    rs2.PixelToMap(System.Convert.ToInt32(bl.X), System.Convert.ToInt32(bl.Y), out nx, out ny);
                    sw.WriteLine(nx.ToString() + "," + ny.ToString());
                }
                sw.Close();
            }
        }

        private void writeCoef()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outPth))
            {
                sw.WriteLine("Normalize");
                sw.WriteLine(pct.ToString());
                sw.WriteLine(refRs.RasterInfo.PixelType.ToString());
                sw.WriteLine(coef.Length.ToString());
                for (int i = 0; i < coef.Length; i++)
                {
                    sw.WriteLine(String.Join(",", (from double d in coef[i] select d.ToString()).ToArray()));
                }
                sw.Close();
            }
        }

        public IFunctionRasterDataset normalize(string mdlPath)
        {
            readCoef(mdlPath);
            IFunctionRasterDataset outRs = transform();
            return outRs;
        }

        private void readCoef(string mdlPath)
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(outPth))
            {
                sr.ReadLine();
                double pct  = System.Convert.ToSingle(sr.ReadLine());
                sr.ReadLine();
                int bnds = System.Convert.ToInt32(sr.ReadLine());
                coef = new double[bnds][];
                for (int i = 0; i < coef.Length; i++)
                {
                    string[] cArr = sr.ReadLine().Split(new char[',']);
                    coef[i] = (from string s in cArr select System.Convert.ToDouble(s)).ToArray(); 
                }
                sr.Close();
            }
        }

        private IFunctionRasterDataset transform()
        {
            IFunctionRasterDataset outRs = null;
            IRasterBandCollection rsBc = new RasterClass();
            for (int i = 0; i < coef.Length; i++)
            {
                double[] c = coef[i];
                double intercept = c[0];
                double slope = c[1];
                IFunctionRasterDataset tRs = rsUtil.getBand(transRs, i);
                IFunctionRasterDataset pRs = rsUtil.calcArithmaticFunction(tRs, slope, esriRasterArithmeticOperation.esriRasterMultiply);
                IFunctionRasterDataset fRs = rsUtil.calcArithmaticFunction(pRs, intercept, esriRasterArithmeticOperation.esriRasterPlus);
                IFunctionRasterDataset bRs = rsUtil.convertToDifFormatFunction(fRs, refRs.RasterInfo.PixelType);
                rsBc.AppendBand(((IRasterBandCollection)bRs).Item(0));
            }
            outRs = rsUtil.compositeBandFunction(rsBc);
            return outRs;
        }

        private double[][] getCoef()
        {
            double[][] outCoef = new double[useArray.Length][];
            int cellCntCheck = bSize * bSize / 4;
            for (int b = 0; b < useArray.Length; b++)
            {

                double[] xVls = new double[rndPnts.Length];
                double[] yVls = new double[rndPnts.Length];
                int[] cntVls = new int[rndPnts.Length];
                for (int r = 0; r < useArray[b].Length; r++)
                {
                    for (int c = 0; c < useArray[b][r].Length; c++)
                    {

                        if (useArray[b][r][c])
                        {
                            xVls[r] = xVls[r] + tranArray[b][r][c];
                            yVls[r] = yVls[r] + refArray[b][r][c];
                            cntVls[r] = cntVls[r] + 1;
                        }

                    }
                }
                List<double> xVlsLst = new List<double>();
                List<double> yVlsLst = new List<double>();
                List<int> cntVlsLst = new List<int>();
                for (int v = 0; v < xVls.Length; v++)
                {
                    //Console.WriteLine(cntVls[v].ToString());
                    //Console.WriteLine(xVls[v].ToString());
                    //Console.WriteLine(yVls[v].ToString());
                    int cnt = cntVls[v];
                    if (cnt > cellCntCheck)
                    {
                        xVlsLst.Add(xVls[v] / cnt);
                        yVlsLst.Add(yVls[v] / cnt);
                        cntVlsLst.Add(cnt);
                    }
                }
                xVls = xVlsLst.ToArray();
                yVls = yVlsLst.ToArray();
                cntVls = cntVlsLst.ToArray();
                Accord.Statistics.Models.Regression.Linear.SimpleLinearRegression slr = new Accord.Statistics.Models.Regression.Linear.SimpleLinearRegression();
                double sse = slr.Regress(xVls, yVls);
                double r2 = slr.CoefficientOfDetermination(xVls, yVls);
                outCoef[b] = new double[4]{ slr.Intercept, slr.Slope, sse,r2};
                if (stXY != null)
                {
                    string pCoef = stXY.Replace(".csv", "_" + b.ToString() + ".txt");
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(pCoef))
                    {
                        sw.WriteLine("X,Y,CNT");
                        for (int v = 0; v < xVls.Length; v++)
                        {
                            sw.WriteLine(xVls[v].ToString() + "," + yVls[v].ToString() + "," + cntVls[v].ToString());

                        }
                        sw.Close();
                    }

                }
            }
            
            return (outCoef);
        }

        private void getUnchangedCells()
        {
            Random rnd = new Random();
            double[][][] difArr = new double[refArray.Length][][];
            for (int b = 0; b < refArray.Length; b++)
            {
                List<double> difLst = new List<double>();
                difArr[b] = new double[refArray[b].Length][];
                double minVal = minArrayRef[b];
                double maxVal = maxArrayRef[b];
                double dif = maxVal - minVal;
                double tMinVal = minArrayTran[b];
                double tMaxVal = maxArrayTran[b];
                double tdif = tMaxVal - tMinVal;
                for (int r = 0; r < refArray[b].Length; r++)
                {
                    difArr[b][r] = new double[refArray[b][r].Length];
                    for (int c = 0; c < refArray[b][r].Length; c++)
                    {
                        double refOldVl = refArray[b][r][c];
                        double tranOldVl = tranArray[b][r][c];
                        if (!(refOldVl == -9999 || tranOldVl == -9999))
                        {
                            double refNewVl = (refOldVl - minVal) / dif;
                            double tranNewVl = (tranOldVl - minVal) / tdif;
                            double sdif = refNewVl - tranNewVl;
                            difArr[b][r][c] = sdif;
                            if (rnd.NextDouble() <= 0.1)
                            {
                                difLst.Add(sdif);
                            }
                        }
                        else
                        {
                            difArr[b][r][c] = -9999;
                        }
                    }
                }
                difLst.Sort();
                int vlIndex = System.Convert.ToInt32(difLst.Count*pct);
                double smallValue = difLst[vlIndex];
                double largeValue = difLst[difLst.Count-vlIndex];
                for (int r = 0; r < difArr[b].Length; r++)
                {
                    for (int c = 0; c < difArr[b][r].Length; c++)
                    {
                        double vl = difArr[b][r][c];
                        if(vl<largeValue&&vl>smallValue)
                        {
                            useArray[b][r][c] = true;
                        }
                    }
                }

            }
        }

        private int bSize = 50;
        private double[][][] refArray = null;
        private double[][][] tranArray = null;
        private bool[][][] useArray = null;
        private void getValues()
        {
            int bSizeCnt = bSize * bSize;
            for (int i = 0; i < refArray.Length; i++)
            {
                refArray[i] = new double[n][];
                tranArray[i] = new double[n][];
                useArray[i] = new bool[n][];
                for (int j = 0; j < n; j++)
                {
                    refArray[i][j] = new double[bSizeCnt];
                    tranArray[i][j] = new double[bSizeCnt];
                    useArray[i][j] = new bool[bSizeCnt];
                }
            }
            int offSetCells = bSize / 2; 
            IRaster2 rs2 = (IRaster2)rsUtil.createRaster(refRs);
            IRaster rs = (IRaster)rs2;
            int nIndex = 0;
            foreach(IPoint pnt in rndPnts)
            {
                fillTransValues(pnt,nIndex);
                IPnt tlPnt = new PntClass();
                IPnt pbSize = new PntClass();
                pbSize.X = bSize;
                pbSize.Y = bSize;
                int c, r;
                rs2.MapToPixel(pnt.X, pnt.Y, out c, out r);
                c = c - offSetCells;
                r = r - offSetCells;
                tlPnt.X = c;
                tlPnt.Y = r;
                IPixelBlock pb = rs.CreatePixelBlock(pbSize);
                rs.Read(tlPnt, pb);
                for (int b = 0; b < pb.Planes; b++)
                {
                    System.Array vlArr = (System.Array)((IPixelBlock3)pb).get_PixelData(b);
                    int vIndex = 0;
                    for (int rw = 0; rw < pb.Height; rw++)
                    {
                        for (int cl = 0; cl < pb.Width; cl++)
                        {
                            object vlObj = vlArr.GetValue(rw, cl);
                            double vlf = -9999;
                            if(vlObj!=null)
                            {
                                vlf = System.Convert.ToDouble(vlObj);
                                if (vlf < minArrayRef[b]) minArrayRef[b] = vlf;
                                if (vlf > maxArrayRef[b]) maxArrayRef[b] = vlf;
                            }
                            refArray[b][nIndex][vIndex] = vlf;
                            vIndex = vIndex + 1;
                        }
                    }
                }
                nIndex = nIndex + 1;
            }
        }

        private void fillTransValues(IPoint pnt, int nIndex)
        {
            IRaster rs = rsUtil.createRaster(transRs);
            IRaster2 rs2 = (IRaster2)rs;
            double conv = (refRs.RasterInfo.CellSize.X / transRs.RasterInfo.CellSize.X);
            int offSetCells = System.Convert.ToInt32(conv*bSize/2);
            IPnt tlPnt = new PntClass();
            IPnt pbSize = new PntClass();
            pbSize.X = System.Convert.ToInt32(bSize*conv);
            pbSize.Y = System.Convert.ToInt32(bSize*conv);
            int c, r;
            rs2.MapToPixel(pnt.X, pnt.Y, out c, out r);
            c = c - offSetCells;
            r = r - offSetCells;
            tlPnt.X = c;
            tlPnt.Y = r;
            IPixelBlock pb = rs.CreatePixelBlock(pbSize);
            rs.Read(tlPnt, pb);
            for (int b = 0; b < pb.Planes; b++)
            {
                System.Array vlArr = (System.Array)((IPixelBlock3)pb).get_PixelData(b);
                int vIndex = 0;
                double[] cellCnt = new double[bSize*bSize];
                for (int rw = 0; rw < pb.Height; rw++)
                {
                    for (int cl = 0; cl < pb.Width; cl++)
                    {
                        object vlObj = vlArr.GetValue(rw, cl);
                        if (vlObj != null)
                        {
                            int bIndex = (int)(rw/conv) * bSize + (int)(cl / conv);
                            double vlf = System.Convert.ToSingle(vlObj);
                            tranArray[b][nIndex][bIndex] = vlf + tranArray[b][nIndex][bIndex];
                            cellCnt[bIndex] = cellCnt[bIndex] + 1;
                        }
                        vIndex = vIndex + 1;
                    }
                }
                for(int i=0; i < cellCnt.Length;i++)
                {
                    double cellCntVl = cellCnt[i];
                    if (cellCntVl > 0)
                    {
                        double meanValue = tranArray[b][nIndex][i] / cellCnt[i];
                        tranArray[b][nIndex][i] = meanValue;
                        if (meanValue < minArrayTran[b]) minArrayTran[b] = meanValue;
                        if (meanValue > maxArrayTran[b]) maxArrayTran[b] = meanValue;

                    }
                    else
                    {
                        tranArray[b][nIndex][i] = -9999;
                    }
                }
            }
        }


        private void checkSR()
        {
            ISpatialReference srRef = refRs.RasterInfo.SpatialReference;
            ISpatialReference srTrans = transRs.RasterInfo.SpatialReference;
            ISpatialReference srFtrCls = null;
            ITopologicalOperator tp;
            if (mskFtrCls != null)
            {
                srFtrCls = ((IGeoDataset)mskFtrCls).SpatialReference;
                IGeometryCollection geoColl = new PolygonClass();
                object obj = Type.Missing;
                IFeatureCursor ftrCur = mskFtrCls.Search(null, true);
                IFeature ftr = ftrCur.NextFeature();
                geo = ftr.ShapeCopy;
                tp = (ITopologicalOperator)geo;
                ftr = ftrCur.NextFeature();
                while (ftr != null)
                {
                    IGeometry geo2 = ftr.ShapeCopy;
                    geo = tp.Union(geo2);
                    tp = (ITopologicalOperator)geo;
                    ftr = ftrCur.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ftrCur);
                if(srRef.FactoryCode != srFtrCls.FactoryCode)
                {
                    geo.Project(srRef);
                }
                tp = (ITopologicalOperator)geo;
                geo = tp.Intersect((IGeometry)refRs.RasterInfo.Extent, esriGeometryDimension.esriGeometry2Dimension);
            }
            else
            {
                IEnvelope env = refRs.RasterInfo.Extent;
                IGeometryBridge2 geoBr = new GeometryEnvironmentClass();
                IPointCollection4 pntCol = new PolygonClass();
                ((IGeometry)pntCol).SpatialReference = refRs.RasterInfo.SpatialReference;
                object mis = Type.Missing;
                pntCol.AddPoint(env.UpperLeft);
                pntCol.AddPoint(env.UpperRight);
                pntCol.AddPoint(env.LowerRight);
                pntCol.AddPoint(env.LowerLeft);
                ((IPolygon)pntCol).Close();
                geo = (IGeometry)pntCol;
                //Console.WriteLine("Area = " + ((IArea)geo).Area.ToString());
            }
            if(srRef.FactoryCode!=srTrans.FactoryCode)
            {
                transRs = rsUtil.reprojectRasterFunction(transRs, srRef);
            }

          
            //intersect with boundary of Trans Raster
            tp = (ITopologicalOperator)geo;
            geo = tp.Intersect((IGeometry)transRs.RasterInfo.Extent, esriGeometryDimension.esriGeometry2Dimension);
            //buffer inside raster half block size
            tp = (ITopologicalOperator)geo;
            geo = tp.Buffer(-1 * bSize * refRs.RasterInfo.CellSize.X / 2);
            IRaster2 rs2 = (IRaster2)rsUtil.createRaster(refRs);
            IPoint ulPnt = geo.Envelope.UpperLeft;
            IPoint lrPnt = geo.Envelope.LowerRight;
            int rStartClm, rStartRw;
            rs2.MapToPixel(ulPnt.X, ulPnt.Y, out rStartClm, out rStartRw);
            int endClm, endRw;
            rs2.MapToPixel(lrPnt.X, lrPnt.Y, out endClm, out endRw);
            int tCells = (endClm - rStartClm) * (endRw - rStartRw);
            double px, py;
            if(n>=(tCells*0.5)) //get all cells from ref Raster
            {
                int iCnt = 0;
                rndPnts = new IPoint[tCells];
                for (int c = rStartClm; c <= endClm; c++)
                {
                    for (int r = rStartRw; r <= endRw ; r++)
                    {
                        rs2.PixelToMap(c, r, out px, out py);
                        IPoint pnt = new PointClass();
                        pnt.PutCoords(px, py);
                        rndPnts[iCnt] = pnt;
                    }
                    iCnt = iCnt + 1;
                }
            }
            else //randomly chose cells from ref Raster up to n 
            {
                Random rnd = new Random();
                HashSet<string> sCheck = new HashSet<string>();
                int iCnt = 0;
                rndPnts = new IPoint[n];
                while (sCheck.Count < n)
                {
                    int clm = rnd.Next(rStartClm, endClm);
                    int rw = rnd.Next(rStartRw, endRw);
                    string rwclm = rw.ToString() + "_" + clm.ToString();
                    if (!sCheck.Contains(rwclm))
                    {
                        rs2.PixelToMap(clm, rw, out px, out py);
                        IPoint pnt = new PointClass();
                        pnt.PutCoords(px, py);
                        IRelationalOperator ro = (IRelationalOperator)pnt;
                        if (ro.Within(geo))
                        {
                            rndPnts[iCnt] = pnt;
                            iCnt = iCnt + 1;
                            sCheck.Add(rwclm);
                        }
                    }
                }
            }
            n = rndPnts.Length;

        }
    }
}
