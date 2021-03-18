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
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;

namespace esriUtil.Forms.Stats
{
    public partial class frmTTest : Form
    {
        public frmTTest(IMap map)
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
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = new rasterUtil();
        private viewUtility vUtil = null;
        private Dictionary<string, ITable> ftrDic = new Dictionary<string, ITable>();
        private Dictionary<string, IRaster> rstDic = new Dictionary<string, IRaster>();
        private IFields flds = null;
        private void populateComboBox()
        {
            if (mp != null)
            {
                IEnumLayer rstLyrs = vUtil.getActiveViewLayers(viewUtility.esriIFeatureLayer);
                ILayer lyr = rstLyrs.Next();
                while (lyr != null)
                {
                    string lyrNm = lyr.Name;
                    IFeatureLayer ftrLyr = (IFeatureLayer)lyr;
                    IFeatureClass ftrCls = ftrLyr.FeatureClass;
                    if (!ftrDic.ContainsKey(lyrNm))
                    {
                        ftrDic.Add(lyrNm, (ITable)ftrCls);
                        cmbSampleFeatureClass.Items.Add(lyrNm);
                    }
                    lyr = rstLyrs.Next();

                }
                rstLyrs = vUtil.getActiveViewLayers(viewUtility.esriIRasterLayer);
                lyr = rstLyrs.Next();
                while (lyr != null)
                {
                    string lyrNm = lyr.Name;
                    IRasterLayer ftrLyr = (IRasterLayer)lyr;
                    IRaster ftrCls = rsUtil.createRaster(((IRaster2)ftrLyr.Raster).RasterDataset);
                    if (!rstDic.ContainsKey(lyrNm))
                    {
                        rstDic.Add(lyrNm, ftrCls);
                        cmbSampleFeatureClass.Items.Add(lyrNm);
                    }
                    lyr = rstLyrs.Next();
                }
            }
        }
        private void getFeaturePath()
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            flt = new ESRI.ArcGIS.Catalog.GxFilterDatasetsClass();
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select a Feature Class, Raster Dataset, or Table";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
                outName = gxObj.BaseName;
                IRaster rs = rsUtil.returnRaster(outPath);
                if (rs == null)
                {
                    if (!ftrDic.ContainsKey(outName))
                    {
                        ftrDic.Add(outName, geoUtil.getTable(outPath));
                        cmbSampleFeatureClass.Items.Add(outName);
                    }
                    else
                    {
                        ftrDic[outName] = geoUtil.getTable(outPath);
                    }
                    
                }
                else
                {
                    if (!rstDic.ContainsKey(outName))
                    {
                        rstDic.Add(outName, rs);
                        cmbSampleFeatureClass.Items.Add(outName);
                    }
                    else
                    {
                        rstDic[outName] = rs;
                    }
                }
                cmbSampleFeatureClass.SelectedItem = outName;
            }
            return;
        }


        private void btnOpenFeture_Click(object sender, EventArgs e)
        {
            getFeaturePath();
        }


        private void btnPlus_Click(object sender, EventArgs e)
        {
            string txt = cmbIndependent.Text;
            if (txt != null && txt != "")
            {
                cmbIndependent.Items.Remove(txt);
                if (!lstIndependent.Items.Contains(txt))
                {
                    lstIndependent.Items.Add(txt);
                }
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection s = lstIndependent.SelectedItems;
            int cnt = s.Count;
            List<string> rLst = new List<string>();
            for (int i = 0; i < cnt; i++)
            {
                string txt = s[i].ToString();
                rLst.Add(txt);
                if (txt != null && txt != "")
                {
                    if (!cmbIndependent.Items.Contains(txt))
                    {
                        cmbIndependent.Items.Add(txt);
                    }
                }
            }
            foreach (string r in rLst)
            {
                lstIndependent.Items.Remove(r);
            }


        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cmbIndependent.Items.Count; i++)
            {
                string st = cmbIndependent.Items[i].ToString();
                lstIndependent.Items.Add(st);
            }
            cmbIndependent.Items.Clear();
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstIndependent.Items.Count; i++)
            {
                string st = lstIndependent.Items[i].ToString();
                cmbIndependent.Items.Add(st);
                
            }
            lstIndependent.Items.Clear();
        }

        private void cmbSampleFeatureClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmbTxt = cmbSampleFeatureClass.Text;
            if (cmbTxt == "" || cmbTxt == null)
            {
                return;
            }
            if (ftrDic.ContainsKey(cmbTxt))
            {
                gpSelection.Enabled = true;
                lstIndependent.Enabled = true;
                cmbIndependent.Enabled = true;
                ITable ftrCls = ftrDic[cmbTxt];
                flds = ftrCls.Fields;
                btnStrataRst.Visible = false;
                cmbIndependent.Items.Clear();
                lstIndependent.Items.Clear();
                cmbStrataField.Items.Clear();
                lblStrat.Text = "Groups Field";
                for (int i = 0; i < flds.FieldCount; i++)
                {
                    IField fld = flds.get_Field(i);
                    string fldNm = fld.Name;
                    esriFieldType fldType = fld.Type;
                    if (fldType != esriFieldType.esriFieldTypeBlob && fldType != esriFieldType.esriFieldTypeDate && fldType != esriFieldType.esriFieldTypeGeometry && fldType != esriFieldType.esriFieldTypeGlobalID && fldType != esriFieldType.esriFieldTypeXML && fldType != esriFieldType.esriFieldTypeGUID && fldType != esriFieldType.esriFieldTypeOID && fldType != esriFieldType.esriFieldTypeRaster)
                    {
                        cmbIndependent.Items.Add(fldNm);
                        cmbStrataField.Items.Add(fldNm);
                    }
                }
            }
            else
            {
                lblStrat.Text = "Groups Raster";
                btnStrataRst.Visible = true;
                cmbIndependent.Items.Clear();
                lstIndependent.Items.Clear();
                cmbStrataField.Items.Clear();
                cmbIndependent.Enabled = false;
                lstIndependent.Enabled = false;
                gpSelection.Enabled = false;
                foreach (string s in rstDic.Keys)
                {
                    cmbStrataField.Items.Add(s);
                }
            }

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            string smpFtrNm = cmbSampleFeatureClass.Text;
            string strataField = cmbStrataField.Text;
            if (strataField.Trim() == ""||strataField==null) strataField = null;
            string outP = txtOutputPath.Text;
            if (smpFtrNm == null || smpFtrNm == "")
            {
                MessageBox.Show("You must select a feature Class","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            
            if (outP == null || outP == "")
            {
                MessageBox.Show("You must select an output model path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Statistics.ModelHelper.runProgressBar("Running T-Test");
                List<string> lstInd = new List<string>();
                for (int i = 0; i < lstIndependent.Items.Count; i++)
                {
                    string s = lstIndependent.Items[i].ToString();
                    lstInd.Add(s);
                }
                if (chbPT.Checked)
                {
                    Statistics.dataPrepPairedTTest pttest = null;
                    if (ftrDic.ContainsKey(smpFtrNm))
                    {
                        if (lstInd.Count < 1)
                        {
                            MessageBox.Show("You must select at least one variable field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        this.Visible = false;
                        ITable ftrCls = ftrDic[smpFtrNm];
                        pttest = new Statistics.dataPrepPairedTTest(ftrCls, lstInd.ToArray(), strataField);
                    }
                    else
                    {
                        IRaster rs = rstDic[smpFtrNm];
                        IRaster rs2 = rstDic[strataField];
                        this.Visible = false;
                        pttest = new Statistics.dataPrepPairedTTest(rs2, rs);
                    }
                    pttest.writeModel(outP);
                    pttest.getReport();
                }
                else
                {
                    Statistics.dataPrepTTest ttest = null;
                    if (ftrDic.ContainsKey(smpFtrNm))
                    {
                        if (lstInd.Count < 1)
                        {
                            MessageBox.Show("You must select at least one variable field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        this.Visible = false;
                        ITable ftrCls = ftrDic[smpFtrNm];
                        ttest = new Statistics.dataPrepTTest(ftrCls, lstInd.ToArray(), strataField);
                    }
                    else
                    {
                        IRaster rs = rstDic[smpFtrNm];
                        IRaster rs2 = rstDic[strataField];
                        this.Visible = false;
                        ttest = new Statistics.dataPrepTTest(rs2, rs);
                    }
                    ttest.writeModel(outP);
                    ttest.getReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Statistics.ModelHelper.closeProgressBar();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Model|*.mdl";
            sd.DefaultExt = "mdl";
            sd.AddExtension = true;
            if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtOutputPath.Text = sd.FileName;
            }
        }

        private void btnStrataRst_Click(object sender, EventArgs e)
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            flt = new ESRI.ArcGIS.Catalog.GxFilterRasterDatasetsClass();
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select a Raster Dataset";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
                outName = gxObj.BaseName;
                IRaster rs = rsUtil.returnRaster(outPath);
                if (!rstDic.ContainsKey(outName))
                {
                    rstDic.Add(outName, rs);
                    cmbStrataField.Items.Add(outName);
                }
                else
                {
                    rstDic[outName] = rs;
                }
                cmbStrataField.SelectedItem = outName;
            }
            return;
        }
    }
}
