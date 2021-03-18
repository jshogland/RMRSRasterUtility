using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesRaster;

namespace esriUtil
{
    public enum Design { Random, Systematic, FIA }
    public class SampleSimulation
    {
        public SampleSimulation(IFunctionRasterDataset inLayer, string outPath, double plotsPerAcre = 1, double plotArea = 0.1, int iterations = 100, Design designType = Design.Random, bool continuous = true, rasterUtil rasterUtility = null, bool createLocations = false,esriUtil.Forms.RunningProcess.frmRunningProcessDialog runningProcess=null)
        {
            rsUtil = rasterUtility;
            fDset = inLayer;
            outputPath = outPath;
            iter = iterations;
            pArea = plotArea;
            ppAcre = plotsPerAcre;
            radius = Math.Sqrt(pArea * 43560 / Math.PI) / 3.281;
            dType = designType;
            cont = continuous;
            createLoc = createLocations;
            rp = runningProcess;
        }
        public SampleSimulation(IFeatureClass inLayer, string fieldName, string outPath, double plotsPerAcre = 1, double plotArea = 0.1, int iterations = 100, Design designType = Design.Random, bool continuous = true, rasterUtil rasterUtility = null, bool createLocations = false, esriUtil.Forms.RunningProcess.frmRunningProcessDialog runningProcess = null)
        {
            rsUtil = rasterUtility;
            ftrCls = inLayer;
            fldName=fieldName;
            outputPath = outPath;
            iter = iterations;
            pArea = plotArea;
            ppAcre = plotsPerAcre;
            radius = Math.Sqrt(pArea * 43560 / Math.PI) / 3.281;
            dType = designType;
            cont = continuous;
            createLoc = createLocations;
            rp = runningProcess;
        }
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = null;
        private IFeatureClass ftrCls = null;
        private IFunctionRasterDataset fDset = null;
        private string fldName = "";
        private string outputPath = "";
        private int iter = 100;
        private double pArea = 0.1;
        private double radius = 11.349689;
        private double ppAcre = 1;
        private Design dType = Design.Random;
        private bool cont = true;
        private IEnvelope env = null;
        private double convFactor = 1;
        private Dictionary<string, double> unDic = new Dictionary<string, double>();
        private double grandMean = 0;
        private bool createLoc = false;
        private esriUtil.Forms.RunningProcess.frmRunningProcessDialog rp = null;
 
        public void runSimulation()
        {
            ISpatialReference sr;
            env = getEnvelope(out sr);
            convFactor = ((IProjectedCoordinateSystem)sr).CoordinateUnit.ConversionFactor;
            radius = radius * convFactor;
            double eAcres = ((env.Width * convFactor) * (env.Height * convFactor)) * 0.000247105;
            int n = System.Convert.ToInt32(eAcres * ppAcre);
            Console.WriteLine("n = " + n.ToString());
            if(rp!=null)
            {
                rp.addMessage("n = " + n.ToString());
            }
            Console.WriteLine("Radius = " + radius);
            if (rp != null)
            {
                rp.addMessage("Radius = " + radius);
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputPath))
            {
                string ln = getFirstLine();
                //Console.WriteLine(ln);
                string[] tlnArr = ln.Split(new char[]{','});
                //Console.WriteLine(tlnArr[0] + "_" +  tlnArr[1]);
                int lng = tlnArr.Length - 1;
                //Console.WriteLine(lng);
                sw.WriteLine(ln);
                double[] tmeanArr = getTrueMeans();
                double[] sumLst = new double[lng];
                double[] sumLst2 = new double[lng];
                double[] sumLst22 = new double[lng];
                double[] sumLstDif = new double[lng];
                for (int i = 1; i <= iter; i++)
                {
                    Console.WriteLine("Working on simulation " + i.ToString());
                    if (rp != null)
                    {
                        rp.addMessage("Working on simulation " + i.ToString());
                        rp.stepPGBar(5);
                    }
                    IPoint[] spnt = getPnts(n,env);
                    if (createLoc)
                    {
                        savePoints(spnt, "pt_" + i.ToString());
                    }
                    string pln = getSummaryValues(spnt);
                    string[] plnArr = pln.Split(new char[]{','});
                    for (int k = 0; k < lng; k++)
                    {
                        double dVl = System.Convert.ToDouble(plnArr[k]);
                        sumLstDif[k] = sumLstDif[k] + dVl;
                        sumLst[k] = sumLst[k] + Math.Pow(dVl,2);
                        double smean = tmeanArr[k] + dVl;
                        sumLst2[k] = sumLst2[k] + smean;
                        sumLst22[k] = sumLst22[k] + Math.Pow(smean, 2);
                    }
                    ln = i.ToString() + "," + pln;
                    sw.WriteLine(ln);
                }

                string[] seArr = new string[lng];
                string[] mArr = new string[lng];
                string[] stdArr = new string[lng];
                for (int k = 0; k < lng; k++)
                {
                    double rmse = Math.Sqrt(sumLst[k] / (iter-1));
                    //Console.WriteLine("Rmse " + rmse.ToString());
                    seArr[k] = rmse.ToString();
                    double m = sumLst2[k]/iter;
                    //Console.WriteLine("m " + m.ToString());
                    mArr[k] = m.ToString();
                    double m2 = sumLst22[k];
                    //Console.WriteLine("m2 " + m2.ToString());
                    double std = Math.Sqrt((m2 - (Math.Pow(sumLst2[k],2)/iter))/(iter-1));
                    //Console.WriteLine("std " + std.ToString());
                    stdArr[k] = std.ToString();
                    
                }
                
                ln = "TrueMean," + String.Join(",",(from double d in tmeanArr select d.ToString()).ToArray());
                sw.WriteLine(ln);
                ln = "Mean_Dif_From_True," + String.Join(",", (from double d in sumLstDif select (d/iter).ToString()).ToArray()); ;
                sw.WriteLine(ln);
                ln = "SE_From_True," + String.Join(",", seArr);
                sw.WriteLine(ln);
                ln = "SampleMean," + String.Join(",", mArr);
                sw.WriteLine(ln);
                ln = "SE_Mean," + String.Join(",", stdArr);
                sw.WriteLine(ln);
                sw.Close();
            }
            
        }

