using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class ClassAST : TypeAST
    {
        public SectionClassName ClassNameSection { get; protected set; }
        public SectionExtendsClass Extends;
        public SectionPropertiesClass Properties;
        public ContextClass ClassContext { get; private set; }
        public List<ProcConstructorBase> Constructors;
        public List<ProcMethod> Methods;

        public ClassAST(FileAST astFile, SectionNameRaw nameSection, SectionExtendsRaw extendsSection
            , SectionPropertiesRaw propertiesSection)
            : base(astFile)
        {
            this.ClassContext = new ContextClass(this.FileContext);
            ClassNameSection = new SectionClassName(nameSection);
            Constructors = new List<ProcConstructorBase>();
            Methods = new List<ProcMethod>();

           
            Extends = new SectionExtendsClass(this, extendsSection);
            
            if (propertiesSection != null)
            {
                Properties = new SectionPropertiesClass(this, propertiesSection);
            }
        }

        public override void AnalyNameSection()
        {
            ClassNameSection.Analy(this);
        }

        public override string GetTypeName()
        {
            return ClassNameSection.TypeName;
        }

        public void AnalyExtends()
        {
            Extends.Analy();
        }

        public void AnalyPropertiesName()
        {
            if (Properties != null)
            Properties.AnalyNames();
        }

        public void EmitPropertiesName()
        {
            if (Properties != null)
            Properties.EmitNames();
        }

        public void AnalyPropertiesType()
        {
            if (Properties != null)
            {
                Properties.AnalyTypes();
            }
        }

        public void AnalyMethodsName()
        {
            if (Constructors.Count == 0)
            {
                ProcConstructorDefault pcd = new ProcConstructorDefault(this);
                Constructors.Add(pcd);
            }

            foreach (ProcMethod method in Methods)
            {
                method.AnalyName();
            }
        }

        public void AnalyMethodsType()
        {
            foreach (ProcMethod method in Methods)
            {
                method.AnalyType();
            }
        }

        public void EmitMethodsName()
        {
            foreach (ProcConstructorBase procConstructor in Constructors)
            {
                procConstructor.EmitName();
            }

            foreach (ProcMethod method in Methods)
            {
                method.EmitName();
            }
        }

        public void EmitMemberBody()
        {
            if (Properties != null)
            {
                Properties.EmitBodys();
            }

            foreach (ProcConstructorBase procConstructor in Constructors)
            {
                procConstructor.EmitBody();
            }

            foreach (ProcMethod method in Methods)
            {
                method.EmitBody();
            }
        }

        public void EmitTypeName()
        {
            if (HasError())
            {
                return;
            }
            else
            {
                ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;
                var fullName = GetTypeFullName();
                bool IsStatic = this.ClassContext.IsStatic();
                TypeAttributes typeAttrs = TypeAttributes.Public;
                if (IsStatic)
                    typeAttrs = TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed;
                var parentZType = this.ClassContext.GetSuperZType();
                TypeBuilder classBuilder = null;
                if (parentZType != null)
                {
                    classBuilder = moduleBuilder.DefineType(fullName, typeAttrs, parentZType.SharpType);
                }
                else
                {
                    classBuilder = moduleBuilder.DefineType(fullName, typeAttrs);
                }

                ASTUtil.SetZAttrClass(classBuilder, IsStatic);
                this.ClassContext.SetTypeBuilder(classBuilder);
            }
        }

        public void AnalyBody()
        {
            if (Properties!=null)
            {
                Properties.AnalyBodys();
            }

            foreach (ProcConstructorBase item in Constructors)
            {
                item.AnalyBody();
            }
            foreach (ProcMethod item in Methods)
            {
                item.AnalyBody();
            }
        }

        public void AnalyExpDim()
        {
            if (Properties != null)
                Properties.AnalyExpDim();

            foreach (ProcConstructorBase item in Constructors)
            {
                item.AnalyExpDim();
            }
            foreach (ProcMethod item in Methods)
            {
                item.AnalyExpDim();
            }
        }

        //public void EmitBody()
        //{
        //    if (Properties != null)
        //    {
        //        Properties.EmitBodys();
        //    }
        //    foreach (ProcConstructorBase item in Constructors)
        //    {
        //        item.EmitBody();
        //    }
        //    foreach (ProcMethod item in Methods)
        //    {
        //        item.EmitBody();
        //    }
        //}

        public ZLClassInfo CreateZType()
        {
            if (!HasError())
            {
                Type type = this.ClassContext.GetTypeBuilder().CreateType();
                var ztype = ZTypeManager.GetByMarkType(type);
                ZLClassInfo zclasstype = (ZLClassInfo)ztype;
                this.FileContext.EmitedIZDescType = zclasstype;
                return zclasstype;
            }
            return null;
        }

        //private void EmitName()
        //{
        //    //string ClassName = this.NameSection.TypeName;
        //    //if (string.IsNullOrEmpty(ClassName)) throw new CCException();
        //    bool IsStatic = this.ClassContext.IsStatic();
        //    ZLClassInfo BaseZType = this.ClassContext.GetSuperZType();
        //    string packageName = this.FileContext.ProjectContext.PackageName;
        //    ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;

        //    var fullName = this.GetTypeFullName();// packageName + "." + ClassName;
        //    TypeAttributes typeAttrs = TypeAttributes.Public;
        //    if (IsStatic)
        //        typeAttrs = TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed;

        //    TypeBuilder typeBuilder;
        //    if (BaseZType != null)
        //    {
        //        typeBuilder = moduleBuilder.DefineType(fullName, typeAttrs, BaseZType.SharpType);
        //    }
        //    else
        //    {
        //        typeBuilder = moduleBuilder.DefineType(fullName, typeAttrs);
        //    }
        //    this.ClassContext.SetTypeBuilder(typeBuilder);
        //    ASTUtil.SetZAttrClass(typeBuilder, IsStatic);
        //}
    }
}
