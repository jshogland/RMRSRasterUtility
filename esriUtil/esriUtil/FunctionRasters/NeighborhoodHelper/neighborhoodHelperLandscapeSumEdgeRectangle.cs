﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;

namespace esriUtil.FunctionRasters.NeighborhoodHelper
{
    class neighborhoodHelperLandscapeSumEdgeRectangle:neighborhoodHelperLandscapeRectangleBase
    {
        public override float findUniqueRegionsValue(Dictionary<int, int[]> uniqueDic)
        {
            float sum = 0;
            foreach (int[] cntArr in uniqueDic.Values)
            {
                float vl = cntArr[1];
                sum += vl;
            }
            return sum;
        }
    }
}
