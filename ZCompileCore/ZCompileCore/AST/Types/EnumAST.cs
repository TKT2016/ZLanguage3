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
    public class EnumAST : TypeAST
    {
        public SectionEnumName EnumNameSection { get; protected set; }
        public SectionExtendsEnum Extends;
        public SectionPropertiesEnum Properties;

        public EnumAST(FileAST astFile, SectionNameRaw nameSection, SectionExtendsRaw extendsSection, SectionPropertiesRaw propertiesSection)
            : base(astFile)//, nameSection)
        {
            EnumNameSection = new SectionEnumName(nameSection);
            Extends = new SectionExtendsEnum(extendsSection);
            Properties = new SectionPropertiesEnum(this,propertiesSection);
        }

        public override void AnalyNameSection()
        {
            EnumNameSection.Analy(this);
        }

        public override string GetTypeName()
        {
            return EnumNameSection.TypeName;
        }

        public void AnalyExtends()
        {
            
        }

        public void AnalyPropertiesName()
        {
            Properties.AnalyNames();
        }

        public void EmitPropertiesName()
        {
            Properties.EmitNames();
        }

        public void EmitPropertiesBody()
        {
            
        }

        public void AnalyPropertiesType()
        {
            Properties.AnalyTypes();
        }

        public void Analy()
        {
            //AnalyNameSection();
            Properties.Analy();
        }
        public EnumBuilder EnumTypeBuilder { get; private set; }

        public ZLEnumInfo CreateZType()
        {
            var EmitedType = EnumTypeBuilder.CreateType();
            ZLEnumInfo ztype = ZTypeManager.GetByMarkType(EmitedType) as ZLEnumInfo;
            return ztype;
        }

        //public ZLEnumInfo Emit()
        //{
        //    if (HasError())
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        //string packageName = this.FileContext.ProjectContext.PackageName;
        //        ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;
        //        var EnumFullName = GetTypeFullName();// packageName + "." + NameSection.TypeName;
        //        EnumTypeBuilder = moduleBuilder.DefineEnum(EnumFullName, TypeAttributes.Public, typeof(int));
        //        ASTUtil.SetZAttrEnum(EnumTypeBuilder);
        //        Properties.Emit(EnumTypeBuilder);
        //        var EmitedType = EnumTypeBuilder.CreateType();
        //        ZLEnumInfo ztype = ZTypeManager.GetByMarkType(EmitedType) as ZLEnumInfo;
        //        return ztype;
        //        //foreach (SectionEnum enumSection in EnumSections)
        //        //{
        //        //    var builder = this.ProjectContext.EmitContext.ModuleBuilder;
        //        //    var packageName = this.ProjectContext.ProjectModel.ProjectPackageName;
        //        //    var fileName = this.FileContext.FileModel.GeneratedClassName;//.GetFileNameNoEx();
        //        //    ZLEnumInfo ztype = enumSection.Compile(builder, packageName, fileName);
        //        //    return ztype;
        //        //}
        //    }
        //}

        public void EmitTypeName()
        {
            ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;
            var EnumFullName = GetTypeFullName();
            EnumTypeBuilder = moduleBuilder.DefineEnum(EnumFullName, TypeAttributes.Public, typeof(int));
            ASTUtil.SetZAttrEnum(EnumTypeBuilder);
        }
    }
}
