using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.CommonCollections;

using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;


namespace ZCompileCore.Contexts
{
    public class ContextClass
    {
        public ContextFile FileContext { get; set; }
        public string ClassName { get { return SelfCompilingType.ZClassName; } }
        public ClassEmitContext EmitContext { get; set; }
        public MethodBuilder InitPropertyMethod { get; set; }
        //public ZCFieldInfo NestedOutFieldSymbol { get; set; }
        public ZCClassInfo SelfCompilingType { get; private set; }
        public bool IsNested { get;protected set; }

        protected ContextClass( )
        {
            EmitContext = new ClassEmitContext();
            SelfCompilingType = new ZCClassInfo();
        }

        public ContextClass(ContextFile fileContext):this()
        {
            FileContext = fileContext;
        }

        public bool ContainsPropertyName(string name)
        {
            //return _properties.ContainsKey(name);
            ZCPropertyInfo zp = SelfCompilingType.SearchDeclaredZProperty(name);
            return zp != null;
        }

        public void SetClassName(string name)
        {
            SelfCompilingType.ZClassName = name;
        }

        public void SetIsStatic(bool isStatic)
        {
            SelfCompilingType.IsStatic = isStatic;
        }

        public void SetSuperClass(ZLClassInfo baseType)
        {
            SelfCompilingType.BaseZClass = baseType;
        }

        public void AddMember(ZCPropertyInfo zcp)
        {
            SelfCompilingType.AddProperty(zcp);
        }

        public string GetClassName()
        {
            return SelfCompilingType.GetZClassName();
        }

        public ZCPropertyInfo SeachZProperty(string name)
        {
            return (ZCPropertyInfo)SelfCompilingType.SearchDeclaredZProperty(name);
        }

        public ZCFieldInfo SeachZField(string name)
        {
            return SelfCompilingType.SearchDeclaredZField(name);
        }

        public ZCClassInfo GetZCompilingType()
        {
            return this.SelfCompilingType;
        }

        public bool IsStatic()
        {
            return this.SelfCompilingType.IsStatic;
        }

        public ZLClassInfo GetSuperZType()
        {
            return this.SelfCompilingType.BaseZClass;
        }

        public ZLMethodInfo[] SearchSuperProc(ZMethodCall procDesc)
        {
            return SelfCompilingType.BaseZClass.SearchZMethod(procDesc);
        }

        public ZCMethodInfo[] SearchThisProc(ZMethodCall procDesc)
        {
            return (ZCMethodInfo[])SelfCompilingType.SearchDeclaredZMethod(procDesc);
        }

        public void SetTypeBuilder(TypeBuilder typeBuilder)
        {
            //this.FileContext.ClassContext.EmitContext.ClassBuilder = typeBuilder;
            if (this.SelfCompilingType.ClassBuilder == null)
                this.SelfCompilingType.ClassBuilder = typeBuilder;
            else
                throw new CCException();
        }

        public TypeBuilder GetTypeBuilder( )
        {
            if (this.SelfCompilingType.ClassBuilder == null)
                throw new CCException();
            return this.SelfCompilingType.ClassBuilder;
        }

        public class ClassEmitContext
        {
            //public TypeBuilder ClassBuilder { get; set; }
            public ConstructorBuilder ZeroConstructor { get;  set; }
            public MethodBuilder InitMemberValueMethod { get;  set; }
            public ISymbolDocumentWriter IDoc { get;  set; }
        }

        public override string ToString()
        {
            return string.Format("ContextClass[{0}]", SelfCompilingType.ZClassName);
        }
    }
}
