using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;

namespace DownloadData
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static LicenseInitializer m_AOLicenseInitializer = new DownloadData.LicenseInitializer();
        [STAThread]
        static void Main()
        {
            m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeAdvanced }, new esriLicenseExtensionCode[] { });
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDownLoad());
            m_AOLicenseInitializer.ShutdownApplication();
        }
    }
}
