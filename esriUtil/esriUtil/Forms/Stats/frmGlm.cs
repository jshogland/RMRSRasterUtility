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
    public partial class frmGlm : Form
    {
        public frmGlm(IMap map)
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
        private viewUtility vUtil = null;
        private Dictionary<string, ITable> ftrDic = new Dictionary<string, ITable>();
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
            }
            cmbLink.Items.AddRange(Enum.GetNames(typeof(Statistics.dataPrepGlm.LinkFunction)));
            cmbLink.SelectedItem = Statistics.dataPrepGlm.LinkFunction.Identity.ToString();
        }
        private void getFeaturePath()
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = null;
            flt = new ESRI.ArcGIS.Catalog.GxFilterTablesAndFeatureClassesClass();
            gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select a Feature";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
                outName = gxObj.BaseName;
                if (!ftrDic.ContainsKey(outName))
                {
                    ftrDic.Add(outName, geoUtil.getTable(outPath));
                    cmbSampleFeatureClass.Items.Add(outName);
                }
                else
                {
                    ftrDic[outName] = geoUtil.getTable(outPath);
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
            if (cmbTxt == null || cmbTxt == "")
            {
                return;
            }
            ITable ftrCls = ftrDic[cmbTxt];
            flds = ftrCls.Fields;
            cmbDepedent.Text = "";
            cmbDepedent.Items.Clear();
            cmbIndependent.Items.Clear();
            lstIndependent.Items.Clear();
            for (int i = 0; i < flds.FieldCount; i++)
            {
                IField fld = flds.get_Field(i);
                string fldNm = fld.Name;
                esriFieldType fldType = fld.Type;
                if (fldType != esriFieldType.esriFieldTypeBlob && fldType != esriFieldType.esriFieldTypeDate && fldType != esriFieldType.esriFieldTypeGeometry && fldType != esriFieldType.esriFieldTypeGlobalID && fldType != esriFieldType.esriFieldTypeXML && fldType != esriFieldType.esriFieldTypeGUID && fldType != esriFieldType.esriFieldTypeOID && fldType != esriFieldType.esriFieldTypeRaster)
                {
                    cmbIndependent.Items.Add(fldNm);
                    cmbDepedent.Items.Add(fldNm);
                }
            }

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            string smpFtrNm = cmbSampleFeatureClass.Text;
            string depStr = cmbDepedent.Text;
            string outPath = txtOutputPath.Text;
            double alpha = System.Convert.ToDouble(nudAlpha.Value);
            string uLinkStr = cmbLink.Text;
            if (smpFtrNm == null || smpFtrNm == "")
            {
                MessageBox.Show("You must select a feature Class or table", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (depStr == "" || depStr == null)
            {
                MessageBox.Show("You must select a dependent field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (uLinkStr == "" || uLinkStr == null)
            {
                MessageBox.Show("You must select a link from the link combo box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(!cmbLink.Items.Contains(uLinkStr))
            {
                MessageBox.Show("You must select a link from the link combo box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (outPath == "" || outPath == null)
            {
                MessageBox.Show("You must select an output Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<string> lstInd = new List<string>();
            List<string> lstCat = new List<string>();
            for (int i = 0; i < lstIndependent.Items.Count; i++)
            {
                string s = lstIndependent.Items[i].ToString();
                lstInd.Add(s);
                IField fld = flds.get_Field(flds.FindField(s));
                if (fld.Type == esriFieldType.esriFieldTypeString)
                {
                    lstCat.Add(s);
                }
            }
            if (lstInd.Count < 1)
            {
                MessageBox.Show("You must select at least one independent field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Statistics.dataPrepGlm.LinkFunction uLink = (Statistics.dataPrepGlm.LinkFunction)Enum.Parse(typeof(Statistics.dataPrepGlm.LinkFunction), uLinkStr);
            this.Visible = false;
            ITable ftrCls = ftrDic[smpFtrNm];
            string[] depArr = { depStr };
            Statistics.dataPrepGlm glm = new Statistics.dataPrepGlm(ftrCls, depArr, lstInd.ToArray(), lstCat.ToArray());
            glm.Link = uLink;
            glm.writeModel(outPath);
            glm.getReport(alpha);
            this.Close();
        }

        private void btnOutputModel_Click(object sender, EventArgs e)
        {
            txtOutputPath.Text = Statistics.ModelHelper.saveModelFileDialog();
        }
    }
}