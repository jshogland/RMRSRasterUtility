﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Framework;


namespace servicesToolBar
{
    public class commandDownloadMapService : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public commandDownloadMapService()
        {
        }

        protected override void OnClick()
        {
            IMxDocument mxDoc = ArcMap.Document;
            IMap map = mxDoc.FocusMap;
            esriUtil.Forms.MapServices.frmDownloadMapServiceLayer frm = new esriUtil.Forms.MapServices.frmDownloadMapServiceLayer(map);
            frm.Show();
        }

        protected override void OnUpdate()
        {
        }
    }
}
