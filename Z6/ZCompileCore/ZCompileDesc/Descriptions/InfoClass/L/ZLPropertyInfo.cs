using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLPropertyInfo : ZAPropertyInfo, ICompleted,IIdent
    {
        #region override

        public override AccessAttrEnum GetAccessAttr() { return this.AccessAttr; }
        public override bool GetIsStatic() { return this.IsStatic; }
        public override bool GetCanRead() { return this._CanRead; }
        public override bool GetCanWrite() { return this._CanWrite; }
        public override string[] GetZPropertyZNames() { return this.ZPropertyZNames; }
        public override ZType GetZPropertyType() { return this.ZPropertyType; }
        public override ZAClassInfo GetZAClass() { return this.ZClass; }

        #endregion

        #region 字段

        private bool _IsStatic;
        private bool _CanRead;
        private bool _CanWrite;
        private ZLType _ZPropertyType;
        private AccessAttrEnum _AccessAttribute;
        private string[] _ZNames;
        #endregion

        #region 构造函数

        public ZLPropertyInfo(ZLClassInfo zClass, PropertyInfo propertyInfo)
            : this(zClass,propertyInfo, propertyInfo)
        {
           
        }

        public ZLPropertyInfo(ZLClassInfo zClass,PropertyInfo markPropertyInfo, PropertyInfo sharpPropertyInfo)
        {
            ZClass = zClass;
            MarkProperty = markPropertyInfo;
            SharpProperty = sharpPropertyInfo;
            Init();
        }

        private void Init()
        {
            if (MarkProperty.GetGetMethod() != null)
                _IsStatic = MarkProperty.GetGetMethod().IsStatic;
            else
                _IsStatic = MarkProperty.GetSetMethod().IsStatic;

            _CanRead = SharpProperty.GetGetMethod() != null;
            _CanWrite = SharpProperty.GetSetMethod() != null;

            _AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpProperty);
        }

        #endregion

        #region 属性
        public ZLClassInfo ZClass { get; private set; }
        public PropertyInfo MarkProperty { get;private set; }
        public PropertyInfo SharpProperty { get; private set; }
        public bool IsStatic { get { return _IsStatic; } }
        //public bool GetCanRead { get { return _CanRead; } }
        //public bool GetCanWrite { get { return _CanWrite; } }
        public AccessAttrEnum AccessAttr { get { return _AccessAttribute; } }
        public ZLType ZPropertyType 
        {
            get
            {
                if (_ZPropertyType == null)
                { 
                    _ZPropertyType = ZTypeManager.GetBySharpType(MarkProperty.PropertyType);
                }
                return _ZPropertyType;
            }
        }

        public string[] ZPropertyZNames
        {
            get
            {
                if (_ZNames == null)
                {
                    _ZNames = ZClassUtil.GetZNames(MarkProperty);
                }
                return _ZNames;
            }
        }

        #endregion


        public override string ToString()
        {
            var names = this.ZPropertyZNames;
            return this.MarkProperty.Name + "(" + string.Join(",", names) + ")";
        }
    }
}
