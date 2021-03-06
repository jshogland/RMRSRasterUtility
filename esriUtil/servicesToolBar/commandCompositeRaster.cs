using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;


namespace servicesToolBar
{
    public class commandCompositeRaster : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public commandCompositeRaster()
        {
        }

        protected override void OnClick()
        {
            IMxDocument mxDoc = ArcMap.Document;
            IMap map = mxDoc.FocusMap;
            esriUtil.Forms.RasterAnalysis.frmCompositeRaster frm = new esriUtil.Forms.RasterAnalysis.frmCompositeRaster(map);
            frm.Show();
        }

        protected override void OnUpdate()
        {
        }
    }
}
