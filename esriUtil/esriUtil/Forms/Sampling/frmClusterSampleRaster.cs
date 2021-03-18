﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Carto;

namespace esriUtil.Forms.Sampling
{
    public partial class frmClusterSampleRaster : Form
    {
        public frmClusterSampleRaster(IMap map)
        {
            InitializeComponent();
            mp = map;
            if (mp != null)
            {
                vUtil = new viewUtility((IActiveView)mp);
            }
            populateComboBox();
        }

        private IMap mp = null;
        private viewUtility vUtil = null;
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = new rasterUtil();
        private Dictionary<string, IFeatureClass> ftrDic = new Dictionary<string, IFeatureClass>();
        private Dictionary<string, IRaster> rstDic = new Dictionary<string, IRaster>();
        private void getFeaturePath(bool featureClass)
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            if (featureClass)
            {
                flt = new ESRI.ArcGIS.Catalog.GxFilterPointFeatureClassesClass();
            }
            else
            {
                flt = new ESRI.ArcGIS.Catalog.GxFilterRasterDatasetsClass();
            }
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select a Feature";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
                outName = gxObj.BaseName;
                if (featureClass)
                {
                    if (!ftrDic.ContainsKey(outName))
                    {
                        ftrDic.Add(outName, geoUtil.getFeatureClass(outPath));
                        cmbSampleFeatureClass.Items.Add(outName);
                    }
                    else
                    {
                        ftrDic[outName] = geoUtil.getFeatureClass(outPath);
                    }
                    cmbSampleFeatureClass.Text = outName;
                }
                else
                {
                    if (!rstDic.ContainsKey(outName))
                    {
                        rstDic.Add(outName, rsUtil.returnRaster(outPath));
                        cmbRaster.Items.Add(outName);
                    }
                    else
                    {
                        rstDic[outName] = rsUtil.returnRaster(outPath);
                    }
                    cmbRaster.Text = outName;
                }

            }
            return;
        }
        private void populateComboBox()
        {
            if (mp != null)
            {
                IEnumLayer ftrLyrs = vUtil.getActiveViewLayers(viewUtility.esriIFeatureLayer);
                ILayer lyr = ftrLyrs.Next();
                while (lyr != null)
                {
                    string lyrNm = lyr.Name;
                    IFeatureLayer ftrLyr = (IFeatureLayer)lyr;
                    IFeatureClass ftrCls = ftrLyr.FeatureClass;
                    if (ftrCls.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
                    {
                        if (!ftrDic.ContainsKey(lyrNm))
                        {
                            ftrDic.Add(lyrNm, ftrCls);
                            cmbSampleFeatureClass.Items.Add(lyrNm);
                        }
                    }
                    lyr = ftrLyrs.Next();

                }
                IEnumLayer rstLyrs = vUtil.getActiveViewLayers(viewUtility.esriIRasterLayer);
                lyr = rstLyrs.Next();
                while (lyr != null)
                {
                    string lyrNm = lyr.Name;
                    IRasterLayer rstLyr = (IRasterLayer)lyr;
                    IRaster rst = rsUtil.createRaster(((IRaster2)rstLyr.Raster).RasterDataset);
                    if (!rstDic.ContainsKey(lyrNm))
                    {
                        rstDic.Add(lyrNm, rst);
                        cmbRaster.Items.Add(lyrNm);
                    }
                    lyr = rstLyrs.Next();

                }
            }
            dgvAzDs.RowCount = 1;
            string sF = "";
            foreach (string s in Enum.GetNames(typeof(rasterUtil.clusterType)))
            {
                cmbCluster.Items.Add(s);
                sF = s;
            }
            cmbCluster.SelectedItem = sF;
        }

        private void btnOpenFeatureClass_Click(object sender, EventArgs e)
        {
            getFeaturePath(true);
        }
        public void addRasterToComboBox(string rstName, IRaster rst)
        {
            if (!cmbRaster.Items.Contains(rstName))
            {
                cmbRaster.Items.Add(rstName);
                rstDic.Add(rstName, rst);
            }
        }
        private void btnOpenRaster_Click(object sender, EventArgs e)
        {
            getFeaturePath(false);
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
            string smpFtrNm = cmbSampleFeatureClass.Text;
            string rstNm = cmbRaster.Text;
            string clsTypeNm = cmbCluster.Text;
            if (smpFtrNm == null || smpFtrNm == "" || rstNm == "" || rstNm == null || clsTypeNm==null||clsTypeNm=="")
            {
                MessageBox.Show("sample location, raster, or cluster type are not specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Dictionary<double, double> azDsDic = new Dictionary<double, double>();
            for (int i = 0; i < dgvAzDs.RowCount; i++)
            {
                double az = System.Convert.ToDouble(dgvAzDs[0,i].Value);
                double ds = System.Convert.ToDouble(dgvAzDs[1,i].Value);
                if (!azDsDic.ContainsKey(az))
                {
                    azDsDic.Add(az, ds);
                }
            }
            if (azDsDic.Count < 1)
            {
                MessageBox.Show("Nothing specified for offset!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            rasterUtil.clusterType clsType = (rasterUtil.clusterType)Enum.Parse(typeof(rasterUtil.clusterType), clsTypeNm);
            this.Visible = false;
            esriUtil.Forms.RunningProcess.frmRunningProcessDialog rp = new RunningProcess.frmRunningProcessDialog(false);
            rp.addMessage("Sampling raster");
            rp.addMessage("This may take a while...");
            rp.stepPGBar(20);
            rp.Show();
            rp.Refresh();
            DateTime dt1 = DateTime.Now;
            try
            {

                rsUtil.sampleRaster(ftrDic[smpFtrNm], rstDic[rstNm], rstNm, azDsDic,clsType);
                DateTime dt2 = DateTime.Now;
                TimeSpan ts = dt2.Subtract(dt1);
                string prcTime = "Time to complete process:\n" + ts.Days.ToString() + " Days " + ts.Hours.ToString() + " Hours " + ts.Minutes.ToString() + " Minutes " + ts.Seconds.ToString() + " Seconds ";
                rp.addMessage(prcTime);

            }
            catch (Exception ex)
            {
                rp.addMessage(ex.ToString());
            }
            finally
            {
                rp.stepPGBar(100);
                rp.enableClose();
                this.Close();
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            dgvAzDs.RowCount = dgvAzDs.RowCount + 1;
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            dgvAzDs.RowCount = dgvAzDs.RowCount - 1;
        }
    }
}