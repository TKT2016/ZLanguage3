using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLConstructorInfo : ZAConstructorInfo, ICompleted
    {
        #region override

        public override AccessAttrEnum GetAccessAttr() { return AccessAttr; }
        public override bool GetIsStatic() { return IsStatic; }
        //public override ZAClassInfo GetZAClass() { return ZClass; }
        //public override ZAParamInfo[] GetZParams() { return ZParams; }
        //public override ZAConstructorDesc GetZDesc() { return ZDesc; }

        #endregion

        public ZLConstructorInfo(ConstructorInfo constructorInfo, ZLClassInfo zclass)
        {
            Constructor = constructorInfo;
            ZClass = zclass;
            Init();
        }

        protected void Init()
        {
            ZDesc = ZClassUtil.CreateZConstructorDesc(Constructor, this,this.ZClass);
            //if(ZDesc==null)
            //{
            //    Console.WriteLine("ZLConstructorInfo Init ZDesc==null");
            //}
            //else if (ZDesc.ZBracketDesc == null)
            //{
            //    Console.WriteLine("ZLConstructorInfo Init ZDesc.ZBracketDesc==null");
            //}
            AccessAttr = ZClassUtil.GetAccessAttributeEnum(Constructor);
            IsStatic = Constructor.IsStatic;
        }

        public virtual bool HasZConstructorDesc(ZNewCall newCall)
        {
            return ZDescUtil.ZEqualsDesc(this.ZDesc,newCall);
        }

        public ZLConstructorDesc ZDesc { get; protected set; }
        public ConstructorInfo Constructor { get; private set; }
        public AccessAttrEnum AccessAttr { get; private set; }
        public bool IsStatic { get; private set; }
        public ZLClassInfo ZClass { get; private set; }

        ZLParamInfo[] _zparams ;
        public ZLParamInfo[] ZParams
        {
            get
            {
                if(_zparams==null)
                {
                    List<ZLParamInfo> paramlist = new List<ZLParamInfo>();
                    int index = 0;
                    foreach (ParameterInfo param in Constructor.GetParameters())
                    {
                        ZLParamInfo zlp = new ZLParamInfo(param, this,index);//, false);
                        paramlist.Add(zlp);
                        index++;
                    }
                    _zparams = paramlist.ToArray();
                }
                return _zparams;
            }
        }

    }
}
