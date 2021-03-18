using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesRaster;

namespace saveSubset
{
    class Program
    {
        private static LicenseInitializer m_AOLicenseInitializer = new saveSubset.LicenseInitializer();
        /// <summary>
        /// saves a subset of an input raster dataset or batch file
        /// </summary>
        /// <param name="args">inRasterPath outName outWorkspacePath rsTypeString extentToSave(xmin:ymin:xmax:ymax) {noDataValue} {BlockWidth} {BlockHeight}</param>
        [STAThread()]
        static void Main(string[] args)
        {
            //ESRI License Initializer generated code.
            m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeBasic, esriLicenseProductCode.esriLicenseProductCodeStandard, esriLicenseProductCode.esriLicenseProductCodeAdvanced },
            new esriLicenseExtensionCode[] { });
            if (args.Length > 0)
            {
                DateTime dt1 = DateTime.Now;
                string inRasterPath = args[0];
                string outName = args[1];
                string wksPath = args[2];
                esriUtil.geoDatabaseUtility geoUtil = new esriUtil.geoDatabaseUtility();
                esriUtil.rasterUtil rsUtil = new esriUtil.rasterUtil();
                IWorkspace wks = geoUtil.OpenRasterWorkspace(wksPath);
                outName = rsUtil.getSafeOutputName(wks, outName);
                string rsTypeString = args[3];
                esriUtil.rasterUtil.rasterType rastertype = (esriUtil.rasterUtil.rasterType)Enum.Parse(typeof(esriUtil.rasterUtil.rasterType), rsTypeString);
                object noDataVl = null;
                int IntBlockWidth = 512;
                int IntBlockHeight = 512;
                if (args.Length > 5)
                {
                    string noDtStr = args[5];
                    if (!rsUtil.isNumeric(noDtStr))
                    {
                        noDataVl = null;
                    }
                    else
                    {
                        noDataVl = System.Convert.ToDouble(noDtStr);
                    }
                    string bw = args[6];
                    if (rsUtil.isNumeric(bw)) IntBlockWidth = System.Convert.ToInt32(bw);
                    string bh = args[7];
                    if (rsUtil.isNumeric(bh)) IntBlockHeight = System.Convert.ToInt32(bh);

                }
                if (args[4].Contains(";"))
                {
                    IEnvelope extent = new EnvelopeClass();
                    string envExtent = args[4];
                    string[] envArr = envExtent.Split(new char[] { ';' });
                    extent.PutCoords(System.Convert.ToDouble(envArr[0]), System.Convert.ToDouble(envArr[1]), System.Convert.ToDouble(envArr[2]), System.Convert.ToDouble(envArr[3]));
                    Console.WriteLine("Saving " + outName);
                    rsUtil.subsetSaveRasterToDataset(inRasterPath, outName, wks, rastertype, extent, noDataVl, IntBlockWidth, IntBlockHeight);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2.Subtract(dt1);
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.GetDirectoryName(wksPath) + "\\timeLog_" + outName + ".txt"))
                    {
                        sw.WriteLine("Total processing time = " + ts.ToString());
                        sw.WriteLine("Start time = " + dt1.ToString());
                        sw.WriteLine("End time = " + dt2.ToString());
                        sw.Flush();
                        sw.Close();
                    }
                }
                else
                {
                    int processors = System.Environment.ProcessorCount;
                    Console.WriteLine(args[4]);
                    IFeatureClass tiles = geoUtil.getFeatureClass(args[4]);
                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    IFeatureCursor ftrCur = tiles.Search(null, true);
                    IFeature ftr = ftrCur.NextFeature();
                    List<IEnvelope> envLst = new List<IEnvelope>();
                    while (ftr != null)
                    {
                        IGeometry geo = ftr.ShapeCopy;
                        envLst.Add(geo.Envelope);
                        ftr = ftrCur.NextFeature();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ftrCur);
                    int cnt = 1;
                    for (int i = 0; i < envLst.Count; i += processors)
                    {
                        System.Diagnostics.Process lp = null;
                        for (int k = 0; k < processors; k++)
                        {
                            int index = i + k;
                            if (index < envLst.Count)
                            {
                                IEnvelope ext = envLst[index];
                                string extStr = ext.XMin.ToString() + ";" + ext.YMin.ToString() + ";" + ext.XMax.ToString() + ";" + ext.YMax.ToString();
                                if (noDataVl == null) noDataVl = "null";
                                string cmd = inRasterPath + " " + outName + "_" + cnt.ToString() + " " + wksPath + " " + rsTypeString + " " + extStr + " " + noDataVl + " " + IntBlockWidth.ToString() + " " + IntBlockHeight.ToString();
                                Console.WriteLine("Running: " + cmd);
                                System.Diagnostics.Process pr = new System.Diagnostics.Process();
                                System.Diagnostics.ProcessStartInfo stInfo = pr.StartInfo;
                                stInfo.FileName = exePath;
                                stInfo.Arguments = cmd;
                                pr.Start();
                                lp = pr;
                                cnt++;
                                System.Threading.Thread.Sleep(2000);
                            }

                        }
                        if (lp != null)
                        {
                            lp.WaitForExit();
                        }
                    }
                }
            }
            else
            {
                esriUtil.Forms.RasterAnalysis.frmSaveParallel frm = new esriUtil.Forms.RasterAnalysis.frmSaveParallel(null);
                System.Windows.Forms.Application.Run(frm);
            }
            m_AOLicenseInitializer.ShutdownApplication();
            Environment.Exit(0);
        }
    }
}
