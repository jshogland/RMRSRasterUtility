using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesRaster;

namespace esriUtil
{
    public class bootStrapping
    {
        public bootStrapping(ITable inTable, string valueFieldName, int iterations, string outputPath, string strataFieldName = "", double alpha = 0.05, esriUtil.Forms.RunningProcess.frmRunningProcessDialog runningProcess = null)
        {
            tbl = inTable;
            vlfld = valueFieldName;
            IField tFld = tbl.Fields.Field[tbl.FindField(vlfld)];
            if(tFld.Type== esriFieldType.esriFieldTypeString)
            {
                cont = false;
            }
            stFld = strataFieldName;
            outPath = outputPath;
            iter = iterations;
            alpha1 = ((1 - alpha)*100).ToString();
            alphaL = alpha / 2;
            alphaU = 1 - alphaL;
            rp = runningProcess;
        }
        ITable tbl = null;
        private string vlfld = "";
        private string stFld = "";
        private string outPath = "";
        private int iter = 100;
        private bool cont = true;
        private string alpha1 = "95";
        private double alphaL = 0.025;
        private double alphaU = 0.975;
        private esriUtil.Forms.RunningProcess.frmRunningProcessDialog rp = null;

        public void runBootStrap()
        {
            if(cont)
            {
                runBootStrapContinuous();
            }
            else
            {
                runBootStrapCategorical();
            }
        }
        private void runBootStrapCategorical()
        {
            Random rand = new Random();
            Console.WriteLine("Reading Data...");
            if (rp != null) rp.addMessage("Reading Data...");
            Dictionary<string, List<string>> vlDic = new Dictionary<string, List<string>>();
            Dictionary<string, HashSet<string>> stDic = new Dictionary<string, HashSet<string>>();
            Dictionary<string, double[][]> mdic = new Dictionary<string, double[][]>();
            IQueryFilter qf = new QueryFilterClass();
            string fldLst = vlfld;
            if (stFld != "")
            {
                fldLst = stFld + "," + vlfld;
            }
            qf.SubFields = fldLst;
            qf.WhereClause = "not " + vlfld + " is Null";
            ICursor cur = tbl.Search(qf, true);
            int vlIndex = cur.FindField(vlfld);
            int stIndex = cur.FindField(stFld);
            IRow rw = cur.NextRow();
            while (rw != null)
            {
                string st = "";
                string vl = rw.Value[vlIndex].ToString();
                if (stIndex > -1)
                {
                    st = rw.Value[stIndex].ToString();
                }
                List<string> tLst;
                if (vlDic.TryGetValue(st, out tLst))
                {
                    tLst.Add(vl);
                    vlDic[st] = tLst;
                    HashSet<string> tHas = stDic[st];
                    tHas.Add(vl);
                    stDic[st] = tHas;

                }
                else
                {
                    tLst = new List<string>();
                    tLst.Add(vl);
                    vlDic.Add(st, tLst);
                    HashSet<string> tHas = new HashSet<string>();
                    tHas.Add(vl);
                    stDic.Add(st, tHas);
                }
                rw = cur.NextRow();
            }
            foreach (KeyValuePair<string, HashSet<string>> kvp in stDic)
            {
                int tCnt = kvp.Value.Count;
                double[][] tdArr = new double[tCnt][];
                for (int i = 0; i < tCnt; i++)
                {
                    tdArr[i] = new double[iter];
                }
                mdic.Add(kvp.Key, tdArr);
            }
            Dictionary<string, string[]> unqDic = new Dictionary<string, string[]>();
            foreach (KeyValuePair<string,HashSet<string>> kvp in stDic)
            {
                unqDic.Add(kvp.Key, kvp.Value.ToArray());
            }
            Console.WriteLine("\tCalulating Proportions for " + iter + " iterations");
            if (rp != null) rp.addMessage("Calulating Proportions for " + iter + " iterations");
            for (int i = 0; i < iter; i++)
            {
                //Console.WriteLine("\tIteration " + i.ToString());
                //if (rp != null) rp.addMessage("\tIteration " + i.ToString());
                foreach (string ky in vlDic.Keys)
                {
                    List<string> tlst = vlDic[ky];
                    double[][] tdArr = mdic[ky];
                    int n = tlst.Count() - 1;
                    //Console.WriteLine(n.ToString());
                    Dictionary<string, int> tDic = new Dictionary<string, int>();
                    for (int k = 0; k <= n; k++)
                    {
                        int lsIndex = rand.Next(n);
                        string vl = tlst[lsIndex];
                        int tcnt;
                        if(tDic.TryGetValue(vl,out tcnt))
                        {
                            tDic[vl] = tcnt + 1;
                        }
                        else
                        {
                            tDic.Add(vl, 1);
                        }
                    }
                    foreach (KeyValuePair<string,int> kvp in tDic)
                    {
                        int aIndex = System.Array.IndexOf(unqDic[ky], kvp.Key);
                        (mdic[ky])[aIndex][i] = System.Convert.ToDouble(kvp.Value) / (n + 1);
                    }
                }
            }
            writeCsvCa(mdic, vlDic, unqDic);
        }

