using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class DimAST : TypeAST
    {
        public SectionDimName DimNameSection { get; protected set; }
        public SectionExtendsDim Extends;
        public SectionPropertiesDim Properties;

        public DimAST(FileAST astFile, SectionNameRaw nameSection, SectionExtendsRaw extendsSection, SectionPropertiesRaw propertiesSection)
            : base(astFile)//, nameSection)
        {
            DimNameSection = new SectionDimName(nameSection);
            Extends = new SectionExtendsDim(extendsSection);
            Properties = new SectionPropertiesDim(this,propertiesSection);
        }

        public override void AnalyNameSection()
        {
            DimNameSection.Analy(this);
        }

        public override string GetTypeName()
        {
            return DimNameSection.TypeName;
        }

        //public ZLDimInfo Compile()
        //{
        //    Analy();
        //    var result = Emit();
        //    return result;
        //}

        public void AnalyExtends()
        {

        }

        //public void AnalyTypeName()
        //{
        //    AnalyNameSection();
        //}

        //public void Analy()
        //{
        //    //AnalyNameSection();
        //    Properties.Analy();
        //}

        public void AnalyPropertiesName()
        {
            Properties.AnalyNames();
        }

        public void AnalyPropertiesType()
        {
            Properties.AnalyTypes();
        }

        public void EmitPropertiesName()
        {
            Properties.EmitNames();
        }

        public void EmitPropertiesBody()
        {
            Properties.EmitBodys();
        }

        public TypeBuilder DimBuilder { get; private set; }

        public ZLDimInfo CreateZType()
        {
            Type type = DimBuilder.CreateType();
            var EmitedZType = ZTypeManager.CreateZLDimImp(type);
            return EmitedZType;
        }

        //public ZLDimInfo Emit()
        //{
        //    if (HasError())
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        string packageName = this.FileContext.ProjectContext.PackageName;
        //        ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;
        //        var fullName = packageName + "." + NameSection.TypeName;

        //        TypeAttributes typeAttrs = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit;
        //        DimBuilder = moduleBuilder.DefineType(fullName, typeAttrs);
        //        ASTUtil.SetZAttrDim(DimBuilder);
        //        Properties.Emit(DimBuilder);
        //        Type type = DimBuilder.CreateType();
        //        var EmitedZType = ZTypeManager.CreateZLDimImp(type);
        //        return EmitedZType;
        //    }
        //}

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
                TypeAttributes typeAttrs = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit;
                DimBuilder = moduleBuilder.DefineType(fullName, typeAttrs);
                ASTUtil.SetZAttrDim(DimBuilder);
                return;
            }
        }    
    }
}