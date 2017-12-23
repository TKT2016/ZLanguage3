using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;

namespace ZCompileDesc
{
    public static class ZLangBasicTypes
    {
        #region 核心类型

        public static ZLClassInfo ZVOID { get; internal set; }
        public static ZLClassInfo ZOBJECT { get; internal set; }
        public static ZLClassInfo ZINT { get; internal set; }
        public static ZLClassInfo ZFLOAT { get; internal set; }
        public static ZLClassInfo ZLIST { get; internal set; }
        public static ZLClassInfo ZBOOL { get; internal set; }
        public static ZLClassInfo ZSTRING { get; internal set; }

        public static ZLClassInfo ZACTION { get; internal set; }
        public static ZLClassInfo ZCONDITION { get; internal set; }
        public static ZLClassInfo ZDATETIME { get; internal set; }
        #endregion
    }
}
