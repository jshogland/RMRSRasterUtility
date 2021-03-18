using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GISClient;
using ESRI.ArcGIS.Server;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMap;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesNetCDF;
using System.Windows.Forms;
using esriUtil;
using System.Threading;
using Accord.Statistics.Testing;
using System.Net;
using System.Collections;
using ESRI.ArcGIS.GeoDatabaseExtensions;
//using System.Windows.Forms.DataVisualization;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;



namespace TestConsole
{
    class Program
    {

        private static LicenseInitializer m_AOLicenseInitializer = new TestConsole.LicenseInitializer();
        [STAThread()]
        static void Main(string[] args)
        {
            //ESRI License Initializer generated code.
            m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeAdvanced}, new esriLicenseExtensionCode[] {esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst});//{esriLicenseExtensionCode.esriLicenseExtensionCode3DAnalyst});
            System.DateTime dt = System.DateTime.Now;
            
            System.DateTime dt2;
            TimeSpan ts;

            //string path = @"C:\Users\jshogland\Documents\John\projects\Code\RMRSRasterUtility\esriUtil\esriUtil\bin\x86\Debug\esriUtil.dll";
            //string path2 = @"C:\Users\jshogland\Documents\John\projects\Code\RMRSRasterUtility\esriUtil\servicesToolBar\bin\x86\Debug\servicetoolBar.dll";
            //System.Reflection.Assembly sampAss = System.Reflection.Assembly.LoadFrom(path);
            //int classes = sampAss.GetTypes().Where(t => t.IsClass).Count();
            //sampAss = System.Reflection.Assembly.LoadFrom(path2);
            //classes = classes + sampAss.GetTypes().Where(t => t.IsClass).Count();
            //Console.WriteLine("Number of classes = " + classes.ToString());
            Console.WriteLine("got license");

            esriUtil.geoDatabaseUtility geoUtil = new geoDatabaseUtility();
            esriUtil.rasterUtil rsUtil = new rasterUtil();
            esriUtil.featureUtil ftrUtil = new featureUtil(rsUtil);
            string roadsPath = @"C:\Users\jshogland\Documents\John\projects\BMFP\data\BMCFLRP.gdb\roadClip";
            string roadsField = "Speed";
            string demPath = @"C:\Users\jshogland\Documents\John\projects\BMFP\data\Elevation\DEM30SHORT.tif";
            string facilitiesPath = @"C:\Users\jshogland\Documents\John\projects\BMFP\data\BMCFLRP.gdb\Facility";
            int onPay = 28;
            int offPay = 4;
            float onRate = 90f;
            float offRate = 85f;
            float offSpeed = 5.5f;
            TransRouting.SpeedUnits units = TransRouting.SpeedUnits.KPH;

            string barPath = @"C:\Users\jshogland\Documents\John\projects\BMFP\data\BMCFLRP.gdb\barVectClip";
            string outDir = @"C:\Users\jshogland\Documents\John\projects\BMFP\data\DeliveredCost\Run1";
            IWorkspace outWks = geoUtil.OpenRasterWorkspace(outDir);

            IFeatureClass rdsFtr = geoUtil.getFeatureClass(roadsPath);
            IFeatureClass facFtr = geoUtil.getFeatureClass(facilitiesPath);
            IFunctionRasterDataset demFDS = rsUtil.createIdentityRaster(demPath);
            IFeatureClass barFtr = geoUtil.getFeatureClass(barPath);

            esriUtil.TransRouting tr = new TransRouting(outWks, facFtr, rdsFtr, roadsField, demFDS, onRate, onPay, offSpeed, offRate, offPay, barFtr, 0, 0, units);
            IFunctionRasterDataset DollarsPerTon = tr.OutDollarsTonsRaster;












            dt2 = System.DateTime.Now;
            ts = dt2.Subtract(dt);
            Console.WriteLine("Total Seconds = " + ts.TotalSeconds.ToString());

            m_AOLicenseInitializer.ShutdownApplication();
                
        }
    }
}
