using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCMethodDesc: ZAMethodDesc,IParts
    {
        #region override

        public override int GetPartCount() { return _parts.Count; }
        public override object[] GetParts() { return Parts; }
        public override string[] GetTextParts() { return _texts.ToArray(); }
        public override ZAParamInfo[] GetZParams() { return ZParams; }
        public override ZABracketDesc[] GetZBrackets() { return _zlbrackets.ToArray(); }
        //public override ZAMethodInfo GetZMethod() { return ZMethod; }
        //public override bool ZEquals(ZMethodDesc zdesc);
        //public override bool ZEquals(ZCallDesc zdesc);
        public override string ToZCode()
        {
            StringBuilder buff = new StringBuilder();
            for (int i = 0; i < index; i++)
            {
                object item = _parts[i];
                if (item is string)
                {
                    buff.Append((string)item);
                }
                else if (item is ZCBracketDesc)
                {
                    string zcode = ((ZCBracketDesc)item).ToZCode();
                    buff.Append(zcode);
                }
                else
                {
                    throw new ZLibRTException();
                }
            }
            return buff.ToString();
        }

        #endregion

        #region 构造函数

        //public ZCMethodDesc()
        //{

        //}

        public ZCMethodDesc(ZCMethodInfo zmethod)
        {
            this.ZMethod = zmethod;
        }

        #endregion


        #region 字段

        private Dictionary<int, object> _parts = new Dictionary<int, object>();
        private List<string> _texts = new List<string>();
        private List<ZCParamInfo> _zlparams = new List<ZCParamInfo>();
        private List<ZCBracketDesc> _zlbrackets = new List<ZCBracketDesc>();
        
        int index = 0;

        #endregion


        #region 属性
        public ZCMethodInfo ZMethod { get; set; }
        public ZCParamInfo[] ZParams { get; private set; }
        public object[] Parts { get { return _parts.Values.ToArray(); } }
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

        public void Add(ZCBracketDesc zbracket)
        {
            _zlbrackets.Add(zbracket);
            _parts.Add(index, zbracket);
            _zlparams.AddRange(zbracket.ZParams);
            index++;
        }

        //public bool ZEquals(ZMethodCall zcall)
        //{
        //    if(!ZDescUtil.ZEqualsIPartsCount(this,zcall) )
        //    {
        //        return false;
        //    }
        //    if (!ZDescUtil.ZEqualsIPartsText(this, zcall))
        //    {
        //        return false;
        //    }
        //    if (!ZDescUtil.ZEqualsIPartsParameters(this, zcall))
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        #endregion
    }
}
