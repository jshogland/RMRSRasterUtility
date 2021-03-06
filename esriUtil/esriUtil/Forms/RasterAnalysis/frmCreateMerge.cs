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
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace esriUtil.Forms.RasterAnalysis
{
    public partial class frmCreateMerge : Form
    {
        public frmCreateMerge(IMap map)
        {
            InitializeComponent();
            rsUtil = new rasterUtil();
            mp = map;
            if (mp != null)
            {
                vUtil = new viewUtility((IActiveView)mp);
            }
            populateComboBox();
            
        }
        public frmCreateMerge(IMap map, ref rasterUtil rasterUtility, bool AddToMap)
        {
            InitializeComponent();
            rsUtil = rasterUtility;
            addToMap = AddToMap;
            mp = map;
            if (mp != null)
            {
                vUtil = new viewUtility((IActiveView)mp);
            }
            populateComboBox();
        }
        private bool addToMap = true;
        private IMap mp = null;
        private viewUtility vUtil = null;
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = null;
        private Dictionary<string, IRaster> rstDic = new Dictionary<string, IRaster>();
        public Dictionary<string, IRaster> RasterDictionary { get { return rstDic; } }
        private void getFeaturePath()
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = true;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            flt = new ESRI.ArcGIS.Catalog.GxFilterRasterDatasetsClass();
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select Rasters";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                while (gxObj != null)
                {
                    outPath = gxObj.FullName;
                    outName = gxObj.BaseName;
                    IRaster rs = rsUtil.returnRaster(outPath);
                    
                    if (!rstDic.ContainsKey(outName))
                    {
                        rstDic.Add(outName, rs);
                        lsbRaster.Items.Add(outName);
                        //cmbInRaster1.Items.Add(nNm);
                    }
                    else
                    {
                        rstDic[outName] = rs;
                    }
                    gxObj = eGxObj.Next();  
                }
            }
            return;
        }
        private IRaster outraster = null;
        public IRaster OutRaster { get { return outraster; } }
        private string outrastername = "";
        public string OutRasterName { get { return outrastername; } }
        public void addRasterToComboBox(string rstName, IRaster rst)
        {
            if (!cmbInRaster1.Items.Contains(rstName))
            {
                cmbInRaster1.Items.Add(rstName);
                rstDic[rstName] = rst;
            }
        }
        public void removeRasterFromComboBox(string rstName)
        {
            if (cmbInRaster1.Items.Contains(rstName))
            {
                cmbInRaster1.Items.Remove(rstName);
                rstDic.Remove(rstName);
            }
        }
        private void populateComboBox()
        {
            if (mp != null)
            {
                IEnumLayer rstLyrs = vUtil.getActiveViewLayers(viewUtility.esriIRasterLayer);
                ILayer lyr = rstLyrs.Next();
                while (lyr != null)
                {
                    string lyrNm = lyr.Name;
                    IRasterLayer rstLyr = (IRasterLayer)lyr;
                    IRaster rst = rsUtil.createRaster(((IRaster2)rstLyr.Raster).RasterDataset);

                    if (!rstDic.ContainsKey(lyrNm))
                    {
                        rstDic.Add(lyrNm, rst);
                        cmbInRaster1.Items.Add(lyrNm);
                    }
                    else
                    {
                        rstDic[lyrNm] = rst;
                    }
                    lyr = rstLyrs.Next();
                }
            }
            foreach (string s in Enum.GetNames(typeof(rasterUtil.mergeType)))
            {
                cmbMergeType.Items.Add(s);
            }
            cmbMergeType.SelectedItem = rasterUtil.mergeType.FIRST.ToString();
        }

         private void btnOpenRaster_Click(object sender, EventArgs e)
        {
            getFeaturePath();
        }
        

        private void btnExecute_Click(object sender, EventArgs e)
        {
            string rstNm = txtOutNm.Text;
            string mtypStr = cmbMergeType.Text;
            if (rstNm == null || rstNm == "")
            {
                MessageBox.Show("You must specify a output name","No Output",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (lsbRaster.Items.Count < 1)
            {
                MessageBox.Show("You must select at least on Raster", "No Rasters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IRaster[] rsArr = new IRaster[lsbRaster.Items.Count];
            for (int i = 0; i < lsbRaster.Items.Count; i++)
            {
                rsArr[i]=rstDic[lsbRaster.Items[i].ToString()];
            }
            if (mtypStr == null || mtypStr == "")
            {
                MessageBox.Show("You must specify a merge type","No merge type",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            rasterUtil.mergeType mType = (rasterUtil.mergeType)Enum.Parse(typeof(rasterUtil.mergeType),mtypStr);
            this.Visible = false;
            esriUtil.Forms.RunningProcess.frmRunningProcessDialog rp = new RunningProcess.frmRunningProcessDialog(false);
            DateTime dt = DateTime.Now;
            rp.addMessage("Merging Rasters. This may take a while...");
            rp.stepPGBar(10);
            rp.TopMost = true;
            rp.Show();
            try
            {
                outraster = rsUtil.createRaster(rsUtil.calcMosaicFunction(rsArr,mType));//rsUtil.mergeRasterFunction(rsArr, mType, rstNm);//
                if (mp != null&&addToMap)
                {
                    rp.Refresh();
                    IRasterLayer rstLyr = new RasterLayerClass();
                    rstLyr.CreateFromRaster(outraster);
                    rstLyr.Visible = false;
                    rstLyr.Name = rstNm;
                    mp.AddLayer((ILayer)rstLyr);
                }
                outrastername = rstNm;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                rp.addMessage(ex.ToString());
            }
            finally
            {
                DateTime dt2 = DateTime.Now;
                TimeSpan ts = dt2.Subtract(dt);
                string t = " in " + ts.Days.ToString() + " days " + ts.Hours.ToString() + " hours " + ts.Minutes.ToString() + " minutes and " + ts.Seconds.ToString() + " seconds .";
                rp.stepPGBar(100);
                rp.addMessage("Finished Mosaic Raster" + t);
                rp.enableClose();
                this.Close();
            }

        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection s = lsbRaster.SelectedItems;
            int cnt = s.Count;
            List<string> rLst = new List<string>();
            for (int i = 0; i < cnt; i++)
            {
                string txt = s[i].ToString();
                rLst.Add(txt);
                if (txt != null && txt != "")
                {
                    if (!cmbInRaster1.Items.Contains(txt))
                    {
                        cmbInRaster1.Items.Add(txt);
                    }
                }
            }
            foreach (string r in rLst)
            {
                lsbRaster.Items.Remove(r);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lsbRaster.Items.Count; i++)
            {
                cmbInRaster1.Items.Add(lsbRaster.Items[i]);
            }
            lsbRaster.Items.Clear();

        }

        private void cmbInRaster1_SelectedIndexChanged(object sender, EventArgs e)
        {
            object itVl = cmbInRaster1.SelectedItem;
            lsbRaster.Items.Add(itVl);
            cmbInRaster1.Items.Remove(itVl);
        }

    }
}

