﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using esriUtil.FunctionRasters.NeighborhoodHelper;

namespace esriUtil.FunctionRasters
{
    class nullToValueFunctionArguments: IRasterInfoFunctionArguments
    {
        public nullToValueFunctionArguments()
        {
            
            rsUtil = new rasterUtil();
        }
        public nullToValueFunctionArguments(rasterUtil rasterUtility)
        {
            
            rsUtil = rasterUtility;
        }
        private rasterUtil rsUtil = null;
        
        public object Raster
        {
            get;
            set;
        }
        public IRasterInfo RasterInfo
        {
            get;
            set;
        }
        public bool Caching
        {
            get;
            set;
        }
        private double newvalue = 0d;
        public double NewValue { get { return newvalue; } set { newvalue = value;} }
        public System.Array NoDataArray {
            get {
                System.Array outArr = new int[1];

                object noDataValue = RasterInfo.NoData;
                if(noDataValue is System.Array){
                    outArr = (System.Array)noDataValue;
                }
                else
                {
                    outArr.SetValue(System.Convert.ToInt32(noDataValue),0);
                }
                return outArr;
            }
        }

    }
}

