using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;

using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;


namespace ZCompileCore.Contexts
{
    public class ContextClass
    {
        public ContextFile FileContext { get; set; }
        //public ProcContextCollection ProcManagerContext { get; set; }

        public string ClassName { get; set; }
        //public string ExtendsName { get; set; }
        //public ZClassType BaseZType { get; set; }

        public ClassEmitContext EmitContext { get; set; }
        
        public MethodBuilder InitPropertyMethod { get; set; }

        //public ClassSymbolTable Symbols { get{return CurrentTable;} }

        //public SuperSymbolTable SuperTable { get; private set; }
        //public ClassSymbolTable CurrentTable { get; private set; }

        public ZCFieldInfo NestedOutFieldSymbol { get; set; }
        //public string ContextKey { get { return FileContext.ContextKey + "." + (ClassName ?? ""); } }
        //string _KeyContext;

        //public ContextStructText ClassStruct { get; private set; }
        //public ZClassCompilingType ThisCompilingType { get; private set; }
         ZCClassInfo ThisCompilingType;
        public ContextClass(ContextFile fileContext)
        {
            FileContext = fileContext;
            EmitContext = new ClassEmitContext();
            ThisCompilingType = new ZCClassInfo();
            //ProcManagerContext = new ProcContextCollection();
            //ProcManagerContext.ClassContext = this;
            
            //CurrentTable = new ClassSymbolTable("Class");

            //ClassStruct = new ContextStructText();
            //_KeyContext = FileContext.FileModel.GetFileNameNoEx() + "." + (ClassName ?? "()");
        }

        public void AddMember(ZCPropertyInfo zcp)
        {
            //Symbols.Add(symbol);
            //ZMemberCompiling zcp = new ZMemberCompiling(symbol.Name, symbol.SymbolZType,this.IsStaticClass);
            ThisCompilingType.AddProperty(zcp);
        }

        public void AddMethod(ZCMethodInfo zcp)
        {
            ThisCompilingType.AddMethod(zcp);
        }

        Dictionary<string, string> _properties = new Dictionary<string, string>();
        public bool ContainsPropertyName(string name)
        {
            return _properties.ContainsKey(name);
        }

        public void AddPropertyName(string name)
        {
            _properties.Add(name, name);
        }

        public void SetClassName(string name)
        {
            ThisCompilingType.ZClassName = name;//.SetClassName(name);
        }

        public void SetIsStatic(bool isStatic)
        {
            ThisCompilingType.IsStatic = isStatic;//.SetIsStatic(isStatic);
        }

        public void SetSuperClass(ZLClassInfo baseType)
        {
            ThisCompilingType.BaseZClass = baseType;//.SetBaseZType(baseType);
        }

        public string GetClassName()
        {
            return ThisCompilingType.GetZClassName();
        }

        public ZCPropertyInfo SeachZProperty(string name)
        {
            return (ZCPropertyInfo)ThisCompilingType.SearchDeclaredZProperty(name);
        }

        public ZCFieldInfo SeachZField(string name)
        {
            return ThisCompilingType.SearchDeclaredZField(name);
        }

        public ZCClassInfo GetZCompilingType()
        {
            return this.ThisCompilingType;
        }

        public bool IsStatic()
        {
            return this.ThisCompilingType.IsStatic;
        }

        public ZLClassInfo GetSuperZType()
        {
            return this.ThisCompilingType.BaseZClass;
        }

        public ZLMethodInfo[] SearchSuperProc(ZMethodCall procDesc)
        {
            return ThisCompilingType.BaseZClass.SearchZMethod(procDesc);
        }

        public ZCMethodInfo[] SearchThisProc(ZMethodCall procDesc)
        {
            return (ZCMethodInfo[])ThisCompilingType.SearchDeclaredZMethod(procDesc);
        }

        //public bool ContainsProc(ZAMethodDesc zdesc)
        //{
        //    var methods = ThisCompilingType.SearchDeclaredZMethod(zdesc);
        //    return methods.Length > 0;
        //}

        public void SetTypeBuilder(TypeBuilder typeBuilder)
        {
            this.FileContext.ClassContext.EmitContext.ClassBuilder = typeBuilder;
            this.ThisCompilingType.ClassBuilder = typeBuilder;//.SetBuilder(typeBuilder);
        }

        public TypeBuilder GetTypeBuilder( )
        {
            return this.ThisCompilingType.ClassBuilder;
        }

        public class ClassEmitContext
        {
            public TypeBuilder ClassBuilder { get; set; }
            public ConstructorBuilder ZeroConstructor { get;  set; }
            public MethodBuilder InitMemberValueMethod { get;  set; }
            public ISymbolDocumentWriter IDoc { get;  set; }
        }

    }
}
