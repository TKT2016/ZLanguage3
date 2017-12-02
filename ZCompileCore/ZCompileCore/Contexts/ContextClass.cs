using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;
using ZCompileDesc.Compilings;

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

        public ZMemberCompiling NestedOutFieldSymbol { get; set; }
        //public string ContextKey { get { return FileContext.ContextKey + "." + (ClassName ?? ""); } }
        //string _KeyContext;

        //public ContextStructText ClassStruct { get; private set; }
        //public ZClassCompilingType ThisCompilingType { get; private set; }
         ZClassCompilingType ThisCompilingType;
        public ContextClass(ContextFile fileContext)
        {
            FileContext = fileContext;
            EmitContext = new ClassEmitContext();
            ThisCompilingType = new ZClassCompilingType();
            //ProcManagerContext = new ProcContextCollection();
            //ProcManagerContext.ClassContext = this;
            
            //CurrentTable = new ClassSymbolTable("Class");

            //ClassStruct = new ContextStructText();
            //_KeyContext = FileContext.FileModel.GetFileNameNoEx() + "." + (ClassName ?? "()");
        }

        public void AddMember(ZMemberCompiling zcp)
        {
            //Symbols.Add(symbol);
            //ZMemberCompiling zcp = new ZMemberCompiling(symbol.Name, symbol.SymbolZType,this.IsStaticClass);
            ThisCompilingType.AddProperty(zcp);
        }

        public void AddMethod(ZMethodCompiling zcp)
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
            ThisCompilingType.SetClassName(name);
        }

        public void SetIsStatic(bool isStatic)
        {
            ThisCompilingType.SetIsStatic(isStatic);
        }

        public void SetSuperClass(ZClassType baseType)
        {
            ThisCompilingType.SetBaseZType(baseType);
        }

        public string GetClassName()
        {
            return ThisCompilingType.ZName;
        }

        public ZMemberCompiling SeachZProperty(string name)
        {
            return ThisCompilingType.SeachDefZProperty(name);
        }

        public ZClassCompilingType GetZCompilingType()
        {
            return this.ThisCompilingType;
        }

        public bool IsStatic()
        {
            return this.ThisCompilingType.IsStatic;
        }

        public ZClassType GetSuperZType()
        {
            return this.ThisCompilingType.BaseZType;
        }

        public ZMethodInfo[] SearchSuperProc(ZCallDesc procDesc)
        {
            return ThisCompilingType.SearchSuperZMethod(procDesc);
        }

        public ZMethodCompiling[] SearchThisProc(ZCallDesc procDesc)
        {
            //return ProcManagerContext.SearchProc(procDesc);
            return ThisCompilingType.SearchThisZMethod(procDesc);
        }

        public bool ContainsProc(ZMethodDesc zdesc)
        {
            var methods = ThisCompilingType.SearchThisZMethod(zdesc);
            return methods.Length > 0;

        }

        public void SetTypeBuilder(TypeBuilder typeBuilder)
        {
            this.FileContext.ClassContext.EmitContext.ClassBuilder = typeBuilder;
            this.ThisCompilingType.SetBuilder(typeBuilder);
        }

        public TypeBuilder GetTypeBuilder( )
        {
            return this.ThisCompilingType.ClassBuilder;// (typeBuilder);
            //this.FileContext.ClassContext.EmitContext.ClassBuilder;  
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
