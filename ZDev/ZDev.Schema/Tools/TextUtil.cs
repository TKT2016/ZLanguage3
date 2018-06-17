using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Schema.Tools
{
    public static class TextUtil
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
