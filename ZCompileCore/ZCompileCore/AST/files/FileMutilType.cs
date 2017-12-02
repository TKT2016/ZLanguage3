using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using ZCompileKit;

namespace ZCompileCore.AST
{
    public class FileMutilType
    {
        public ContextFile FileContext { set; get; }

        public SectionImport ImporteSection = new SectionImport ();
        public SectionUse UseSection = new SectionUse();

        public List<SectionEnum> Enumes= new List<SectionEnum> ();
        public List<SectionDim> Dimes = new List<SectionDim> ();
        public List<SectionClassNameDef> Classes = new List<SectionClassNameDef> ();

        public List<SectionProperties> Propertieses = new List<SectionProperties> ();

        public List<SectionProc> Proces = new List<SectionProc> ();
        public List<SectionConstructorDef> Constructors = new List<SectionConstructorDef>();

        public void AddSection(SectionBase section)
        {
            if(section is SectionImport)
            {
                ImporteSection.Packages.AddRange((section as SectionImport).Packages);
            }
            else if (section is SectionUse)
            {
                UseSection.TypeNameTokens.AddRange((section as SectionUse).TypeNameTokens);
            }
            else if (section is SectionEnum)
            {
                Enumes.Add(section as SectionEnum);
            }
            else if (section is SectionDim)
            {
                Dimes.Add(section as SectionDim);
            }
            else if (section is SectionClassNameDef)
            {
                Classes.Add(section as SectionClassNameDef);
            }
            else if (section is SectionProperties)
            {
                Propertieses.Add(section as SectionProperties);
            }
            else if (section is SectionProc)
            {
                Proces.Add(section as SectionProc);
            }
            else if (section is SectionConstructorDef)
            {
                Constructors.Add(section as SectionConstructorDef);
            }
            else
            {
                throw new CCException();
            }
        }
       

    }
}
