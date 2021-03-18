using System;
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
    public partial class frmSampleIntensity : Form
    {
        public frmSampleIntensity(IMap map)
        {
            InitializeComponent();
            mp = map;
            frmHlp = new frmHelper(map);
            rsUtil = frmHlp.RasterUtility;
            geoUtil = frmHlp.GeoUtility;
            ftrDic = frmHlp.FeatureDictionary;
            rstDic = frmHlp.FunctionRasterDictionary;
            ftrUtil = frmHlp.FeatureUtility;
            populateComboBox();
        }
        private frmHelper frmHlp = null;
        private IMap mp = null;
        private geoDatabaseUtility geoUtil = new geoDatabaseUtility();
        private rasterUtil rsUtil = null;
        private featureUtil ftrUtil = null;
        private Dictionary<string, IFeatureClass> ftrDic = null;
        private Dictionary<string, IFunctionRasterDataset> rstDic = null;
        private void getFeaturePath()
        {
            string outPath = null;
            string outName = "";
            ESRI.ArcGIS.CatalogUI.IGxDialog gxDialog = new ESRI.ArcGIS.CatalogUI.GxDialogClass();
            gxDialog.AllowMultiSelect = false;
            ESRI.ArcGIS.Catalog.IGxObjectFilter flt = new ESRI.ArcGIS.Catalog.GxFilterPolygonFeatureClasses();
            ESRI.ArcGIS.Catalog.IGxObjectFilterCollection fltCol = (ESRI.ArcGIS.Catalog.IGxObjectFilterCollection)gxDialog;
            fltCol.AddFilter(new ESRI.ArcGIS.Catalog.GxFilterRasterDatasets(), true);
            fltCol.AddFilter(flt, false);
            //gxDialog.ObjectFilter = flt;
            gxDialog.Title = "Select";
            ESRI.ArcGIS.Catalog.IEnumGxObject eGxObj;
            if (gxDialog.DoModalOpen(0, out eGxObj))
            {
                ESRI.ArcGIS.Catalog.IGxObject gxObj = eGxObj.Next();
                outPath = gxObj.FullName;
                outName = gxObj.BaseName;
                IFunctionRasterDataset tRs = rsUtil.createIdentityRaster(outPath);
                if(tRs==null)
                {
                    ftrDic[outName] = geoUtil.getFeatureClass(outPath);
                }
                else
                {
                    rstDic[outName] = tRs;
                }
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
                        if (ftrCls.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
                        {
                            cmbSampleFeatureClass.Items.Add(nm);
                        }
                    }
                }
                foreach (KeyValuePair<string, IFunctionRasterDataset> kvp in rstDic)
                {
                    IFunctionRasterDataset rsDet = kvp.Value;
                    string nm = kvp.Key;
                    cmbSampleFeatureClass.Items.Add(nm);
                }

                foreach (string s in Enum.GetNames(typeof(Design)))
                {
                    cmbDesign.Items.Add(s);
                }
                cmbDesign.SelectedItem = Design.Random.ToString();
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
                MessageBox.Show("feature class or raster are not specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string outPath = cmbOut.Text;
            if(outPath==null||outPath=="")
            {
                MessageBox.Show("Output file not specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Design dType = (Design)Enum.Parse(typeof(Design),cmbDesign.Text);
            string fldNm = cmbField.Text;
            if(fldNm==null||fldNm=="")
            {
                MessageBox.Show("field name not specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Visible = false;
            esriUtil.Forms.RunningProcess.frmRunningProcessDialog rp = new RunningProcess.frmRunningProcessDialog(false);
            rp.addMessage("Running Simmulations");
            rp.addMessage("This may take a while...");
            rp.stepPGBar(20);
            rp.Show();
            rp.Refresh();
            DateTime dt1 = DateTime.Now;
            try
            {                
                if (rstDic.Keys.Contains(smpFtrNm))
                {

                    IFunctionRasterDataset fDset = rstDic[smpFtrNm];
                    SampleSimulation ss = new SampleSimulation(fDset, outPath, System.Convert.ToDouble(nudPlots.Value), System.Convert.ToDouble(nudArea.Value), System.Convert.ToInt32(nudIter.Value), dType, chbContinuous.Checked, rsUtil, chbPoints.Checked, rp);
                    ss.runSimulation();
                }
                else
                {
                    IFeatureClass ftrCls = ftrDic[smpFtrNm];
                    SampleSimulation ss = new SampleSimulation(ftrCls, fldNm, outPath, System.Convert.ToDouble(nudPlots.Value), System.Convert.ToDouble(nudArea.Value), System.Convert.ToInt32(nudIter.Value), dType, chbContinuous.Checked, rsUtil, chbPoints.Checked, rp);
                    ss.runSimulation();
                }
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
            if (rslt== System.Windows.Forms.DialogResult.OK)
            {
                cmbOut.Text = sfd.FileName;
            }
        }

        private void cmbSampleFeatureClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbField.Items.Clear();
            if (ftrDic.ContainsKey(cmbSampleFeatureClass.SelectedItem.ToString()))
            {
                cmbField.Enabled = true;
                IFeatureClass ftrCls = ftrDic[cmbSampleFeatureClass.SelectedItem.ToString()];
                IFields flds = ftrCls.Fields;
                for (int i = 0; i < flds.FieldCount; i++)
                {
                    IField fld = flds.get_Field(i);
                    if (fld.Type == esriFieldType.esriFieldTypeDouble || fld.Type == esriFieldType.esriFieldTypeInteger || fld.Type == esriFieldType.esriFieldTypeSingle || fld.Type == esriFieldType.esriFieldTypeSmallInteger)
                    {
                        cmbField.Items.Add(fld.Name);
                    }
                }
            }
            else if (rstDic.ContainsKey(cmbSampleFeatureClass.SelectedItem.ToString()))
            {
                cmbField.Enabled = false;
                cmbField.Items.Add("VALUE");
                cmbField.SelectedItem = "VALUE";
            }
            else
            {
            }
        }
    }
}
