using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZLMethodDesc : ZAMethodDesc, IParts
    {
        
        #region override

        public override int GetPartCount() { return _parts.Count; }
        public override object[] GetParts() { return _parts.Values.ToArray(); }
        public override string[] GetTextParts() { return _texts.ToArray(); }
        public override ZAParamInfo[] GetZParams() { return ZLParams; }
        public override ZABracketDesc[] GetZBrackets() { return _zlbrackets.ToArray(); }
        //public override ZAMethodInfo GetZMethod() { return ZMethod; }
        //public override bool ZEquals(ZMethodDesc zdesc);
        //public override bool ZEquals(ZCallDesc zdesc);
        public override string ToZCode()
        {
            return _ZCode;
            //StringBuilder buff = new StringBuilder();
            //for (int i = 0; i < index; i++)
            //{
            //    object item = _parts[i];
            //    if (item is string)
            //    {
            //        buff.Append((string)item);
            //    }
            //    else if (item is ZLBracketDesc)
            //    {
            //        string zcode = ((ZLBracketDesc)item).ToZCode();
            //        buff.Append(zcode);
            //    }
            //    else
            //    {
            //        throw new ZLibRTException();
            //    }
            //}
            //return buff.ToString();
        }

        #endregion

        #region 构造函数

        public ZLMethodDesc(ZLMethodInfo zmethod, string zcode)
        {
            this.ZMethod = zmethod;
            _ZCode = zcode;
        }

        #endregion


        #region 字段

        private Dictionary<int, object> _parts = new Dictionary<int, object>();
        private List<string> _texts = new List<string>();
        private List<ZLParamInfo> _zlparams = new List<ZLParamInfo>();
        private List<ZLBracketDesc> _zlbrackets = new List<ZLBracketDesc>();
        
        int index = 0;
        private string _ZCode = null;
        #endregion


        #region 属性
        public ZLMethodInfo ZMethod { get;private set; }
        public ZLParamInfo[] ZLParams { get { return _zlparams.ToArray(); } }

        #endregion


        #region 方法

        public object GetPart(int i)
        {
            return _parts[i];
        }

        public void Add(string str)
        {
            _texts.Add(str);
            _parts.Add(index, str);
            index++;
        }

        public void Add(ZLBracketDesc zbracket)
        {
            _zlbrackets.Add(zbracket);
            _parts.Add(index, zbracket);
            _zlparams.AddRange(zbracket.ZParams);
            index++;
        }
        #endregion

        #region 辅助

        #endregion

    }
}
