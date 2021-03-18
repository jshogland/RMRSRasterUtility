﻿using System;
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
    public class normalization
    {
        public normalization(string mdlPath, IFunctionRasterDataset TransformRaster)
        {
            rsUtil = new rasterUtil();
            transformRaster = TransformRaster;
            getDaModel(mdlPath);
        }
        public normalization(IFunctionRasterDataset ReferenceRaster, IFunctionRasterDataset TransformRaster, int PercentChange = 20, rasterUtil rasterUtility = null)
        {
            referenceRaster = ReferenceRaster;
            IRasterBandCollection rsBc = (IRasterBandCollection)referenceRaster;
            rsType = referenceRaster.RasterInfo.PixelType;
            cellCount = new int[rsBc.Count];
            minArray = new double[rsBc.Count];
            maxArray = new double[rsBc.Count];
            sumX2Array = new double[rsBc.Count];
            sumXArray = new double[rsBc.Count];
            sumXYArray = new double[rsBc.Count];
            sumYArray = new double[rsBc.Count];
            sumY2Array = new double[rsBc.Count];
            coef = new double[rsBc.Count][];
            blockCellCount = new int[rsBc.Count];
            difDic = new Dictionary<int, int>[rsBc.Count];
            for (int i = 0; i < rsBc.Count; i++)
            {
                difDic[i] = new Dictionary<int, int>();
            }
            transformRaster = TransformRaster;
            pChange = System.Convert.ToDouble(PercentChange) / 200d;
            rsUtil = rasterUtility;
        }
        private rstPixelType rsType = rstPixelType.PT_UCHAR;
        private rasterUtil rsUtil = null;
        private IFunctionRasterDataset referenceRaster = null;
        private IFunctionRasterDataset transformRaster = null;
        private IFunctionRasterDataset clipRs = null;
        private double pChange;
        public double[][] Coefficients
        {
            get
            {
                return coef;
            }
        }
        public IFunctionRasterDataset ReferenceRaster
        {
            get
            {
                return referenceRaster;
            }
            set
            {
                referenceRaster = value;
            }
        }
        public IFunctionRasterDataset TransformRaster
        {
            get
            {
                return transformRaster;
            }
            set
            {
                transformRaster = value;
            }
        }
        public double PercentAreaChange
        {
            get
            {
                return pChange;
            }
            set
            {
                pChange = value;
            }
        }
        private IFunctionRasterDataset outraster = null;
        public IFunctionRasterDataset OutRaster
        {
            get
            {
                if (outraster == null) normalize();
                return outraster;
            }
            set
            {
                outraster = value;
            }
        }
      
        private IFunctionRasterDataset normalize()
        {
            if (cellCount[0] < 1)
            {
                getUnChangeCells();
                getRegVals();
            }
            return transform();
        }

        private IFunctionRasterDataset transform()
        {
            IRasterBandCollection rsBc = new RasterClass();
            for (int i = 0; i < coef.Length; i++)
            {
                double[] c = coef[i];
                double intercept = c[0];
                double slope = c[1];
                IFunctionRasterDataset tRs = rsUtil.getBand(transformRaster, i);
                IFunctionRasterDataset pRs = rsUtil.calcArithmaticFunction(tRs, slope, esriRasterArithmeticOperation.esriRasterMultiply);
                IFunctionRasterDataset fRs = rsUtil.calcArithmaticFunction(pRs, intercept, esriRasterArithmeticOperation.esriRasterPlus);
                IFunctionRasterDataset bRs = rsUtil.convertToDifFormatFunction(fRs, rsType);
                rsBc.AppendBand(((IRasterBandCollection)bRs).Item(0));
            }
            OutRaster = rsUtil.compositeBandFunction(rsBc);
            return OutRaster;
        }
        private int[] blockCellCount = null;
        private double[][] coef = null; // slope coefficients for each band second double array = {intercept,slope,R2}
        private void getRegVals()
        {
            IRaster2 mRs = (IRaster2)rsUtil.createRaster(rsUtil.clipRasterFunction(referenceRaster, clipGeo, esriRasterClippingType.esriRasterClippingOutside));
            IRaster2 sRs = (IRaster2)rsUtil.createRaster(rsUtil.clipRasterFunction(transformRaster, clipGeo, esriRasterClippingType.esriRasterClippingOutside));
            IPnt pntSize = new PntClass();
            pntSize.SetCoords(250, 250);
            IRasterCursor mRsCur = mRs.CreateCursorEx(pntSize);
            IRasterCursor sRsCur = sRs.CreateCursorEx(pntSize);
            IRasterCursor cRsCur = ((IRaster2)rsUtil.createRaster(clipRs)).CreateCursorEx(pntSize);
            IPixelBlock mPb, sPb, cPb;
            int bndCnt = minArray.Length;
            //int curCnt = 1;
            do
            {
                mPb = mRsCur.PixelBlock;
                sPb = sRsCur.PixelBlock;
                cPb = cRsCur.PixelBlock;
                for (int r = 0; r < cPb.Height; r+=50)
                {
                    for (int c = 0; c < cPb.Width; c+=50)
                    {
                        for (int p = 0; p < bndCnt; p++)
                        {
                            double minVl = minArray[p];
                            double maxVl = maxArray[p];
                            int bCnt = 0;
                            double ySumVl = 0;
                            double xSumVl = 0;
                            int adw = (cPb.Width-c);
                            int adh = (cPb.Height - r);
                            if (adw > 50) adw = 50;
                            if (adh > 50) adh = 50;
                            for (int br = 0; br < adh; br++)
                            {
                                for (int bc = 0; bc < adw; bc++)
                                {
                                    int c2 = c + bc;
                                    int r2 = r + br;
                                    object vlObj = cPb.GetVal(p, c2, r2);
                                    if (vlObj == null)
                                    {
                                        //Console.WriteLine("Clip Not a number");
                                        continue;
                                    }
                                    else
                                    {
                                        double vl = System.Convert.ToDouble(vlObj);
                                        if (vl <= maxVl && vl >= minVl)
                                        {

                                            object mVlObj = mPb.GetVal(p, c2, r2);
                                            object sVlObj = sPb.GetVal(p, c2, r2);
                                            if (mVlObj == null || sVlObj == null)
                                            {
                                                //Console.WriteLine("master or slave is null");
                                                continue;
                                            }
                                            else
                                            {
                                                //Console.WriteLine(mVlObj.ToString() + ", " + sVlObj.ToString());
                                                ySumVl += System.Convert.ToDouble(mVlObj);
                                                xSumVl += System.Convert.ToDouble(sVlObj);
                                                bCnt += 1;
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            if (bCnt == 0) continue;
                            else
                            {
                                double yBlock = ySumVl / bCnt;
                                double xBlock = xSumVl / bCnt;
                                //Console.WriteLine(yBlock.ToString() + ", " + xBlock.ToString());
                                sumYArray[p] = sumYArray[p] + yBlock;
                                sumXArray[p] = sumXArray[p] + xBlock;
                                sumXYArray[p] = sumXYArray[p] + (yBlock * xBlock);
                                sumX2Array[p] = sumX2Array[p] + (xBlock * xBlock);
                                sumY2Array[p] = sumY2Array[p] + (yBlock * yBlock);
                                blockCellCount[p] = blockCellCount[p]+1;
                            }
                        }
                        
                    }
                }
                mRsCur.Next();
                sRsCur.Next();
                //Console.WriteLine(curCnt.ToString());
                //curCnt++;
            } while (cRsCur.Next() == true);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(cRsCur);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(mRsCur);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(sRsCur);
            for (int i = 0; i < bndCnt; i++)
            {
                double n = System.Convert.ToDouble(blockCellCount[i]);
                double meanX = sumXArray[i]/n;
                double meanY = sumYArray[i]/n;
                //double meanX2 = sumX2Array[i]/n;
                //double meanXY = sumXYArray[i]/n;
                //Console.WriteLine("numb of cells = " + n.ToString());
                //Console.WriteLine(meanX.ToString() + ", " + meanY.ToString() + ", " + meanX2.ToString() + ", " + meanXY.ToString());
                double slope = (n * sumXYArray[i] - (sumXArray[i] * sumYArray[i])) / (n * sumX2Array[i] - (System.Math.Pow(sumXArray[i], 2)));
                double intercept = meanY-(slope*meanX);
                double r2 = System.Math.Pow((n * sumXYArray[i] - (sumXArray[i] * sumYArray[i])) / (System.Math.Sqrt((n * sumX2Array[i] - (System.Math.Pow(sumXArray[i], 2))))*System.Math.Sqrt(n*sumY2Array[i] - System.Math.Pow(sumYArray[i],2))),2);
                //Console.WriteLine("Intercept and Slope = " + intercept.ToString() + ", " + slope.ToString());
                coef[i] = new double[3]{intercept,slope,r2};
            }
        }
        private double[] minArray = null;
        private double[] maxArray = null;
        private double[] sumXArray = null;
        private double[] sumYArray = null;
        private double[] sumXYArray = null;
        private double[] sumX2Array = null;
        private double[] sumY2Array = null;
        private Dictionary<int,int>[] difDic = null;
        private int[] cellCount = null;
        private IGeometry clipGeo = null;
        public bool writeModel(string outPath)
        {
            bool finished = true;
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outPath))
                {
                    sw.WriteLine("Normalize");
                    sw.WriteLine(pChange.ToString());
                    sw.WriteLine(rsType.ToString());
                    sw.WriteLine(coef.Length.ToString());
                    for (int i = 0; i < coef.Length; i++)
                    {
                        sw.WriteLine(String.Join(",", (from double d in coef[i] select d.ToString()).ToArray()));
                    }
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                finished = false;
            }
            return finished;
        }
        public bool buildFreqTable(string outPath)
        {
            bool finished = true;
            try
            {
                if (difDic[0].Count < 1)
                {
                    createDictionaryArray();
                }
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outPath))
                {
                    string ln = "band,difference,count";
                    sw.WriteLine(ln);
                    for (int i = 0; i < difDic.Length; i++)
                    {
                        Dictionary<int, int> cDic = difDic[i];
                        foreach (KeyValuePair<int, int> kvp in cDic)
                        {
                            ln = i.ToString() + "," + kvp.Key.ToString() + "," + kvp.Value.ToString();
                            sw.WriteLine(ln);
                        }
                    }
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                finished = false;
                Console.WriteLine(e.ToString());
            }
            return finished;

        }
        private void getUnChangeCells()
        {
            if (difDic[0].Count < 1)
            {
                createDictionaryArray();
            }
            int pCnt = referenceRaster.RasterInfo.BandCount;
            for (int p = 0; p < pCnt; p++)
            {
                int cCnt = cellCount[p];
                int cutOffCnt = System.Convert.ToInt32(pChange * cCnt);
                Dictionary<int,int> cDic = difDic[p];
                List<int> kSort = cDic.Keys.ToList();
                kSort.Sort();
                int kSortLeng = kSort.Count;
                int lCnt = 0;
                double minDif = 0;
                double maxDif = 0;
                for (int i = 0; i < kSortLeng; i++)
                {
                    int minVl = kSort[i];
                    lCnt += cDic[minVl];
                    if (lCnt > cutOffCnt)
                    {
                        minDif = minVl;
                        break;
                    }
                }
                lCnt = 0;
                for (int i = kSortLeng-1; i >= 0; i--)
                {
                    int maxVl = kSort[i];
                    lCnt += cDic[maxVl];
                    if (lCnt > cutOffCnt)
                    {
                        maxDif = maxVl;
                        break;
                    }
                }
                //Console.WriteLine("mindif, maxdif(" + p.ToString() + ") = " + minDif.ToString() + ", " + maxDif.ToString());
                //Console.WriteLine("maxdif(" + p.ToString() + ") = " + maxDif.ToString());
                minArray[p] = minDif;
                maxArray[p] = maxDif;
            }


        }

        private void createDictionaryArray()
        {
            IEnvelope env1 = referenceRaster.RasterInfo.Extent;
            IEnvelope env2 = transformRaster.RasterInfo.Extent;
            env1.Intersect(env2);
            clipGeo = (IGeometry)env1;
            IFunctionRasterDataset minRs = rsUtil.calcArithmaticFunction(referenceRaster, transformRaster, esriRasterArithmeticOperation.esriRasterMinus);
            clipRs = rsUtil.clipRasterFunction(minRs, clipGeo, esriRasterClippingType.esriRasterClippingOutside);
            IPnt pntSize = new PntClass();
            pntSize.SetCoords(512, 512);
            IRasterCursor rsCur = ((IRaster2)rsUtil.createRaster(clipRs)).CreateCursorEx(pntSize);
            int pCnt = rsCur.PixelBlock.Planes;
            do
            {
                IPixelBlock pbMinBlock = rsCur.PixelBlock;
                for (int r = 0; r < pbMinBlock.Height; r++)
                {
                    for (int c = 0; c < pbMinBlock.Width; c++)
                    {
                        for (int p = 0; p < pCnt; p++)
                        {
                            object vlObj = pbMinBlock.GetVal(p, c, r);
                            if (vlObj == null)
                            {
                                continue;
                            }
                            else
                            {
                                int vl = System.Convert.ToInt32(vlObj);
                                Dictionary<int, int> cDic = difDic[p];
                                int cnt = 0;
                                if (!cDic.TryGetValue(vl, out cnt))
                                {
                                    cDic.Add(vl, 1);
                                }
                                else
                                {
                                    cDic[vl] = cnt + 1;
                                }
                                cellCount[p] += 1;
                            }
                        }
                    }
                }
            } while (rsCur.Next() == true);
        }


        public void getDaModel(string modelPath)
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(modelPath))
            {
                string mType = sr.ReadLine();
                esriUtil.Statistics.dataPrepBase.modelTypes m = (esriUtil.Statistics.dataPrepBase.modelTypes)Enum.Parse(typeof(esriUtil.Statistics.dataPrepBase.modelTypes), mType);
                if (m != esriUtil.Statistics.dataPrepBase.modelTypes.Normalize)
                {
                    System.Windows.Forms.MessageBox.Show("Model file specified is not a Normalizatoin model!!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                PercentAreaChange = System.Convert.ToDouble(sr.ReadLine());
                rsType = (rstPixelType)Enum.Parse(typeof(rstPixelType),sr.ReadLine());
                int coefLength = System.Convert.ToInt32(sr.ReadLine());
                coef = new double[coefLength][];
                cellCount = new int[coefLength];
                for (int i = 0; i < coefLength; i++)
                {
                    cellCount[i] = 100;
                }
                for (int i = 0; i < coefLength; i++)
			    {
                    double[] vlArr = (from string s in (sr.ReadLine().Split(new char[]{','})) select System.Convert.ToDouble(s)).ToArray();
                    coef[i] = vlArr;
			    }
                sr.Close();
            }
        }

        public static void createReport(string mdlp)
        {
            Forms.RunningProcess.frmRunningProcessDialog rd = new Forms.RunningProcess.frmRunningProcessDialog(false);
            rd.Text = "Image Normalization Results";
            rd.TopLevel = true;
            rd.pgbProcess.Visible = false;
            rd.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            rd.addMessage("Band     |Intercept|Slope    |R-squared");
            rd.addMessage("".PadRight(39, '-'));
            using(System.IO.StreamReader sr = new System.IO.StreamReader(mdlp))
            {
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                int bnds = System.Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < bnds; i++)
                {
                    string coef = sr.ReadLine();
                    string[] coefArr = coef.Split(new char[] { ',' });
                    string ln = (i + 1).ToString();
                    ln = ln.PadRight(9,' ');
                    for (int j = 0; j < coefArr.Length; j++)
                    {
                        coefArr[j] = coefArr[j].Substring(0, 9);
                    }
                    rd.addMessage(ln + "|" + String.Join("|", coefArr));
                    
                }
                sr.Close();
            }
            rd.enableClose();
            rd.Show();
        }
    }
}
