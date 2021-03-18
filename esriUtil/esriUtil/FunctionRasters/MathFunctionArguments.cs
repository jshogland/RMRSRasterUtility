﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace esriUtil.FunctionRasters
{
    class MathFunctionArguments
    {
        public MathFunctionArguments()
        {
            rsUtil = new rasterUtil();
        }
        public MathFunctionArguments(rasterUtil rasterUtility)
        {
            rsUtil = rasterUtility;
        }
        private IFunctionRasterDataset inrs = null;
        private rasterUtil rsUtil = null;
        public IFunctionRasterDataset InRaster
        {
            get
            {
                return inrs;
            }
            set
            {
                inrs = rsUtil.createIdentityRaster(value,rstPixelType.PT_FLOAT);
            }
        }
    }
}
