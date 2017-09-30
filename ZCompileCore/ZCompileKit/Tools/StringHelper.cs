﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileKit.Tools
{
    public static class StringHelper
    {
        public static bool IsInt(string str)
        {
            int value = -1;
            bool result = int.TryParse(str, out value);
            return result;
        }

        public static bool IsFloat(string str)
        {
            float value = -1;
            bool result = float.TryParse(str, out value);
            return result;
        }
    }
}