        private void writeCsvCa(Dictionary<string, double[][]> mdic, Dictionary<string, List<string>> vlDic, Dictionary<string, string[]> unqDic)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outPath))
            {
                string ln = "stratum,iterations,class,proportion,se,low" + alpha1 + ",upper" +alpha1 + ",min,max,n";
                sw.WriteLine(ln);
                if (rp != null)
                    rp.addMessage(ln);
                foreach (KeyValuePair<string, double[][]> kyp in mdic)
                {
                    double[][] vlArr = kyp.Value;
                    string[] stArr = unqDic[kyp.Key];
                    for (int i = 0; i < stArr.Length; i++)
                    {
                        double[] vlArr2 = vlArr[i];
                        string cls = stArr[i];
                        System.Array.Sort(vlArr2);
                        int lIndex = System.Convert.ToInt32(alphaL * vlArr2.Length) - 1;
                        if (lIndex < 0) lIndex = 0;
                        int uIndex = System.Convert.ToInt32(alphaU * vlArr2.Length) - 1;
                        string[] lnArr = new string[10];
                        lnArr[0] = kyp.Key;
                        double min = vlArr2[0];
                        double max = vlArr2[vlArr2.Length - 1];
                        double s = 0;
                        double ss = 0;
                        double n = vlArr2.Length;
                        lnArr[1] = n.ToString();
                        foreach (double d in vlArr2)
                        {
                            s = s + d;
                            ss = ss + d * d;
                        }
                        double m1 = s / n;
                        lnArr[2] = cls;
                        lnArr[3] = m1.ToString();
                        lnArr[4] = Math.Sqrt((ss - (Math.Pow(s, 2) / n)) / (n - 1)).ToString();
                        lnArr[5] = vlArr2[lIndex].ToString();
                        lnArr[6] = vlArr2[uIndex].ToString();
                        lnArr[7] = min.ToString();
                        lnArr[8] = max.ToString();
                        lnArr[9] = vlDic[kyp.Key].Count.ToString();
                        ln = String.Join(",", lnArr);
                        sw.WriteLine(ln);
                        if (rp != null) rp.addMessage(ln);
                    }
                }
                sw.Close();
            }
        }

        private void runBootStrapContinuous()
        {
            Random rand = new Random();
            Console.WriteLine("Reading Data...");
            if (rp != null) rp.addMessage("Reading Data");
            Dictionary<string,List<double>> vlDic = new Dictionary<string, List<double>>();
            Dictionary<string, double[]> mdic = new Dictionary<string, double[]>();
            IQueryFilter qf = new QueryFilterClass();
            string fldLst = vlfld;
            if(stFld!="")
            {
                fldLst = stFld + "," + vlfld;
            }
            qf.SubFields = fldLst;
            qf.WhereClause = "not " + vlfld + " is Null";
            ICursor cur = tbl.Search(qf, true);
            int vlIndex = cur.FindField(vlfld);
            int stIndex = cur.FindField(stFld);
            IRow rw = cur.NextRow();
            while (rw!=null)
            {
                string st = "";
                double vl = System.Convert.ToDouble(rw.Value[vlIndex]);
                if(stIndex>-1)
                {
                    st = rw.Value[stIndex].ToString();
                }
                List<double> tLst;
                if(vlDic.TryGetValue(st,out tLst))
                {
                    tLst.Add(vl);
                    vlDic[st] = tLst;
                }
                else
                {
                    tLst = new List<double>();
                    tLst.Add(vl);
                    vlDic.Add(st, tLst);
                    mdic.Add(st, new double[iter]);
                }
                rw = cur.NextRow();
            }
            Console.WriteLine("\tCalulating Means for " + iter + " iterations");
            if (rp != null) rp.addMessage("Calulating Means for " + iter + " iterations");
            for (int i = 0; i < iter; i++)
            {
                //Console.WriteLine("\tIteration " + i.ToString());
                //if (rp != null) rp.addMessage("\tIteration " + i.ToString());
                foreach (string ky in vlDic.Keys)
                {
                    List<double> tDic = vlDic[ky]; 
                    int n = tDic.Count()-1;
                    //Console.WriteLine(n.ToString());
                    double svl = 0;
                    for (int k = 0; k <= n; k++)
                    {
                        int lsIndex = rand.Next(n);
                        double vl = tDic[lsIndex];
                        svl = svl + vl;
                    }
                    double meanVl = svl / (n + 1);
                    mdic[ky][i] = meanVl;
                }
            }
            writeCsvC(mdic,vlDic);
        }

        private void writeCsvC(Dictionary<string, double[]> mdic,Dictionary<string,List<double>> vlDic)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outPath))
            {
                string ln = "stratum,iterations,mean,se,low" + alpha1 + ",upper" + alpha1 + ",min,max,n";
                sw.WriteLine(ln);
                if (rp != null) rp.addMessage(ln);
                foreach (KeyValuePair<string,double[]> kyp in mdic)
                {
                    double[] vlArr = kyp.Value;
                    System.Array.Sort(vlArr);
                    int lIndex = System.Convert.ToInt32(alphaL * vlArr.Length)-1;
                    if (lIndex < 0) lIndex = 0;
                    int uIndex = System.Convert.ToInt32(alphaU * vlArr.Length)-1;
                    string[] lnArr = new string[9];
                    lnArr[0] = kyp.Key;
                    double min = vlArr[0];
                    double max = vlArr[vlArr.Length-1];
                    double s = 0;
                    double ss = 0;
                    double n = vlArr.Length;
                    lnArr[1] = n.ToString();
                    foreach (double d in vlArr)
                    {
                        s = s + d;
                        ss = ss + d * d;
                    }
                    double m1 = s / n;
                    lnArr[2] = m1.ToString();
                    lnArr[3] = Math.Sqrt((ss - (Math.Pow(s, 2)/n)) / (n - 1)).ToString();
                    lnArr[4] = vlArr[lIndex].ToString();
                    lnArr[5] = vlArr[uIndex].ToString();
                    lnArr[6] = min.ToString();
                    lnArr[7] = max.ToString();
                    lnArr[8] = vlDic[kyp.Key].Count.ToString();
                    ln = String.Join(",", lnArr);
                    sw.WriteLine(ln);
                    if (rp != null) rp.addMessage(ln);
                }
                sw.Close();
            }
        }
    }
}
