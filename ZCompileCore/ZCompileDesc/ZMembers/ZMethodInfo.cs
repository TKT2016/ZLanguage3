using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Utils;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc.ZMembers
{
    public class ZMethodInfo //: IWordDictionary
    {
        public MethodInfo MarkMethod { get; protected set; }
        public MethodInfo SharpMethod { get; protected set; }
        public bool IsStatic { get; protected set; }

        public List<ZParam> DefArgs { get { return ZDesces[0].DefArgs; } }
        public ZMethodDesc[] ZDesces { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }
        protected ZType _RetZType;
        public virtual ZType RetZType
        {
            get
            {
                if (_RetZType == null)
                {
                    Type rtype = SharpMethod.ReturnType;
                    _RetZType = ZTypeManager.GetBySharpType(rtype) as ZType;

                }
                return _RetZType;
            }
        }

        internal ZMethodInfo( )
        {
            
        }

        public ZMethodInfo(MethodInfo method)
        {
            MarkMethod = method;
            SharpMethod = method;
            Init();
        }

        public ZMethodInfo(MethodInfo markMethod,MethodInfo sharpMethod)
        {
            MarkMethod = markMethod;
            SharpMethod = sharpMethod;
            Init();
        }

        public ZMethodInfo(MethodBuilder builder, bool isStatic, ZMethodDesc[] desces, AccessAttributeEnum accAttr)
        {
            MarkMethod = builder;
            SharpMethod = builder;
            IsStatic = isStatic;
            ZDesces = desces;
            AccessAttribute = accAttr;
        }

        protected void Init()
        {
            IsStatic = SharpMethod.IsStatic;
            ZDesces = GetProcDesc(MarkMethod, SharpMethod);
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpMethod);
        }

        //public bool ContainsWord(string text)
        //{
        //    if (ParamsContainsWord(text)) return true;
        //    if (NameContainsWord(text)) return true;
        //    return false;
        //}

        //private bool ParamsContainsWord(string text)
        //{
        //    var paramsArr = this.SharpMethod.GetParameters();
        //    foreach (var item in paramsArr)
        //    {
        //        if (item.Name.Length == 1) continue;//参数名称一个字符不加入字典
        //        if (item.Name == text)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private bool NameContainsWord(string text)
        //{
        //    foreach (ZMethodDesc part in this.ZDesces)
        //    {
        //        if (part.ContainsWord(text))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public WordInfo SearchWord(string text)
        //{
        //    WordInfo info1 = null;
        //    WordInfo info2 = null;

        //    if(ParamsContainsWord(text))
        //    {
        //        info1 = new WordInfo( text, WordKind.ParamName,this );
        //    }
        //    if (NameContainsWord(text))
        //    {
        //        info2 = new WordInfo(text, WordKind.ProcNamePart, this);
        //    }
        //    WordInfo newWord = WordInfo.Merge(info1, info2);
        //    return newWord;
        //}

        public virtual bool HasZProcDesc(ZCallDesc procDesc)
        {
            foreach (ZMethodDesc item in ZDesces)
            {
                if (item.ZEquals(procDesc))
                    return true;
            }
            return false;
        }

        public virtual bool HasZProcDesc(ZMethodDesc procDesc)
        {
            foreach (ZMethodDesc item in ZDesces)
            {
                if (procDesc.ZEquals(item))
                    return true;
            }
            return false;
        }

        protected ZMethodDesc[] GetProcDesc(MethodInfo markMethod, MethodInfo sharpMethod)
        {
            List<ZMethodDesc> list = new List<ZMethodDesc>();
            ZCodeAttribute[] attrs = AttributeUtil.GetAttributes<ZCodeAttribute>(markMethod);
            foreach (ZCodeAttribute attr in attrs)
            {
                ZCodeParser parser = new ZCodeParser(sharpMethod.DeclaringType,sharpMethod);
                ZMethodDesc typeProcDesc = parser.Parser(attr.Code);
                typeProcDesc.ZMethod = this;
                list.Add(typeProcDesc);
            }
            return list.ToArray();
        }

        public override string ToString()
        {
            return this.MarkMethod.Name + "(" + string.Join(",", ZDesces.Select(p=>p.ToString())) + ")";
        }

        public string SharpMemberName
        {
            get { return this.MarkMethod.Name; }
        }
    }
}
