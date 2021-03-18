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
    public class commandSetupToolbar : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public commandSetupToolbar()
        {
        }

        protected override void OnClick()
        {
            IMxDocument mxDoc = ArcMap.Document;
            IMap map = mxDoc.FocusMap;
            esriUtil.Forms.RasterAnalysis.frmSetupToolbar frm = new esriUtil.Forms.RasterAnalysis.frmSetupToolbar();
            frm.Show();
        }

        protected override void OnUpdate()
        {
        }
    }
}
