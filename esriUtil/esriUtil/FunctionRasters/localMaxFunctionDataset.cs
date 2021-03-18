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
    public class localMaxFunctionDataset : localFunctionBase
    {
        public override bool getOutPutVl(System.Array[] inArr, int c, int r, out float maxVl)
        {
            int bands = inArr.Length;
            bool checkNoData = true;
            maxVl = float.MinValue;

            for (int i = 0; i < bands; i++)
            {
                object objVl = inArr[i].GetValue(c, r);
                if (objVl == null)
                {
                    checkNoData = false;
                    maxVl = 0;
                    break;
                }
                else
                {
                    float vl = System.Convert.ToSingle(objVl);
                    if (vl > maxVl)
                    {
                        maxVl = vl;
                    }
                }

            }
            return checkNoData;
        }
    }
}