        private double[] getTrueMeans()
        {
            double[] outArr;
            if (cont)
            {
                outArr = new double[1];
                outArr[0] = grandMean;
            }
            else
            {
                outArr = unDic.Values.ToArray();
            }
            return outArr;

        }

        private void savePoints(IPoint[] spnt,string name)
        {
            string frOut = System.IO.Path.GetDirectoryName(outputPath) + "\\" + name + ".shp";
            ISpatialReference sr = null;
            if(fDset!=null)
            {
                sr = fDset.RasterInfo.SpatialReference;
            }
            else
            {
                sr = ((IGeoDataset)ftrCls).SpatialReference;
            }
            IFeatureClass tFtrCls = geoUtil.createFeatureClass(frOut, null, esriGeometryType.esriGeometryPoint, sr);
            IFeatureCursor ftrCur = tFtrCls.Insert(true);
            IFeatureBuffer ftrBuff = tFtrCls.CreateFeatureBuffer();
            foreach (IPoint pt in spnt)
            {
                ftrBuff.Shape = pt;
                ftrCur.InsertFeature(ftrBuff);
            }
        }

        private string getSummaryValues(IPoint[] pnts)
        {
            if(cont)
            {
               return getMeans(pnts);
            }
            else
            {
               return getProp(pnts);
            }
        }

        private string getProp(IPoint[] pnts)
        {
            if (ftrCls != null)
            {
                return getFtrClassProp(pnts);
            }
            else
            {
                return getRasterProps(pnts);
            }
        }

