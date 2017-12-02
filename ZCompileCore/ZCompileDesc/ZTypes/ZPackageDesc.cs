using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZLangRT;

namespace ZCompileDesc.ZTypes
{
    public class ZPackageDesc :IZTypeDictionary// IWordDictionary, , IZDescTypeDictionary
    {
        public string Name { get; set; }
        public List<ZDimType> DimTypes { get; protected set; }
        public List<ZEnumType> EnumTypes { get; protected set; }
        public List<ZClassType> ClassTypes { get; protected set; }

        public ZPackageDesc(string name)
        {
            Name = name;
            Init();
        }

        protected void Init()
        {
            DimTypes = new List<ZDimType>();
            EnumTypes = new List<ZEnumType>();
            ClassTypes = new List<ZClassType>();
        }

        public bool ContainsZDescType(string zname)
        {
            return ContainsZType(zname) 
                 || ZDescTypeListHelper.Contains(zname, DimTypes.ToArray())
                ;
        }

        public IZDescType[] SearchZDescType(string zname)
        {
            List<IZDescType> ztypes = new List<IZDescType>();
            ztypes.AddRange(SearchZType(zname));
            ztypes.AddRange(ZDescTypeListHelper.Search(zname, ClassTypes.ToArray()));
            return ztypes.ToArray();
        }

        public bool ContainsZType(string zname)
        {
            return ZTypeListHelper.Contains(zname, EnumTypes.ToArray()) || ZTypeListHelper.Contains(zname,ClassTypes.ToArray());
        }

        public ZType SearchZTypeOne(string zname)
        {
            ZType ztype = null;
            ztype = EnumerableHelper.SearchOne<ZEnumType>(EnumTypes, (ZEnumType zenum) => zenum.ZName == zname);
            if (ztype == null)
            {
                ztype = EnumerableHelper.SearchOne<ZClassType>(ClassTypes, (ZClassType zclass) => zclass.ZName == zname);
            }
            return ztype;
        }

        public ZType[] SearchZType(string zname)
        {
            ZType ztype = SearchZTypeOne( zname);
            if (ztype == null)
            {
                return new ZType[] { };
            }
            else
            {
                return new ZType[] { ztype };
            }
        }

        //public bool ContainsWord(string text)
        //{
        //    return
        //        EnumerableHelper.AnyOne <ZEnumType>(EnumTypes,(ZEnumType zenum)=>zenum.ZName== text)
        //        || EnumerableHelper.AnyOne<ZClassType>(ClassTypes, (ZClassType zclass) => zclass.ZName == text) 
        //        ||IWordDictionaryHelper.ArrayContainsWord(text, EnumTypes.ToArray()) 
        //        || IWordDictionaryHelper.ArrayContainsWord(text, ClassTypes.ToArray()); // this.WordDictionaryList.ContainsWord(text);
        //}

        //public WordInfo SearchWord(string text)
        //{
        //    if (!ContainsWord(text)) return null;
        //    ZType ztype = SearchZTypeOne(text);
        //    WordInfo info0 = null;
        //    if(ztype!=null)
        //    {
        //        info0 = new WordInfo(text, WordKind.TypeName, ztype);
        //    }
        //    WordInfo info1 = IWordDictionaryHelper.EnumerableSearchWord(text, EnumTypes);
        //    WordInfo info2 = IWordDictionaryHelper.EnumerableSearchWord(text, ClassTypes);
        //    WordInfo newWord = WordInfo.Merge(info0,info1, info2);
        //    return newWord;
        //}

        public void AddZDescType(IZDescType descType)
        {
            if(descType is ZDimType)
            {
                DimTypes.Add(descType as ZDimType);
            }
            else if (descType is ZEnumType)
            {
                ZEnumType zenum = descType as ZEnumType;
                EnumTypes.Add(zenum);
            }
            else if (descType is ZClassType)
            {
                ZClassType zclass = descType as ZClassType;
                ClassTypes.Add(zclass);
            }
            else
            {
                throw new ZyyRTException();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}(dim[{1},enum:{2},class:{3})", Name, this.DimTypes.Count, this.EnumTypes.Count, this.ClassTypes.Count);
        }
    }
}
