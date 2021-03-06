using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Carto;

namespace esriUtil.Forms.Sampling
{
    public partial class frmBootstrap : Form
    {
        public frmBootstrap(IMap map)
        {
            InitializeComponent();
            mp = map;
            frmHlp = new frmHelper(map);
            rsUtil = frmHlp.RasterUtility;
            geoUtil = frmHlp.GeoUtility;
            tblDic = frmHlp.TableDictionary;
            ftrDic = frmHlp.FeatureDictionary;
            ftrUtil = frmHlp.FeatureUtility;
            populateComboBox();
        }
        private frmHelper frmHlp = null;
        private IMap mp = null;
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = null;
        private featureUtil ftrUtil = null;
        private Dictionary<string, ITable> tblDic = null;
        private Dictionary<string, IFeatureClass> ftrDic = null;
        private void getFeaturePath()
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = new ESRI.ArcGIS.Catalog.GxFilterFeatureClasses();
            ESRI.ArcGIS.Catalog.IGxObjectFilterCollection fltCol = (ESRI.ArcGIS.Catalog.IGxObjectFilterCollection)gxDialog;
            fltCol.AddFilter(flt, true);
            fltCol.AddFilter(new ESRI.ArcGIS.Catalog.GxFilterTables(), false);
            //gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
                outName = gxObj.BaseName;
                ITable tbl = geoUtil.getTable(outPath);
                tblDic.Add(outName, tbl);
                cmbSampleFeatureClass.Items.Add(outName);
                cmbSampleFeatureClass.SelectedItem = outName;
            }
            return;
        }
        private void populateComboBox()
        {
            if (mp != null)
            {
                foreach (KeyValuePair<string, IFeatureClass> kvp in ftrDic)
                {
                    IFeatureClass ftrCls = kvp.Value;
                    if (ftrCls != null)
                    {
                        string nm = kvp.Key;
                        cmbSampleFeatureClass.Items.Add(nm);
                    }
                }
                foreach (KeyValuePair<string, ITable> kvp in tblDic)
                {
                    string nm = kvp.Key;
                    cmbSampleFeatureClass.Items.Add(nm);
                }
            }
        }

        private void btnOpenFeatureClass_Click(object sender, EventArgs e)
        {
            getFeaturePath();
        }


        private void btnSample_Click(object sender, EventArgs e)
        {
            string smpFtrNm = cmbSampleFeatureClass.Text;
            if (smpFtrNm == null || smpFtrNm == "")
            {
                MessageBox.Show("Table not specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string outPath = cmbOut.Text;
            if (outPath == null || outPath == "")
            {
                MessageBox.Show("Output file not specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string fldNm = cmbField.Text;
            if (fldNm == null || fldNm == "")
            {
                MessageBox.Show("field name not specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string strataFldNm = cmbStrata.Text;
            this.Visible = false;
            esriUtil.Forms.RunningProcess.frmRunningProcessDialog rp = new RunningProcess.frmRunningProcessDialog(false);
            rp.addMessage("Calculating Bootstrapped estimates");
            rp.addMessage("This may take a while...");
            rp.stepPGBar(20);
            rp.Show();
            rp.Refresh();
            DateTime dt1 = DateTime.Now;
            try
            {
                ITable tbl = null;
                if (ftrDic.Keys.Contains(smpFtrNm))
                {
                    tbl = (ITable)ftrDic[smpFtrNm];
                }
                else
                {
                    tbl = tblDic[smpFtrNm];
                }
                if (tbl.FindField(strataFldNm) == -1) strataFldNm = "";
                bootStrapping bs = new bootStrapping(tbl, fldNm, System.Convert.ToInt32(nudIter.Value), outPath, strataFldNm, System.Convert.ToDouble(nudAlpha.Value), rp);
                bs.runBootStrap();
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


        private void btnOutFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV|*.csv";
            sfd.AddExtension = true;
            System.Windows.Forms.DialogResult rslt = sfd.ShowDialog();
            if (rslt == System.Windows.Forms.DialogResult.OK)
            {
                cmbOut.Text = sfd.FileName;
            }
        }

        private void cmbSampleFeatureClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbField.Items.Clear();
            cmbStrata.Items.Clear();
            string snm = cmbSampleFeatureClass.SelectedItem.ToString();
            ITable tbl = null;
            if(ftrDic.ContainsKey(snm))
            {
                tbl = (ITable)ftrDic[snm];
            }
            else
            {
                tbl = tblDic[snm];
            }
            IFields flds = tbl.Fields;
            for (int i = 0; i < flds.FieldCount; i++)
            {
                IField fld = flds.get_Field(i);
                if (fld.Type == esriFieldType.esriFieldTypeDouble || fld.Type == esriFieldType.esriFieldTypeInteger || fld.Type == esriFieldType.esriFieldTypeSingle || fld.Type == esriFieldType.esriFieldTypeSmallInteger||fld.Type==esriFieldType.esriFieldTypeString)
                {
                    cmbField.Items.Add(fld.Name);
                    cmbStrata.Items.Add(fld.Name);
                }
            }
        }
    }
}