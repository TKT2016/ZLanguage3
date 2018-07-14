using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLFieldInfo : ZAFieldInfo, ICompleted
    {
        #region override

        public override AccessAttrEnum GetAccessAttr() { return this.AccessAttr; }
        public override bool GetIsStatic() { return this.IsStatic; }
        public override bool GetCanRead() { return this.CanRead; }
        public override bool GetCanWrite() { return this.CanWrite; }
        public override string[] GetZFieldZNames() { return this.ZFieldZNames; }
        public override ZType GetZFieldType() { return this.ZFieldType; }
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

        public ZLFieldInfo(ZLClassInfo zClass, FieldInfo propertyInfo)
            : this(zClass,propertyInfo, propertyInfo)
        {
           
        }

        public ZLFieldInfo(ZLClassInfo zClass, FieldInfo markPropertyInfo, FieldInfo sharpPropertyInfo)
        {
            ZClass = zClass;
            MarkField = markPropertyInfo;
            SharpField = sharpPropertyInfo;
            Init();
        }

        private void Init()
        {
            _IsStatic = SharpField.IsStatic;
            _CanRead = true;
            _CanWrite = true;
            _AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpField);
        }

        #endregion

        #region 属性
        public ZLClassInfo ZClass { get; private set; }
        public FieldInfo MarkField { get; private set; }
        public FieldInfo SharpField { get; private set; }
        public bool IsStatic { get { return _IsStatic; } }
        public bool CanRead { get { return _CanRead; } }
        public bool CanWrite { get { return _CanWrite; } }
        public AccessAttrEnum AccessAttr { get { return _AccessAttribute; } }
        public ZLType ZFieldType
        {
            get
            {
                if (_ZPropertyType == null)
                { 
                    _ZPropertyType = ZTypeManager.GetBySharpType(MarkField.FieldType);
                }
                return _ZPropertyType;
            }
        }

        public string[] ZFieldZNames
        {
            get
            {
                if (_ZNames == null)
                {
                    _ZNames = ZClassUtil.GetZNames(MarkField);
                }
                return _ZNames;
            }
        }

        #endregion


        public override string ToString()
        {
            var names = this.ZFieldZNames;
            return this.MarkField.Name + "(" + string.Join(",", names) + ")";
        }
    }
}
