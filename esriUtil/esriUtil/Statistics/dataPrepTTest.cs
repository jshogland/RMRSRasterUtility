﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using Accord.MachineLearning;

namespace esriUtil.Statistics
{
    public class dataPrepTTest
    {
        public dataPrepTTest()
        {
        }
        public dataPrepTTest(IRaster StrataRaster, IRaster VariableRaster)
        {
            InValueRaster = VariableRaster;
            if (StrataRaster == null)
            {
                InStrataRaster = rsUtil.createRaster(rsUtil.constantRasterFunction(rsUtil.getBand(VariableRaster, 0), 1));
            }
            else
            {
                InStrataRaster = StrataRaster;
            }
            VariableFieldNames = new string[((IRasterBandCollection)InValueRaster).Count];
            for (int i = 0; i < VariableFieldNames.Length; i++)
            {
                VariableFieldNames[i] = (i + 1).ToString();
            }
            buildModel();
        }

        public dataPrepTTest(ITable table, string[] variables, string strataField)
        {
            InTable = table;
            VariableFieldNames = variables;
            if (strataField == "" || strataField == null)
            {
                if (InTable.FindField("gr") > -1) StrataField = "gr";
                else StrataField = addConstantGroupField();
            }
            else
            {
                StrataField = strataField;
            }
            buildModel();
        }

