using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc
{
    public static class ZLangBasicTypes
    {
        #region 核心类型
        
        public static ZClassType ZVOID { get; internal set; }
        public static ZClassType ZOBJECT { get; internal set; }
        public static ZClassType ZINT { get; internal set; }
        public static ZClassType ZFLOAT { get; internal set; }
        public static ZClassType ZLIST { get; internal set; }
        public static ZClassType ZBOOL { get; internal set; }
        public static ZClassType ZSTRING { get; internal set; }

        public static ZClassType ZACTION { get; internal set; }
        public static ZClassType ZCONDITION { get; internal set; }
        public static ZClassType ZDATETIME { get; internal set; }
        #endregion
    }
}