        private string getRasterProps(IPoint[] pnts)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            double cellSize = fDset.RasterInfo.CellSize.X;
            IRaster rs = rsUtil.createRaster(fDset);
            int dCells = System.Convert.ToInt32(radius * 2 / cellSize);
            if (dCells < 1)
            {
                dCells = 1;
            }
            double[,] mask = createMask(dCells);
            IPnt pntSize = new PntClass();
            pntSize.X = dCells;
            pntSize.Y = dCells;
            IPixelBlock pBlock = rs.CreatePixelBlock(pntSize);
            IPnt pntLoc = new PntClass();
            foreach (IPoint pt in pnts)
            {
                pntLoc.SetCoords(pt.X, pt.Y);
                rs.Read(pntLoc, pBlock);
                for (int r = 0; r < pBlock.Height; r++)
                {
                    for (int c = 0; c < pBlock.Width; c++)
                    {
                        double vlMask = mask[c, r];
                        if (vlMask > 0)
                        {
                            object vlObj = pBlock.GetVal(0, c, r);
                            if (vlObj != null)
                            {
                                string vl = vlObj.ToString();
                                double cnt;
                                if(dic.TryGetValue(vl,out cnt))
                                {
                                    dic[vl] = cnt + 1;
                                }
                                else
                                {
                                    dic.Add(vl, 1);
                                }
                            }
                        }
                    }
                }
            }
            double sArea = dic.Values.Sum();
            double tArea = unDic.Values.Sum();
            List<string> lnArr = new List<string>();
            foreach (KeyValuePair<string, double> kvp in unDic)
            {
                string ky = kvp.Key;
                double ar = kvp.Value;
                double ar2;
                double dArea = 0;
                if (dic.TryGetValue(ky, out ar2))
                {
                    dArea = ((ar2 / sArea) * tArea)-ar;
                }
                else
                {
                    dArea = 0-ar;
                }
                lnArr.Add(dArea.ToString());
            }
            return String.Join(",", lnArr.ToArray());
        }

        private string getFtrClassProp(IPoint[] pnts)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            foreach (IPoint pt in pnts)
            {
                ITopologicalOperator tp = (ITopologicalOperator)pt;
                IGeometry geo = tp.Buffer(radius);
                ISpatialFilter sp = new SpatialFilterClass();
                sp.Geometry = geo;
                sp.SubFields = ftrCls.ShapeFieldName+","+fldName;
                sp.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor ftrCur = ftrCls.Search(sp, true);
                int fldIndex = ftrCur.FindField(fldName);
                IFeature ftrRow = ftrCur.NextFeature();
                while (ftrRow != null)
                {
                    IGeometry geo2 = ftrRow.ShapeCopy;
                    object vlObj = ftrRow.get_Value(fldIndex);
                    string vl = vlObj.ToString();
                    ITopologicalOperator tp2 = (ITopologicalOperator)geo2;
                    IGeometry geo3 = tp2.Intersect(geo, esriGeometryDimension.esriGeometry2Dimension);
                    IArea poly = (IArea)geo3;
                    double cArea = poly.Area;
                    double ar;
                    if(dic.TryGetValue(vl,out ar))
                    {
                        dic[vl] = ar+cArea;
                    }
                    else
                    {
                        dic[vl] = cArea;
                    }
                    ftrRow = ftrCur.NextFeature();
                }
            }
            double sArea = dic.Values.Sum();
            double tArea = unDic.Values.Sum();
            List<string> lnArr = new List<string>();
            foreach(KeyValuePair<string,double> kvp in unDic)
            {
                string ky = kvp.Key;
                double ar = kvp.Value;
                double ar2;
                double dArea = 0;
                if(dic.TryGetValue(ky,out ar2))
                {
                    dArea = ((ar2 / sArea) * tArea)-ar;
                }
                else
                {
                    dArea = 0-ar;
                }
                lnArr.Add(dArea.ToString());
            }
            return String.Join(",",lnArr.ToArray());
        }

        private string getMeans(IPoint[] pnts)
        {
            if(ftrCls!=null)
            {
                return getFtrClassMeans(pnts);
            }
            else
            {
                return getRasterMeans(pnts);
            }
        }

        private string getRasterMeans(IPoint[] pnts)
        {
            double tSumValue = 0;
            double cellSize = fDset.RasterInfo.CellSize.X;
            IRaster rs = rsUtil.createRaster(fDset);
            int dCells = System.Convert.ToInt32(radius*2/cellSize);
            if(dCells<1)
            {
                dCells=1;
            }
            double[,] mask = createMask(dCells);
            IPnt pntSize = new PntClass();
            pntSize.X=dCells;
            pntSize.Y=dCells;
            IPixelBlock pBlock = rs.CreatePixelBlock(pntSize);
            IPnt pntLoc = new PntClass();
            foreach (IPoint pt in pnts)
            {
                pntLoc.SetCoords(pt.X, pt.Y);
                rs.Read(pntLoc, pBlock);
                double n = 0;
                double sumValue = 0;
                for (int r = 0; r < pBlock.Height; r++)
                {
                    for (int c = 0; c < pBlock.Width; c++)
                    {
                        double vlMask = mask[c, r];
                        if (vlMask > 0)
                        {
                            object vlObj = pBlock.GetVal(0, c, r);
                            if (vlObj != null)
                            {
                                double vl =System.Convert.ToDouble(vlObj);
                                sumValue = sumValue + vl;
                                n = n + 1;

                            }
                        }
                    }
                }
                tSumValue = tSumValue + (sumValue / n);
            }
            return ((tSumValue/pnts.Length)-grandMean).ToString();
        }

        private double[,] createMask(int dCells)
        {
            double hd = dCells / 2.0;
            double[,] outArra = new double[dCells,dCells];
            for (int r = 0; r < dCells; r++)
            {
                double vt = Math.Pow(((r+0.5) - hd),2);
                for (int c = 0; c < dCells; c++)
                {
                    double hz = Math.Pow(((c+0.5) - hd),2);
                    double td = Math.Sqrt(vt + hz);
                    if(td <= hd)
                    {
                        outArra[r, c] = 1;
                    }
                    else
                    {
                        outArra[r, c] = 0;
                    }
                }
            }
            return outArra;
        }

        private string getFtrClassMeans(IPoint[] pnts)
        {
            double sumValue = 0;
            int cnt = 0;
            foreach (IPoint pt in pnts)
            {
                ITopologicalOperator tp = (ITopologicalOperator)pt;
                IGeometry geo = tp.Buffer(radius);
                //Console.WriteLine("Area = " + ((IArea)geo).Area.ToString());
                ISpatialFilter sp = new SpatialFilterClass();
                sp.Geometry = geo;
                sp.SubFields = ftrCls.ShapeFieldName+","+fldName;
                sp.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                sp.WhereClause = "not " + fldName + " is Null";
                IFeatureCursor ftrCur = ftrCls.Search(sp, true);
                int fldIndex = ftrCur.FindField(fldName);
                IFeature ftrRow = ftrCur.NextFeature();
                double sArea = 0;
                double sValue = 0;
                while (ftrRow != null)
                {
                    IGeometry geo2 = ftrRow.ShapeCopy;
                    object vlObj = ftrRow.get_Value(fldIndex);
                    //Console.WriteLine("Value = " + vlObj.ToString());
                    ITopologicalOperator tp2 = (ITopologicalOperator)geo2;
                    IGeometry geo3 = tp2.Intersect(geo, esriGeometryDimension.esriGeometry2Dimension);
                    IArea poly = (IArea)geo3;
                    double cArea = poly.Area;
                    //Console.WriteLine("CArea = " + cArea.ToString());
                    sArea = sArea + cArea;
                    sValue = sValue + System.Convert.ToDouble(vlObj) * cArea;
                    ftrRow = ftrCur.NextFeature();
                }
                if (sArea > 0)
                {
                    sumValue = sumValue + (sValue / sArea);
                    cnt = cnt + 1;
                }
            }
            return ((sumValue / cnt)-grandMean).ToString();

        }

        private string getFirstLine()
        {
            string ln = "iteration";
            if(cont)
            {
                ln = ln + ",difMean";
                if (fDset != null)
                {
                    IRaster trs = rsUtil.createRaster(fDset);
                    IRasterBandCollection rsbc = (IRasterBandCollection)trs;
                    IRasterBand rsb = rsbc.Item(0);
                    IRasterStatistics rsStats = rsb.Statistics;
                    if (rsStats==null||!rsStats.IsValid)
                    {
                        grandMean = getGrandMean(fDset);
                    }
                    else
                    {
                        grandMean = rsStats.Mean;
                    }
                }
                else
                {
                    IQueryFilter qf = new QueryFilterClass();
                    qf.SubFields = fldName;
                    qf.WhereClause = "not " + fldName + " is NULL";
                    IFeatureCursor ftrCur = ftrCls.Search(qf, true);
                    int vlIndex = ftrCur.FindField(fldName);
                    IFeature ftr = ftrCur.NextFeature();
                    double s = 0;
                    int n = 0;
                    while(ftr!=null)
                    {
                        s = s + System.Convert.ToDouble(ftr.get_Value(vlIndex));
                        ftr = ftrCur.NextFeature();
                        n = n+1;
                    }
                    grandMean = s/n;
                }
               
            }
            else
            {
                if(fDset!=null)
                {
                    IRaster2 rs = (IRaster2)rsUtil.createRaster(fDset);
                    ITable tbl = rs.AttributeTable;
                    if (tbl == null)
                    {
                        Dictionary<int,int> vDic = rsUtil.buildVat(fDset);
                        foreach (KeyValuePair<int,int> kvp in vDic)
                        {
                            unDic.Add(kvp.Key.ToString(), System.Convert.ToDouble(kvp.Value));
                        }
                    }
                    else
                    {
                        ICursor cur = tbl.Search(null, true);
                        int vlIndex = cur.FindField("VALUE");
                        int cntIndex = cur.FindField("COUNT");
                        IRow rw = cur.NextRow();
                        while (rw!=null)
                        {
                            unDic.Add(rw.get_Value(vlIndex).ToString(),System.Convert.ToDouble(rw.get_Value(cntIndex)));
                            rw = cur.NextRow();
                        }
                    }

                }
                else if(ftrCls!=null)
                {
                    IQueryFilter qf = new QueryFilterClass();
                    qf.SubFields = ftrCls.ShapeFieldName + "," + fldName;
                    qf.WhereClause = "not " + fldName + " is NULL";
                    IFeatureCursor fCur = ftrCls.Search(qf, true);
                    int vlIndex = fCur.FindField(fldName);
                    IFeature ftr = fCur.NextFeature();
                    while(ftr!=null)
                    {
                        string vl = ftr.get_Value(vlIndex).ToString();
                        //Console.WriteLine(vl);
                        IGeometry geo = ftr.ShapeCopy;
                        if (geo != null)
                        {
                            IArea shpAr = (IArea)geo;
                            double ar2 = shpAr.Area;
                            double ar;
                            if (unDic.TryGetValue(vl, out ar))
                            {
                                unDic[vl] = ar + ar2;
                            }
                            else
                            {
                                unDic.Add(vl, ar2);
                            }
                        }
                        else
                        {
                            //Console.WriteLine("Geo = Null");
                        }
                        ftr = fCur.NextFeature();
                    }
                }
                ln = ln + "," + String.Join(",", unDic.Keys.ToArray());
            }
            return ln;
        }

        private double getGrandMean(IFunctionRasterDataset fDset)
        {
            IRaster rs = (IRaster)rsUtil.createRaster(fDset);
            IRasterCursor rsCur = rs.CreateCursor();
            double sumVl = 0;
            int cnt = 0;
            while (rsCur.Next())
            {
                IPixelBlock pb = rsCur.PixelBlock;
                for (int c = 0; c < pb.Width; c++)
                {
                    for (int r = 0; r < pb.Height; r++)
                    {
                        object objVl = pb.GetVal(0, c, r);
                        if(objVl!=null)
                        {
                            double vl = System.Convert.ToDouble(objVl);
                            sumVl = sumVl + vl;
                            cnt = cnt + 1;
                        }
                    }
                }
            }
            double outVl = 0;
            if(cnt>0)
            {
                outVl = sumVl / cnt;
            }
            return outVl;
        }

        private IPoint[] getPnts(int n,IEnvelope env)
        {
            IPoint[] outPnt = null;
            switch (dType)
            {
                case Design.Random:
                    outPnt = getRndPnt(n,env);
                    break;
                case Design.Systematic:
                    outPnt = getSysPnt(n, env);
                    break;
                case Design.FIA:
                    outPnt = getFiaPnt(n, env);
                    break;
                default:
                    break;
            }
            return outPnt;
        }

        private IPoint[] getFiaPnt(int n, IEnvelope env)
        {
            IPoint[] tpnts =  getSysPnt(n, env, true);
            List<IPoint> outPnts = new List<IPoint>();
            double cv = 36.57421518/convFactor; //meters 36.57421518
            if (fDset!=null)
            {
                cv = System.Convert.ToInt32((fDset.RasterInfo.CellSize.X * convFactor)/36.57421518); //cells
            }
            //Console.WriteLine("Conversion Factor = " + cv);
            foreach(IPoint pnt in tpnts)
            {
                outPnts.Add(pnt);
                foreach(double az in new double[]{360,120,240})
                {
                    IPoint pnt2 = new PointClass();
                    double nX = (System.Math.Sin(az * Math.PI / 180) * cv);
                    double nY = (System.Math.Cos(az * Math.PI / 180) * cv);
                    if(fDset==null)
                    {
                        pnt2.X = pnt.X + System.Convert.ToInt32(nX);
                        pnt2.Y = pnt.Y + System.Convert.ToInt32(nY);
                    }
                    else
                    {
                        pnt2.X=pnt.X + nX;
                        pnt2.Y=pnt.Y + nY;
                    }
                    outPnts.Add(pnt2);
                }
            }
            return outPnts.ToArray();
        }

        private IPoint[] getSysPnt(int n, IEnvelope env, bool offset=false)
        {
            List<IPoint> outPnts = new List<IPoint>();
            double tArea = env.Width * env.Height;
            double pArea = tArea / n;
            Random rnd = new Random();
            if(ftrCls!=null)
            {
                double sk = Math.Sqrt(pArea);
                Console.WriteLine("SK = " + sk.ToString());
                double rx = rnd.NextDouble() * sk + env.XMin;
                double x = rx;
                double ry = rnd.NextDouble() * sk + env.YMin;
                ISpatialFilter spf = new SpatialFilterClass();
                spf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                spf.GeometryField = ftrCls.ShapeFieldName;
                for (int c = 0; c < System.Convert.ToInt32(env.Width/sk); c++)
                {
                    double y = ry;
                    if(offset && c%2==0)
                    {
                        y = y + (sk / 2);
                    }
                    for (int r = 0; r < System.Convert.ToInt32(env.Height/sk); r++)
                    {
                        IPoint pnt = new PointClass();
                        pnt.X = x;
                        pnt.Y = y;
                        spf.Geometry = pnt;
                        int fcnt = ftrCls.FeatureCount(spf);
                        if (fcnt > 0)
                        {
                            outPnts.Add(pnt);
                        }
                        y = y + sk;
                    }
                    x = x + sk;
                }
            }
            else if (fDset!=null)
            {
                IRaster2 rs2 = (IRaster2)rsUtil.createRaster(fDset);
                int w = fDset.RasterInfo.Width;
                int h = fDset.RasterInfo.Height;
                int ski= System.Convert.ToInt32(Math.Sqrt(w*h/n));
                int rx = System.Convert.ToInt32(rnd.NextDouble() * ski);
                int ry = System.Convert.ToInt32(rnd.NextDouble() * ski);
                int cnt = 0;
                for (int c = rx; c < w; c+= ski)
                {
                    int r2 = 0;
                    if (offset && (cnt % 2 == 0))
                    {
                        r2 = System.Convert.ToInt32(ski / 2);
                    }
                    int rstart = r2 + ry;
                    int rend = h + r2;
                    for (int r = rstart; r < rend; r+= ski)
                    {
                        IPoint pnt = new PointClass();
                        pnt.X = c;
                        pnt.Y = r;
                        object outVl = rs2.GetPixelValue(0, c, r);
                        if (outVl != null)
                        {
                            outPnts.Add(pnt);
                        }
                    }
                    cnt = cnt + 1;
                    
                }

            }
            return outPnts.ToArray();
        }

        private IPoint[] getRndPnt(int n, IEnvelope env)
        {
            List<IPoint> outPnts=new List<IPoint>();
            if(ftrCls!=null)
            {
                Random rnd = new Random();
                ISpatialFilter spf = new SpatialFilterClass();
                spf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                spf.GeometryField = ftrCls.ShapeFieldName;
                for (int i = 0; i < n; i++)
                {
                    double x = (rnd.NextDouble() * env.Width) + env.XMin;
                    double y = (rnd.NextDouble() * env.Height) + env.YMin;
                    IPoint pnt = new PointClass();
                    pnt.X = x;
                    pnt.Y = y;
                    spf.Geometry = pnt;
                    int fcnt = ftrCls.FeatureCount(spf);
                    if(fcnt>0)
                    {
                        outPnts.Add(pnt);
                    }
                }

            }
            else if (fDset!=null)
            {
                Random rnd = new Random();
                int mx = fDset.RasterInfo.Width;
                int my = fDset.RasterInfo.Height;
                IRaster2 rs = (IRaster2)rsUtil.createRaster(fDset);
                for (int i = 0; i < n; i++)
                {
                    int x = rnd.Next(0, mx);
                    int y = rnd.Next(0, my);
                    IPoint pnt = new PointClass();
                    pnt.X = x;
                    pnt.Y = y;
                    object outVl = rs.GetPixelValue(0, x, y);
                    if(outVl!=null)
                    {
                        outPnts.Add(pnt);
                    }
                }
            }
            return outPnts.ToArray();
        }

        private IEnvelope getEnvelope(out ISpatialReference sr)
        {
            IEnvelope outEnv = null;
            if(ftrCls !=null)            {
                IGeoDataset geoDs = (IGeoDataset)ftrCls;
                outEnv = geoDs.Extent;
                sr = geoDs.SpatialReference;
            }
            else
            {
                outEnv = fDset.RasterInfo.Extent;
                sr = fDset.RasterInfo.SpatialReference;
            }
            return outEnv;
        }
    }
}