        public string addConstantGroupField()
        {
            string outNm = "gr";
            IObjectClassInfo2 ocI2 = (IObjectClassInfo2)InTable;
            if (!ocI2.CanBypassEditSession())
            {
                System.Windows.Forms.MessageBox.Show("Input table has a composition relationship. Please export data and try again!");
            }
            else
            {
                outNm = geoUtil.createField(InTable, outNm, esriFieldType.esriFieldTypeString,false);
                IQueryFilter qf = new QueryFilterClass();
                qf.SubFields = outNm;
                ICursor cur = InTable.Update(qf, false);
                int flIndex = cur.FindField(outNm);
                IRow rw = cur.NextRow();
                while (rw != null)
                {
                    rw.set_Value(flIndex, "1");
                    cur.UpdateRow(rw);
                    rw = cur.NextRow();
                }
            }
            return outNm;
        }
        public void buildModel()
        {
            dataPrepStrata dStrat;
            if(InValueRaster==null)
            {
                 dStrat = new dataPrepStrata(InTable, VariableFieldNames, StrataField);
            }
            else
            {
                dStrat = new dataPrepStrata(InValueRaster,InStrataRaster);
            }
            inpath = dStrat.InPath;
            n = dStrat.N;
            kmeans = dStrat.Model;
            lbl = dStrat.Labels;
            k = lbl.Count;
            prop = 1;

        }

        
        private double prop = 1;
        private int k = 2;
        public void buildModel(string modelPath)
        {
            outmodelpath = modelPath;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(outmodelpath))
            {
                dataPrepBase.modelTypes mType = (dataPrepBase.modelTypes)Enum.Parse(typeof(dataPrepBase.modelTypes), sr.ReadLine());
                if (mType != dataPrepBase.modelTypes.TTEST)
                {

                    System.Windows.Forms.MessageBox.Show("Not a TTest Model!!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                inpath = sr.ReadLine();
                stratafld = sr.ReadLine();
                VariableFieldNames = sr.ReadLine().Split(new char[] { ',' });
                n = System.Convert.ToInt32(sr.ReadLine());
                prop = System.Convert.ToDouble(sr.ReadLine());
                k = System.Convert.ToInt32(sr.ReadLine());
                lbl = sr.ReadLine().Split(new char[] { ',' }).ToList();
                kmeans = new KMeans(k);
                KMeansClusterCollection kmeansColl = kmeans.Clusters;
                for (int i = 0; i < k; i++)
                {
                    double[] mns = (from s in (sr.ReadLine().Split(new char[] { ',' })) select System.Convert.ToDouble(s)).ToArray();
                    string[] covVlsStr = sr.ReadLine().Split(new char[] { ',' });
                    double p = System.Convert.ToDouble(sr.ReadLine());
                    double[,] cov = new double[VariableFieldNames.Length, VariableFieldNames.Length];
                    for (int j = 0; j < VariableFieldNames.Length; j++)
                    {
                        for (int l = 0; l < VariableFieldNames.Length; l++)
                        {
                            int indexVl = (j * VariableFieldNames.Length) + l;
                            cov[l, j] = System.Convert.ToDouble(covVlsStr[indexVl]);
                        }
                    }

                    KMeansCluster kc = new KMeansCluster(kmeansColl, i);
                    kc.Mean = mns;
                    kc.Covariance = cov;
                    kc.Proportion = p;
                }
                sr.Close();
            }
        }
        private int n = 0;
        public int N { get { return n; } }
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = new rasterUtil();
        private string inpath = "";
        public string InPath
        {
            get
            {
                return inpath;

            }

        }
        private IRaster instratarst = null;
        public IRaster InStrataRaster
        {
            get
            {
                return instratarst;
            }
            set
            {
                instratarst = value;
                IDataset dSet = (IDataset)((IRaster2)instratarst).RasterDataset;
                inpath = dSet.Workspace.PathName + "\\" + dSet.BrowseName;
            }
        }
        private IRaster invlrst = null;
        public IRaster InValueRaster
        {
            get
            {
                return invlrst;
            }
            set
            {
                invlrst = value;
            }
        }
        private ITable intable = null;
        public ITable InTable
        {
            get
            {
                return intable;
            }
            set
            {
                intable = value;
                IDataset dSet = (IDataset)intable;
                inpath = dSet.Workspace.PathName + "\\" + dSet.BrowseName;
            }
        }
        public string[] VariableFieldNames { get; set; }
        private string outmodelpath = null;
        private string stratafld = "";
        public string StrataField { get { return stratafld; } set { stratafld = value; } }
        private KMeans kmeans = null;
        public KMeans Model { get { return kmeans; } }
        private List<string> lbl = new List<string>();
        public List<string> Labels
        {
            get
            {
                return lbl;
            }
        }
        public void writeModel(string outPath)
        {
            if (kmeans == null) buildModel();
            outmodelpath = outPath;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outmodelpath))
            {
                sw.WriteLine(dataPrepBase.modelTypes.TTEST.ToString());
                sw.WriteLine(InPath);
                sw.WriteLine(StrataField);
                sw.WriteLine(String.Join(",", VariableFieldNames));
                sw.WriteLine(n.ToString());
                sw.WriteLine(prop.ToString());
                sw.WriteLine(k.ToString());
                sw.WriteLine(String.Join(",", lbl.ToArray()));
                KMeansClusterCollection gCol = kmeans.Clusters;
                for (int i = 0; i < gCol.Count; i++)
                {
                    KMeansCluster gClust = gCol[i];
                    sw.WriteLine(String.Join(",", (from double d in gClust.Mean select d.ToString()).ToArray()));
                    sw.WriteLine(String.Join(",", (from double d in gClust.Covariance select d.ToString()).ToArray()));
                    sw.WriteLine(gClust.Proportion.ToString());
                }
                sw.Close();
            }
        }
        public double[] computeNew(object category)
        {
            string cat = category.ToString();
            int catIndex = lbl.IndexOf(cat);
            int np = ((VariableFieldNames.Length * VariableFieldNames.Length) - VariableFieldNames.Length) / 2;
            double[] pValues = new double[np];
            if (catIndex == -1) return pValues;
            KMeansClusterCollection gCol = kmeans.Clusters;
            KMeansCluster gClust = gCol[catIndex];
            double[] mns = gClust.Mean;
            double[] var = new double[mns.Length];
            double nSample = gClust.Proportion * N;
            double seAdjust = Math.Sqrt(2 * 1 / nSample);
            for (int j = 0; j < mns.Length; j++)
            {
                var[j] = gClust.Covariance[j, j];
            }
            int cnt = 1;
            int pCnt = 0;
            for (int j = 0; j < mns.Length-1; j++)
            {
                for (int k = cnt; k < mns.Length; k++)
                {
                    double mD = mns[j] - mns[k];
                    double pSD = Math.Sqrt((var[j] + var[k]) / 2);
                    double se = pSD * seAdjust;
                    double tStat = mD / se;
                    Accord.Statistics.Distributions.Univariate.TDistribution tDist = new Accord.Statistics.Distributions.Univariate.TDistribution(2*nSample-2);
                    double cdf = tDist.DistributionFunction(tStat);
                    double pValue = 0;
                    if (tStat>0)
                    {
                        pValue = (1 - cdf) * 2;
                    }
                    else
                    {
                        pValue = (cdf * 2);
                    }
                    pValues[pCnt] = pValue;
                    pCnt++;
                }
                cnt += 1;
            }
            return pValues;
        }
        private Forms.RunningProcess.frmRunningProcessDialog rd = null;
        public Forms.RunningProcess.frmRunningProcessDialog ReportForm { get { return rd; } }
        public void getReport()
        {
            if (kmeans == null) buildModel();
            rd = new Forms.RunningProcess.frmRunningProcessDialog(false);
            rd.Text = "Independent T-Test Results";
            rd.TopLevel = true;
            rd.pgbProcess.Visible = false;
            rd.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            rd.addMessage("Input path = " + InPath);
            rd.addMessage("Total Sample size = " + n.ToString());
            rd.addMessage("\nLabel   |Compare V1-V2            |N       |Dif     |T-Stat  |P-Value ");
            rd.addMessage("-".PadRight(83,'-'));
            KMeansClusterCollection gCol = kmeans.Clusters;
            for (int i = 0; i < gCol.Count; i++)
            {
                string l = getValue(Labels[i],8);
                KMeansCluster gClust = gCol[i];
                double[] mns = gClust.Mean;
                double[] var = new double[mns.Length];
                double nSample = gClust.Proportion * N;
                if (nSample <= 1) continue;
                double seAdjust = Math.Sqrt(2*1/nSample);
                for (int j = 0; j < mns.Length; j++)
                {
                    var[j] = gClust.Covariance[j, j];
                }
                int cnt = 1;
                for (int j = 0; j < mns.Length-1; j++)
                {
                    for (int k = cnt; k < mns.Length; k++)
                    {
                        string fN1 = getValue(VariableFieldNames[j],12);
                        string fN2 = getValue(VariableFieldNames[k],12);
                        double mD = mns[j] - mns[k];
                        double pSD = Math.Sqrt((var[j] + var[k]) / 2);
                        double se = pSD * seAdjust;
                        double tStat = mD / se;
                        Accord.Statistics.Distributions.Univariate.TDistribution tDist = new Accord.Statistics.Distributions.Univariate.TDistribution(2*nSample-2);
                        double cdf = tDist.DistributionFunction(tStat);
                        double pValue = 0;
                        if (tStat>0)
                        {
                            pValue = (1 - cdf) * 2;
                        }
                        else
                        {
                            pValue = (cdf * 2);
                        }
                        string pValueS = pValue.ToString();
                        if (pValue < 0.0001) pValueS = "p < 0.0001";
                        string ln = l + "|" + fN1 + "-" + fN2 + "| " + getValue(nSample.ToString(), 6) + " | " + getValue(mD.ToString(), 6) + " | " + getValue(tStat.ToString(), 6) + " | " + getValue(pValueS,10);
                        rd.addMessage(ln);
                    }
                    cnt += 1;
                }
            }
            rd.addMessage("-".PadRight(83, '-'));
            rd.enableClose();
            rd.Show();
        }

        public string getValue(string vl, int leng)
        {
            string outVl = vl;
            if (vl.Length > leng) outVl = vl.Substring(0, leng);
            else outVl = vl.PadRight(leng, ' ');
            return outVl;
        }
    }
}
