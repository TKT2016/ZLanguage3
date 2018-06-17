using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.ASTRaws;
using ZCompileCore.CommonCollections;
using ZCompileCore.Contexts;

namespace ZCompileCore.Parsers.Asts
{
    public class FileASTParser
    {
        private ArrayTape<SectionRaw> tape;
        private FileAST fileAST;

        public FileAST Parse(FileRaw fileRaw, ContextFile fileContext)
        {
            fileAST = new FileAST(fileContext);
            tape = new ArrayTape<SectionRaw>(fileRaw.Sections);

            while (tape.HasCurrent)
            {
                SectionRaw section = tape.Current;
                if (section is SectionImportRaw)
                {
                    fileAST.ImportSection = new SectionImport(fileAST, section as SectionImportRaw);
                    tape.MoveNext();
                }
                else if (section is SectionUseRaw)
                {
                    //fileAST.UseSection = (section as SectionUse);
                    //fileAST.UseSection.FileContext = fileAST.FileContext;
                    //tape.MoveNext();
                    fileAST.UseSection = new SectionUse(fileAST, section as SectionUseRaw);
                    //fileAST.ImporteSection.FileContext = fileAST.FileContext;
                    tape.MoveNext();
                }
                //else if (section is SectionNameRaw)
                //{
                //    ParseType();
                //}
                else if ((section is SectionExtendsRaw) || (section is SectionPropertiesRaw) || (section is SectionProcRaw) || section is SectionNameRaw)
                {
                    //ParseTypeSection();
                    var tempTypeBody = ParseTempTypeBody();
                    TypeAST tast = tempTypeBody.Parse();
                    fileAST.AddTypeAST(tast);
                }
                else
                {
                    throw new CCException();
                }
            }
            
            return fileAST;
        }

        private TempTypeBody ParseTempTypeBody()
        {
           var tempTypeBody = new TempTypeBody(fileAST);
            while (tape.HasCurrent)
            {
                SectionRaw section = tape.Current;
                if (section is SectionNameRaw)
                {
                    if (tempTypeBody.NameSection == null)
                    {
                        tempTypeBody.NameSection = (section as SectionNameRaw);
                        tape.MoveNext();
                    }
                    else
                    {
                        TypeAST tast = tempTypeBody.Parse();
                        fileAST.AddTypeAST(tast);
                        break;
                    }
                }
                //else if ((section is SectionExtendsRaw) || (section is SectionPropertiesRaw) || (section is SectionProcRaw))
                //{
                //    ParseTypeSection();
                //}
                else if (section is SectionExtendsRaw)
                {
                    tempTypeBody.ExtendsSection = (section as SectionExtendsRaw);
                    tape.MoveNext();
                }
                else if (section is SectionPropertiesRaw)
                {
                    tempTypeBody.PropertiesSection = (section as SectionPropertiesRaw);
                    tape.MoveNext();
                }
                else if (section is SectionProcRaw)
                {
                    tempTypeBody.Proces.Add(section as SectionProcRaw);
                    tape.MoveNext();
                }
                else
                {
                    throw new CCException();
                }
            }
            return tempTypeBody;
        }

        class TempTypeBody
        {
            public SectionNameRaw NameSection;
            public SectionExtendsRaw ExtendsSection;
            public SectionPropertiesRaw PropertiesSection;
            public List<SectionProcRaw> Proces = new List<SectionProcRaw>();
            public FileAST fileAST;

            public TempTypeBody(FileAST fileAST)
            {
                this.fileAST = fileAST;
            }

            public TypeAST Parse()
            {
                string baseType = "普通类型";
                if (ExtendsSection != null && ExtendsSection.BaseTypeToken != null)
                {
                    baseType = ExtendsSection.BaseTypeToken.Text;
                }

                if (baseType == "约定类型")
                {
                    EnumAST enumAST = new EnumAST(fileAST,NameSection, ExtendsSection, PropertiesSection);
                    return enumAST;
                }
                else if (baseType == "声明类型")
                {
                    DimAST dimAST = new DimAST(fileAST, NameSection, ExtendsSection, PropertiesSection);
                    return dimAST;
                }
                else
                {
                    ClassAST classAST = new ClassAST(fileAST, NameSection, ExtendsSection, PropertiesSection);
                    foreach(var item in Proces)
                    {
                        if(item.IsConstructor)
                        {
                            classAST.Constructors.Add(new ProcConstructor(classAST,item));
                        }
                        else
                        {
                            classAST.Methods.Add(new ProcMethod(classAST,item));
                        }
                    }
                    return classAST;
                }
            }
        }
    }
}
