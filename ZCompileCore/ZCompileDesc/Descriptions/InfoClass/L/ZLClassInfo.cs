using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLClassInfo  : ZAClassInfo, ICompleted, ZLType, IZLObj
    {
        #region override

        public override AccessAttrEnum GetAccessAttr() { return this._AccessAttribute; }
        public override string GetZClassName() { return this.ZClassName; }
        public override bool GetIsStatic() { return this._IsStatic; }
        public override ZAClassInfo GetBaseZClass() { return this.BaseZClass; }
        public override ZAPropertyInfo[] GetZPropertys() { return this.ZPropertys; }
        public override ZAFieldInfo[] GetZFields() { return this.ZFields; }
        public override ZAConstructorInfo[] GetZConstructors() { return this.ZConstructors; }
        public override ZAMethodInfo[] GetZMethods() { return this.ZMethods; }
        public override bool IsStruct { get { return ReflectionUtil.IsStruct(this.SharpType); } }

        #endregion

        #region field

        private Type _MarkType;
        private Type _SharpType;
        private bool _IsStatic;
        private AccessAttrEnum _AccessAttribute;
        private string _ZClassName;
        #endregion

        #region 构造函数

        public ZLClassInfo(Type markType, Type sharpType, bool isStatic)
        {
            _MarkType = markType;
            _SharpType = sharpType;
            _IsStatic = isStatic;
            _AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(_SharpType);
           
        }

        #endregion

        #region 属性

        public string ZClassName
        {
            get
            {
                if (_ZClassName == null)
                {
                    if (!this._MarkType.IsGenericType)
                        _ZClassName =  _MarkType.Name;
                    else
                        _ZClassName = GenericUtil.GetGenericTypeShortName(this._MarkType);
                }
                return _ZClassName;
            }
        }

        public ZLClassInfo BaseZClass
        {
            get
            {
                if (!_IsStatic && _SharpType != typeof(object))
                {
                    return ZTypeManager.GetBySharpType(_SharpType.BaseType) as ZLClassInfo;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsStatic
        {
            get { return _IsStatic; }
        }

        public Type MarkType
        {
            get
            {
                return _MarkType;
            }
        }

        public Type SharpType
        {
            get
            {
                return _SharpType;
            }
        }

        #endregion

        #region 成员,方法

        public ZLClassInfo MakeGenericType(params ZType[] argZTypes)
        {
            var args = argZTypes.Select(U => ZTypeUtil.GetTypeOrBuilder(U)).ToArray();
            Type newtype= this.SharpType.MakeGenericType(args);
            ZLClassInfo newclassinfo = new ZLClassInfo(newtype, newtype,IsStatic);

            newclassinfo._GenericTypeDict = new Dictionary<string, Type>();
            Type[] typeArguments = this.SharpType.GetGenericArguments();
            for (int i = 0; i < typeArguments.Length;i++ )
            {
                newclassinfo._GenericTypeDict.Add(typeArguments[i].Name, args[i]);
            }
            return newclassinfo;
        }

        private Dictionary<string, Type>  _GenericTypeDict;
        public Dictionary<string, Type> GenericTypeDict
        {
            get{
                if(_GenericTypeDict==null)
                {
                    _GenericTypeDict = GenericUtil.GetTypeNameGenericArgTypes(SharpType); 
                }
                return _GenericTypeDict;
            }
        }

        private ZLFieldInfo[] _ZFields;
        public ZLFieldInfo[] ZFields
        {
            get
            {
                if (_ZFields == null)
                {
                    _ZFields = ZClassUtil.GetZFields(_MarkType, _SharpType, _IsStatic,this);
                }
                return _ZFields;
            }
        }

        private ZLPropertyInfo[] _ZPropertys;
        public ZLPropertyInfo[] ZPropertys
        {
            get
            {
                if (_ZPropertys == null)
                {
                    _ZPropertys = ZClassUtil.GetZPropertys(_MarkType, _SharpType, _IsStatic, this);
                }
                return _ZPropertys;
            }
        }

        private ZLConstructorInfo[] _ZConstructors;
        public ZLConstructorInfo[] ZConstructors
        {
            get
            {
                if (_ZConstructors == null)
                {
                    _ZConstructors = ZClassUtil.GetZConstructors(_MarkType, _SharpType, _IsStatic, this);
                }
                return _ZConstructors;
            }
        }

        private ZLMethodInfo[] _ZMethods;
        public ZLMethodInfo[] ZMethods
        {
            get
            {
                if (_ZMethods == null)
                {
                    _ZMethods = ZClassUtil.GetZMethods(_MarkType, _SharpType, _IsStatic, this);
                }
                return _ZMethods;
            }
        }

        #endregion

        #region 次要改进

        public override string ToString()
        {
            return this._MarkType.Name + "-" + this._SharpType.Name;
        }

        #endregion

        #region 搜索

        public ZLPropertyInfo SearchDeclaredZProperty(string zname)
        {
            foreach (var item in this.ZPropertys)
            {
                if (item.HasZName(zname))
                {
                    return item;
                }
            }
            return null;
        }

        public ZLFieldInfo SearchDeclaredZField(string zname)
        {
            foreach (var item in this.ZFields)
            {
                if (item.HasZName(zname))
                {
                    return item;
                }
            }
            return null;
        }

        public ZLPropertyInfo SearchProperty (string zname)
        {
            ZLClassInfo temp = this;
            while(temp!=null)
            {
                ZLPropertyInfo zp = temp.SearchDeclaredZProperty(zname);
                if(zp!=null)
                {
                    return zp;
                }
                else
                {
                    temp = temp.BaseZClass;
                }
            }
            return null;
        }

        public ZLFieldInfo SearchField(string zname)
        {
            ZLClassInfo temp = this;
            while (temp != null)
            {
                ZLFieldInfo zp = temp.SearchDeclaredZField(zname);
                if (zp != null)
                {
                    return zp;
                }
                else
                {
                    temp = temp.BaseZClass;
                }
            }
            return null;
        }

        public ZLConstructorInfo[] SearchDeclaredZConstructor(ZNewCall znew)
        {
            List<ZLConstructorInfo> constructors = new List<ZLConstructorInfo>();
            var ZConstructors = this.GetZConstructors();
            foreach (ZLConstructorInfo item in ZConstructors)
            {
                if (item.HasZConstructorDesc(znew))
                {
                    constructors.Add(item);
                }
            }
            return constructors.ToArray();
        }

        public ZLMethodInfo[] SearchDeclaredZMethod(ZMethodCall zcall)
        {
            List<ZLMethodInfo> methods = new List<ZLMethodInfo>();
            var ZMethods = this.ZMethods; //.GetZMethods();
            foreach (var item in ZMethods)
            {
                if (item.HasZProcDesc(zcall))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZLMethodInfo[] SearchDeclaredZMethod(ZCMethodDesc zcall)
        {
            List<ZLMethodInfo> methods = new List<ZLMethodInfo>();
            var ZMethods = this.ZMethods;
            foreach (var item in ZMethods)
            {
                if (item.HasZProcDesc(zcall))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZLMethodInfo[] SearchZMethod(ZMethodCall zcall)
        {
            ZLClassInfo temp = this;
            while (temp != null)
            {
                ZLMethodInfo[] zmethods = temp.SearchDeclaredZMethod(zcall);
                if (zmethods.Length > 0)
                {
                    return zmethods;
                }
                else
                {
                    temp = temp.BaseZClass;
                }
            }
            return new ZLMethodInfo[] { };
        }

        public ZLMethodInfo[] SearchZMethod(ZCMethodDesc zdesc)
        {
            ZLClassInfo temp = this;
            while (temp != null)
            {
                ZLMethodInfo[] zmethods = temp.SearchDeclaredZMethod(zdesc);
                if (zmethods.Length > 0)
                {
                    return zmethods;
                }
                else
                {
                    temp = temp.BaseZClass;
                }
            }
            return new ZLMethodInfo[] { };
        }

        #endregion
    }
}
