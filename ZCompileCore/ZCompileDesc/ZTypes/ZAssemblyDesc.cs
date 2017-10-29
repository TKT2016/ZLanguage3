using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc.ZTypes
{
    public class ZAssemblyDesc : IWordDictionary, IZTypeDictionary
    {
        public Assembly ZAssembly { get; protected set; }
        public string Name { get; protected set; }
        private Dictionary<string, ZPackageDesc> _PackageDescTable;
        private Dictionary<string, ZPackageDesc> PackageDescTable { get{
            if(_PackageDescTable==null)
            {
                InitPackageDescTable();
            }
            return _PackageDescTable;
        } }

        public ZAssemblyDesc(string name, Assembly assembly)
        {
            Name = name;
            ZAssembly = assembly;
        }

        protected void InitPackageDescTable()
        {
            _PackageDescTable = new Dictionary<string, ZPackageDesc>();

            var refTypes = ZAssembly.GetTypes();
            foreach (var type in refTypes)
            {
                if (type.IsPublic)
                {
                    string packageName = type.Namespace;
                    packageName = packageName.Replace('.','/');
                    ZPackageDesc zpackage = GetZPackageDesc(packageName);

                    IZDescType descType = ZTypeManager.GetByMarkType(type);
                    if (descType != null)
                    {
                        zpackage.AddZDescType(descType);
                    }
                }
            }
        }

        public bool ContainsZType(string zname)
        {
            return ZTypeListHelper.Contains(zname, PackageDescTable.Values.ToArray());
        }

        public ZType[] SearchZType(string zname)
        {
            return ZTypeListHelper.Search(zname, PackageDescTable.Values.ToArray());
        }

        public bool ContainsWord(string text)
        {
            return IWordDictionaryHelper.EnumerableContainsWord(text, PackageDescTable.Values);
        }

        public WordInfo SearchWord(string text)
        {
            WordInfo info1 = IWordDictionaryHelper.EnumerableSearchWord(text, PackageDescTable.Values);
            return info1;
        }

        public List<ZPackageDesc> GetPackageDescs()
        {
            var list = PackageDescTable.Values.OrderBy(p => p.Name).ToList();
            return list;
        }

        public ZPackageDesc SearhcZPackageDesc(string packageName)
        {
            if (PackageDescTable.ContainsKey(packageName))
            {
                return PackageDescTable[packageName];
            }
            else
            {
                return null;
            }
        }

        protected ZPackageDesc GetZPackageDesc(string packageName)
        {
            if (PackageDescTable.ContainsKey(packageName))
            {
                return PackageDescTable[packageName];
            }
            else
            {
                ZPackageDesc zpackage = new ZPackageDesc(packageName);
                PackageDescTable.Add(packageName, zpackage);
                return zpackage;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}(package{1},enum:{2},class:{3},word:{4}])", Name, string.Join(",",PackageDescTable.Keys));
        }

       
    }
}
