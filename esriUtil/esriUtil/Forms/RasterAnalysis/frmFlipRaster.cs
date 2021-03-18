﻿using System;
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
    public partial class frmFlipRaster : Form
    {
        public frmFlipRaster(IMap map,rasterUtil.surfaceType surfaceType = rasterUtil.surfaceType.FLIP)
        {
            InitializeComponent();
            rsUtil = new rasterUtil();
            sType = surfaceType;
            this.Text = sType.ToString() + " Raster";
            mp = map;
            if (mp != null)
            {
                vUtil = new viewUtility((IActiveView)mp);
            }
            populateComboBox();
        }
        public frmFlipRaster(IMap map, ref rasterUtil rasterUtility, bool AddToMap, rasterUtil.surfaceType surfaceType = rasterUtil.surfaceType.FLIP)
        {
            InitializeComponent();
            rsUtil = rasterUtility;
            sType = surfaceType;
            this.Text = sType.ToString() + " Raster";
            addToMap = AddToMap;
            mp = map;
            if (mp != null)
            {
                vUtil = new viewUtility((IActiveView)mp);
            }
            populateComboBox();
        }
        private rasterUtil.surfaceType sType = rasterUtil.surfaceType.FLIP;
        private bool addToMap = true;
        public bool AddToMap { get { return addToMap; } }
        private IMap mp = null;
        public IMap Map { get { return mp; } }
        private viewUtility vUtil = null;
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = null;
        public rasterUtil RasterUtil { get { return rsUtil; } }
        private Dictionary<string, IRaster> rstDic = new Dictionary<string, IRaster>();
        public Dictionary<string, IRaster> RasterDictionary { get { return rstDic; } }
        private void getFeaturePath()
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            flt = new ESRI.ArcGIS.Catalog.GxFilterRasterDatasetsClass();
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select a Raster";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
                outName = gxObj.BaseName;
                if (!rstDic.ContainsKey(outName))
                {
                    rstDic.Add(outName, rsUtil.returnRaster(outPath));
                    cmbInRaster1.Items.Add(outName);
                }
                else
                {
                    rstDic[outName] = rsUtil.returnRaster(outPath);
                }
                cmbInRaster1.Text = outName;


            }
            return;
        }
        private IRaster outraster = null;
        public IRaster OutRaster { get { return outraster; } set { outraster = value; } }
        private string outrastername = "";
        public string OutRasterName { get { return outrastername; } set { outrastername = value; } }
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
                    lyr = rstLyrs.Next();

                }
            }
        }
        private void btnOpenRaster_Click(object sender, EventArgs e)
        {
            getFeaturePath();
        }

        public void btnClip_Click(object sender, EventArgs e)
        {
            string rstNm = cmbInRaster1.Text;
            string outNm = txtOutName.Text;
            if ( rstNm == "" || rstNm == null)
            {
                MessageBox.Show("You must have a raster layer selected and a valid value", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (outNm == "" || outNm == null)
            {
                MessageBox.Show("You must specify an output raster name", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Visible = false;
            esriUtil.Forms.RunningProcess.frmRunningProcessDialog rp = new RunningProcess.frmRunningProcessDialog(false);
            DateTime dt = DateTime.Now;
            rp.addMessage("Transforming Raster. This may take a while...");
            rp.stepPGBar(10);
            rp.TopMost = true;
            try
            {
                IRaster rst = rstDic[rstNm];
                switch (sType)
                {
                    case rasterUtil.surfaceType.SLOPE:
                        outraster = rsUtil.returnRaster(rsUtil.calcSlopeFunction(rst));
                        break;
                    case rasterUtil.surfaceType.ASPECT:
                        outraster = rsUtil.createRaster(rsUtil.calcAspectFunction(rst));
                        break;
                    case rasterUtil.surfaceType.EASTING:
                        outraster = rsUtil.createRaster(rsUtil.calcEastWestFunction(rst));
                        break;
                    case rasterUtil.surfaceType.NORTHING:
                        outraster = rsUtil.createRaster(rsUtil.calcNorthSouthFunction(rst));
                        break;
                    default:
                        outraster = rsUtil.createRaster(rsUtil.flipRasterFunction(rst));
                        break;
                }
                
                if (mp != null && addToMap)
                {
                    rp.addMessage("Calculating Statistics...");
                    rp.Show();
                    rp.Refresh();
                    IRasterLayer rstLyr = new RasterLayerClass();
                    //rsUtil.calcStatsAndHist(((IRaster2)outraster).RasterDataset);
                    rstLyr.CreateFromRaster(outraster);
                    rstLyr.Name = outNm;
                    rstLyr.Visible = false;
                    mp.AddLayer(rstLyr);
                }
                outrastername = outNm;
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
                rp.addMessage("Finished Setting Values Raster" + t);
                rp.enableClose();
                this.Close();
            }

        }
    }
}
