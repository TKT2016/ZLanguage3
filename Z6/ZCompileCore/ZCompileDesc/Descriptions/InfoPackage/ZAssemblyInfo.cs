using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZAssemblyInfo : IZTypeDictionary
    {
        public Assembly ZAssembly { get; protected set; }
        public string Name { get; protected set; }
        private Dictionary<string, ZPackageInfo> _PackageDescTable;
        private Dictionary<string, ZPackageInfo> PackageDescTable { get{
            if(_PackageDescTable==null)
            {
                InitPackageDescTable();
            }
            return _PackageDescTable;
        } }

        public ZAssemblyInfo(string name, Assembly assembly)
        {
            Name = name;
            ZAssembly = assembly;
        }

        protected void InitPackageDescTable()
        {
            _PackageDescTable = new Dictionary<string, ZPackageInfo>();

            var refTypes = ZAssembly.GetTypes();
            foreach (var type in refTypes)
            {
                if (type.IsPublic)
                {
                    string packageName = type.Namespace;
                    packageName = packageName.Replace('.','/');
                    ZPackageInfo zpackage = GetZPackageDesc(packageName);
                    var descType = ZTypeManager.GetByMarkType(type);
                    if (descType != null)
                    {
                        zpackage.AddZDescType(descType);
                    }
                    else
                    {
                        var zdim = ZTypeManager.CreateZLDimImp(type);
                        if (zdim != null)
                        {
                            zpackage.AddZDimType(zdim);
                        }
                    }
                }
            }
        }

        public bool ContainsZType(string zname)
        {
            return ZTypeListHelper.Contains(zname, PackageDescTable.Values.ToArray());
        }

        public ZLType[] SearchZType(string zname)
        {
            return ZTypeListHelper.Search(zname, PackageDescTable.Values.ToArray());
        }

        public List<ZPackageInfo> GetPackageDescs()
        {
            var list = PackageDescTable.Values.OrderBy(p => p.Name).ToList();
            return list;
        }

        public ZPackageInfo SearhcZPackageDesc(string packageName)
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

        protected ZPackageInfo GetZPackageDesc(string packageName)
        {
            if (PackageDescTable.ContainsKey(packageName))
            {
                return PackageDescTable[packageName];
            }
            else
            {
                ZPackageInfo zpackage = new ZPackageInfo(packageName);
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
