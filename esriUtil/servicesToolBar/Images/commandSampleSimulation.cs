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
    public class commandSampleSimulation : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public commandSampleSimulation()
        {
        }

        protected override void OnClick()
        {
            IMxDocument mxDoc = ArcMap.Document;
            IMap map = mxDoc.FocusMap;
            esriUtil.Forms.Sampling.frmSampleIntensity frm = new esriUtil.Forms.Sampling.frmSampleIntensity(map);
            frm.Show();
        }

        protected override void OnUpdate()
        {
        }
    }
}
