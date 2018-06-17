using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCClassInfo : ZAClassInfo, ICompling,ZCType
    {
        public override bool IsRuntimeType { get { return false; } }
        #region override

        public override AccessAttrEnum GetAccessAttr() { return AccessAttrEnum.Public; }
        public override string GetZClassName() { return this.ZClassName; }
        public override bool GetIsStatic() { return this.IsStatic; }
        public override ZAClassInfo GetBaseZClass() { return this.BaseZClass; }
        public override ZAPropertyInfo[] GetZPropertys() { return this.ZPropertys; }
        public override ZAFieldInfo[] GetZFields() { return this.ZFields; }
        public override ZAConstructorInfo[] GetZConstructors() { return this.ZConstructors; }
        public override ZAMethodInfo[] GetZMethods() { return this.ZMethods; }
        public override bool IsStruct { get { return false; } }
        #endregion

        #region 属性
        public string ZClassName { get; set; }
        public bool IsStatic { get; set; }
        public ZLClassInfo BaseZClass { get; set; }
        public TypeBuilder ClassBuilder { get; set; }
        #endregion

        #region 成员

        private List<ZCFieldInfo> _ZCompilingFields = new List<ZCFieldInfo>();
        public ZCFieldInfo[] ZFields
        {
            get
            {
                return _ZCompilingFields.ToArray();
            }
        }

        private List<ZCPropertyInfo> _ZCompilingPropertys = new List<ZCPropertyInfo>();
        public ZCPropertyInfo[] ZPropertys
        {
            get
            {
                return _ZCompilingPropertys.ToArray();
            }
        }

        private List<ZCConstructorInfo> _ZCompilingConstructors = new List<ZCConstructorInfo>();
        public ZCConstructorInfo[] ZConstructors
        {
            get
            {
                return _ZCompilingConstructors.ToArray();
            }
        }

        public void AddConstructord(ZCConstructorInfo zmc)
        {
            _ZCompilingConstructors.Add(zmc);
        }

        private List<ZCMethodInfo> _ZCompilingMethods = new List<ZCMethodInfo>();
        public ZCMethodInfo[] ZMethods
        {
            get
            {
                return _ZCompilingMethods.ToArray();
            }
        }

        #endregion

        #region 方法

        public ZCPropertyInfo SearchDeclaredZProperty(string zname)
        {
            foreach(var item in _ZCompilingPropertys)
            {
                if (item.HasZName(zname))
                {
                    return item;
                }
            }
            return null;
        }

        public ZCFieldInfo SearchDeclaredZField(string zname)
        {
            foreach (var item in _ZCompilingFields)
            {
                if (item.HasZName(zname))
                {
                    return item;
                }
            }
            return null;
        }

        public ZCMethodInfo[] SearchDeclaredZMethod(ZMethodCall zdesc)
        {
            List<ZCMethodInfo> methods = new List<ZCMethodInfo>();
            var ZMethods = this._ZCompilingMethods;
            foreach (ZCMethodInfo item in ZMethods)
            {
                if (item.HasZProcDesc(zdesc))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZCMethodInfo[] SearchDeclaredZMethod(ZCMethodDesc zdesc)
        {
            List<ZCMethodInfo> methods = new List<ZCMethodInfo>();
            var ZMethods = this._ZCompilingMethods;
            foreach (ZCMethodInfo item in ZMethods)
            {
                if (item.HasZProcDesc(zdesc))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZAPropertyInfo SearchProperty(string zname)
        {
            var result = SearchDeclaredZProperty(zname);
            if (result != null) return result;
            return this.BaseZClass.SearchProperty(zname);
        }

        public ZCFieldInfo DefineFieldPublic(string name,ZAClassInfo ztype)
        {
            Type varSharpType = ZTypeUtil.GetTypeOrBuilder(ztype);
            FieldBuilder field = ClassBuilder.DefineField(name, varSharpType, FieldAttributes.Public);
            ZCFieldInfo zf = new ZCFieldInfo(name, (ZAClassInfo)ztype, field);
            _ZCompilingFields.Add(zf);
            return zf;
        }

        //public void AddField(ZCFieldInfo zmc)
        //{
        //    _ZCompilingFields.Add(zmc);
        //}

        public ZCPropertyInfo DefinePropertyPublic(string PropertyName)
        {
            var ZPropertyCompiling = new ZCPropertyInfo();
            //PropertyName = NameToken.Text;
             //   this.ClassContext.AddPropertyName(PropertyName);
                ZPropertyCompiling.ZPropertyZName = PropertyName;
             //   this.ClassContext.AddMember(ZPropertyCompiling);
                _ZCompilingPropertys.Add(ZPropertyCompiling);
                return ZPropertyCompiling;
        }

        public void AddProperty(ZCPropertyInfo zmc)
        {
            _ZCompilingPropertys.Add(zmc);
        }

        public void AddMethod(ZCMethodInfo zmc)
        {
            _ZCompilingMethods.Add(zmc);
        }
        #endregion

        public override string ToString()
        {
            return "ZCClassInfo(" + this.ZClassName ?? "none" + ")";
        }
    }
}
