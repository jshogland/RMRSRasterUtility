﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;

namespace esriUtil.FunctionRasters.NeighborhoodHelper
{
    class neighborhoodHelperLandscapeMaxAreaRectangle: neighborhoodHelperLandscapeRectangleBase
    {
        public override float findUniqueRegionsValue(Dictionary<int, int[]> uniqueDic)
        {
            int maxVl = 0;
            foreach (int[] cntArr in uniqueDic.Values)
            {
                int cnt = cntArr[0];
                if (cnt > maxVl)
                {
                    maxVl = cnt;
                }
            }
            return maxVl;
        }
    }
}
