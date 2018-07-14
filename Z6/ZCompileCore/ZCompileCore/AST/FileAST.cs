using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class FileAST
    {
        public ContextFile FileContext { get; private set; }
        public SectionImport ImportSection;
        public SectionUse UseSection;

        public List<EnumAST> EnumASTList;
        public List<DimAST> DimASTList;
        public List<ClassAST> ClassASTList;

        public FileAST(ContextFile fileContext)
        {
            FileContext = fileContext;
            EnumASTList = new List<EnumAST>();
            DimASTList = new List<DimAST>();
            ClassASTList = new List<ClassAST>();
        }

        public List<IZObj> Compile()
        {
            AnlayImportUse();
            AnalyTypeName();
            AnalyExtends();
            EmitTypesName();
            AnalyMembersName();
            AnalyMembersType();
            EmitMembersName();
            this.FileContext.ImportUseContext.BuildSegementer(); 
            AnalyMembersBody();

            foreach (ClassAST item in ClassASTList)
            {
                item.AnalyExpDim();
            }
            //EmitMembersBody();
            //List<IZObj> ztypes = CreateZTypes();
            List<IZObj> ztypes = EmitTypes();
            return ztypes;
        }

        private void AnalyMembersBody()
        {
            foreach (EnumAST item in EnumASTList)
            {
                item.AnalyPropertiesType();
            }

            foreach (DimAST item in DimASTList)
            {
                item.AnalyPropertiesType();
            }

            foreach (ClassAST item in ClassASTList)
            {
                item.AnalyBody();
            }
        }

        public void AddTypeAST(TypeAST typeAST)
        {
            if (typeAST is EnumAST)
            {
                EnumASTList.Add(typeAST as EnumAST);
            }
            else if (typeAST is DimAST)
            {
                DimASTList.Add(typeAST as DimAST);
            }
            else if (typeAST is ClassAST)
            {
                ClassASTList.Add((ClassAST)typeAST);
            }
            else
            {
                throw new CCException();
            }
        }

        private void AnlayImportUse()
        {
            if (ImportSection != null)
            {
                ImportSection.Analy();
            }
            if (UseSection != null)
            {
                UseSection.Analy();
            }
        }

        private void AnalyTypeName()
        {
            foreach (var item in EnumASTList)
            {
                item.AnalyTypeName();
            }

            foreach (var item in DimASTList)
            {
                item.AnalyTypeName();
            }

            foreach (var item in ClassASTList)
            {
                item.AnalyTypeName();
            }
        }

        private void AnalyExtends()
        {
            foreach (var item in EnumASTList)
            {
                item.AnalyExtends();
            }

            foreach (var item in DimASTList)
            {
                item.AnalyExtends();
            }

            foreach (var item in ClassASTList)
            {
                item.AnalyExtends();
            }
        }

        private void EmitTypesName()
        {
            foreach (var item in EnumASTList)
            {
                item.EmitTypeName();
            }

            foreach (var item in DimASTList)
            {
                item.EmitTypeName();
            }

            foreach (var item in ClassASTList)
            {
                item.EmitTypeName();
            }
        }

        private void AnalyMembersName()
        {
            foreach (EnumAST item in EnumASTList)
            {
                item.AnalyPropertiesName();
            }

            foreach (DimAST item in DimASTList)
            {
                item.AnalyPropertiesName();
            }

            foreach (ClassAST item in ClassASTList)
            {
                item.AnalyPropertiesName();
            }

            foreach (ClassAST item in ClassASTList)
            {
                item.AnalyMethodsName();
            }
        }

        private void AnalyMembersType()
        {
            foreach (EnumAST item in EnumASTList)
            {
                item.AnalyPropertiesType();
            }

            foreach (DimAST item in DimASTList)
            {
                item.AnalyPropertiesType();
            }

            foreach (ClassAST item in ClassASTList)
            {
                item.AnalyPropertiesType();
                item.AnalyMethodsType();
            }
        }

        private void EmitMembersName()
        {
            if (FileContext.HasError())
            {
                return;
            }
            foreach (EnumAST item in EnumASTList)
            {
                item.EmitPropertiesName();
            }

            foreach (DimAST item in DimASTList)
            {
                item.EmitPropertiesName();
            }

            foreach (ClassAST item in ClassASTList)
            {
                item.EmitPropertiesName();
            }

            foreach (ClassAST item in ClassASTList)
            {
                item.EmitMethodsName();
            }
        }

        private List<IZObj> EmitTypes()
        {
            if(FileContext.HasError() )
            {
                return new List<IZObj>();
            }
            List<IZObj> list = new List<IZObj>();
            foreach (EnumAST item in EnumASTList)
            {
                item.EmitPropertiesBody();
                ZLEnumInfo ze = item.CreateZType();
                if (ze != null)
                {
                    list.Add(ze);
                    this.FileContext.ProjectContext.CompiledTypes.Add(ze);
                }
            }

            foreach (DimAST item in DimASTList)
            {
                item.EmitPropertiesBody();
                ZLDimInfo zd = item.CreateZType();
                if (zd != null)
                {
                    list.Add(zd);
                    this.FileContext.ProjectContext.CompiledTypes.Add(zd);
                }
            }

            foreach (ClassAST item in ClassASTList)
            {
                item.EmitMemberBody();
                ZLClassInfo zc = item.CreateZType();
                if (zc != null)
                {
                    list.Add(zc);
                    this.FileContext.ProjectContext.CompiledTypes.Add(zc); 
                    //this.FileContext.ImportUseContext.ContextFileManager.Import(zc);
                }
            }

            return list;
        }
         
    }
}
