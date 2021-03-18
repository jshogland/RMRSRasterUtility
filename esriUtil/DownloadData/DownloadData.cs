using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Net;


namespace DownloadData
{
    public partial class frmDownLoad : Form
    {
        public frmDownLoad()
        {
            InitializeComponent();
        }

        private string getPolyPath()
        {
            string outPath = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            flt = new ESRI.ArcGIS.Catalog.GxFilterPolygonFeatureClassesClass();
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select a Feature";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
            }
            return outPath;
        }

        private void btnIndex_Click(object sender, EventArgs e)
        {
            txtIndex.Text = getPolyPath();
        }

        private void btnPolyExtent_Click(object sender, EventArgs e)
        {
            txtExtent.Text = getPolyPath();
        }

        private void btnWorkspace_Click(object sender, EventArgs e)
        {
            txtWorkspace.Text = getDirectoryPath();
        }

        private string getDirectoryPath()
        {
            string outPath = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            flt = new ESRI.ArcGIS.Catalog.GxFilterFileFolderClass();//.GxFilterPolygonFeatureClassesClass();
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select a Feature";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
            }
            return outPath;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string ftpSite = txtDownloadSite.Text;  //@"ftp://rockyftp.cr.usgs.gov/vdelivery/Datasets/Staged/NAIP/mt_2013/";
            string outDir = txtWorkspace.Text;//@"E:\Helena\NAIP\Bulk Order 421750\NAIP JPG2000";
            string indexFtrPath = txtIndex.Text;
            string extentFtrPath = txtExtent.Text;
            string ext = txtFileExtension.Text;
            string fldName = cmbFld.Text;
            if (ftpSite == ""||ftpSite==null)
            {
                System.Windows.Forms.MessageBox.Show("You must specify a FTP site before you can download data");
                return;
            }
            if (outDir == "" || outDir == null)
            {
                System.Windows.Forms.MessageBox.Show("You must specify an output directory before you can download data");
                return;
            }
            if (indexFtrPath == "" || indexFtrPath == null)
            {
                System.Windows.Forms.MessageBox.Show("You must specify an index feature class before you can download data");
                return;
            }
            if (extentFtrPath == "" || extentFtrPath == null)
            {
                System.Windows.Forms.MessageBox.Show("You must specify an extent feature class before you can download data");
                return;
            }
            if (ext == "" || ext == null)
            {
                System.Windows.Forms.MessageBox.Show("You must specify a extension (.zip) for the given files before you can download data");
                return;
            }
            if (fldName == "" || fldName == null)
            {
                System.Windows.Forms.MessageBox.Show("You must specify a field name before you can download data");
                return;
            }
            //this.Visible = false;
            esriUtil.Forms.RunningProcess.frmRunningProcessDialog rpd = new esriUtil.Forms.RunningProcess.frmRunningProcessDialog(false);
            rpd.TopMost = false;
            rpd.Show();
            try
            {
                rpd.addMessage("Finding all Files on the server");
                List<string> fNames = getFileNames(ftpSite);

                rpd.addMessage("Found " + fNames.Count.ToString() + " on the server.\nFinding all existing files stored locally");
                List<string> exNames = getExistingNames(outDir,ext);
                rpd.addMessage("Found " + exNames.Count.ToString() + " within local directory.\nFinding files to download");
                List<string> tiles = getTiles(indexFtrPath, extentFtrPath,fldName,ext);
                rpd.addMessage("Found " + tiles.Count.ToString() + " files to download!\nAttempting to download files!");
                int dc = Math.Abs(tiles.Count - exNames.Count);
                int stpCnt = System.Convert.ToInt32(dc / 100);
                if (stpCnt == 0) stpCnt = 1;
                for (int i = 0; i < fNames.Count; i++)
                {
                    string fName = fNames[i];
                    //rpd.addMessage("Looking at file name " + fName);
                    if (tiles.Contains(fName))
                    {
                        if (!exNames.Contains(fName))
                        {
                            rpd.addMessage("FileName = " + fName +"\n\tAttempting Download...");

                            bool gotFile = Download(ftpSite, outDir, fName);
                            rpd.addMessage("\tSuccessful download = " + gotFile.ToString());
                            if (i % stpCnt == 0) rpd.stepPGBar(1);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                rpd.addMessage(exp.ToString());
                MessageBox.Show(exp.ToString());
            }
            finally
            {
                rpd.addMessage("Finished Downloading Data");
                rpd.Refresh();
                rpd.stepPGBar(100);
                rpd.enableClose();
            }
        }

        private bool Download(string ftpPath, string filePath, string fileName)
        {
            bool ch = true;
            FtpWebRequest reqFTP;
            try
            {

                System.IO.FileStream outputStream = new System.IO.FileStream(filePath + "\\" + fileName, System.IO.FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential("anonymous", "anonymous");
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                System.IO.Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ch = false;
            }
            return ch;
        }
        private esriUtil.geoDatabaseUtility geoUtil = new esriUtil.geoDatabaseUtility();
        private esriUtil.rasterUtil rsUtil = new esriUtil.rasterUtil();
        private List<string> getTiles(string indexPath, string extentPath, string fldName = "Name", string ext = ".jp2")
        {
            List<string> outLSt = new List<string>();
            IFeatureClass ftrClsExt = geoUtil.getFeatureClass(extentPath);
            IFeatureClass ftrClsInd = geoUtil.getFeatureClass(indexPath);
            IEnvelope env = ((IGeoDataset)ftrClsExt).Extent;
            ISpatialFilter spF = new SpatialFilterClass();
            spF.Geometry = env;
            spF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor ftrCur = ftrClsInd.Search(spF, true);
            IFeature ftr = ftrCur.NextFeature();
            int fnIndex = ftrCur.FindField(fldName);
            while (ftr != null)
            {
                object vlObj = ftr.get_Value(fnIndex);
                if (vlObj != null)
                {
                    string vl = (vlObj.ToString()+ext).ToLower();
                    outLSt.Add(vl);
                }
                ftr = ftrCur.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ftrCur);
            return outLSt;
        }

        private List<string> getExistingNames(string outDir,string ext=".jp2")
        {
            List<string> outLst = new List<string>();
            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(outDir);
            System.IO.FileInfo[] fInfoArr = d.GetFiles("*" + ext);
            for (int i = 0; i < fInfoArr.Length; i++)
            {
                System.IO.FileInfo f = fInfoArr[i];
                outLst.Add(f.Name.ToLower());
            }
            return outLst;

        }

        private List<string> getFileNames(string ftpPath)
        {
            List<string> outLst = new List<string>();

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous", "anonymous");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            System.IO.Stream responseStream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(responseStream);
            string ln;
            while ((ln = sr.ReadLine()) != null)
            {
                string[] lnArr = ln.Split(new char[] { ' ' });
                outLst.Add(lnArr[lnArr.Length - 1].Trim().ToLower());
            }
            sr.Close();
            response.Close();
            return outLst;
        }


        private void txtIndex_TextChanged(object sender, EventArgs e)
        {
            cmbFld.Items.Clear();
            try
            {
                IFeatureClass ftrCls = geoUtil.getFeatureClass(txtIndex.Text);
                IFields flds = ftrCls.Fields;
                for (int i = 0; i < flds.FieldCount; i++)
                {
                    cmbFld.Items.Add(flds.get_Field(i).Name);
                }
                cmbFld.SelectedItem = cmbFld.Items[0];
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        
    }
}
