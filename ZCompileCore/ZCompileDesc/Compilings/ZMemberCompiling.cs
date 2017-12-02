using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileDesc.Utils;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.Compilings
{
    public class ZMemberCompiling : ZPropertyInfo, IZCompilingInfo
    {
        public string Name { get; set; }
        //public ZType DefType { get; set; }
        public bool HasDefaultValue { get; set; }
        //public bool IsField { get; set; }

        public ZMemberCompiling()
        {
            CanRead = true;
            CanWrite = true;
            AccessAttribute = AccessAttributeEnum.Public;
        }

        public ZMemberCompiling(string name,ZType ztype,bool isStatic )
            :this()
        {
            Name = name;
            _MemberType = ztype;
            //ZNames = new string[] { Name};
            IsStatic = isStatic;
        }

        public override string[] GetZNames()
        {
            return new string[] { Name };
        }

        ZType _MemberType = null;
        public void SetMemberType(ZType ztype)
        {
            _MemberType = ztype;
        }

        public ZType GetMemberType()
        {
            return _MemberType ;
        }

        //bool _isStatic;
        public void SetIsStatic(bool isStatic)
        {
            this.IsStatic = isStatic;
           // _isStatic = isStatic;
        }

        public bool GetIsStatic()
        {
            return this.IsStatic;
        }

        private PropertyBuilder _PropertyBuilder;
        private FieldBuilder _FieldBuilder;

        public void SetBuilder(PropertyBuilder builder)
        {
            MarkProperty = builder;
            SharpProperty = builder;
            _PropertyBuilder = builder;
            _FieldBuilder = null;
        }

        public void SetBuilder(FieldBuilder builder)
        {
            MarkProperty = null;
            SharpProperty = null;
            _PropertyBuilder = null;
            _FieldBuilder = builder;
        }

        public bool IsField()
        {
           return (this._FieldBuilder != null);
        }

        public object GetBuilder()
        {
            if (IsField()) return _FieldBuilder;
            else return _PropertyBuilder;
        }

        //public PropertyBuilder Builder
        //{
        //    get
        //    {
        //        return _PropertyBuilder;
        //    }
        //    set
        //    {
        //        MarkProperty = value;
        //        SharpProperty = value;
        //        _PropertyBuilder = value;
        //        _FieldBuilder = null;
        //    }
        //}

        ////public FieldBuilder _FieldBuilder;
        //public FieldBuilder FieldBuilder
        //{
        //    get
        //    {
        //        return _FieldBuilder;
        //    }
        //    set
        //    {
        //        _PropertyBuilder = null;
        //        //MarkProperty = value;
        //        //SharpProperty = value;
        //        _FieldBuilder = value;
        //    }
        //}

        //public ZPropertyInfo(PropertyInfo markPropertyInfo,PropertyInfo sharpPropertyInfo)
        //{
        //    MarkProperty = markPropertyInfo;
        //    SharpProperty = sharpPropertyInfo;
        //    Init();
        //}

        //private void Init()
        //{
        //    if (MarkProperty.GetGetMethod() != null)
        //        IsStatic= MarkProperty.GetGetMethod().IsStatic;
        //    else
        //        IsStatic = MarkProperty.GetSetMethod().IsStatic;

        //    ZNames = ZDescriptionHelper.GetZNames(MarkProperty);
        //    CanRead = SharpProperty.GetGetMethod()!=null;
        //    CanWrite = SharpProperty.GetSetMethod() != null;

        //    AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpProperty);
        //}

        //public override ZType MemberZType
        //{
        //    get { return ZTypeManager.GetBySharpType(MarkProperty.PropertyType ) as ZType ; }
        //}

        //public override string SharpMemberName
        //{
        //    get { return this.MarkProperty.Name; }
        //}

        public override string ToString()
        {
            return "ZPropertyCompilingInfo("+ Name + ")";
        }
    }
}
