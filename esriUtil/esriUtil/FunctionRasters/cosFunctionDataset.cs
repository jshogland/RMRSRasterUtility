﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace esriUtil.FunctionRasters
{
    public class cosFunctionDataset : mathFunctionBase
    {
        public override double getFunctionValue(double inValue)
        {
            return Math.Cos(inValue);
        }

    }
}