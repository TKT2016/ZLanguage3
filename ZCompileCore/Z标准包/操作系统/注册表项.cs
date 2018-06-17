using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using ZLangRT.Attributes;
using Z语言系统;

namespace Z标准包.操作系统
{
    [ZInstance]
    public class 注册表项
    {
        RegistryKey keyRoot;
        internal RegistryKey keySub;
        string rootKeyName = "";
        string subKeyName = "";

        public 注册表项(string name)
        {
            string name2 = name.ToUpper();
            if(name2.StartsWith("HKEY_CLASSES_ROOT\\"))
            {
                //keyRoot = Registry.ClassesRoot;
                rootKeyName = "HKEY_CLASSES_ROOT\\";
            }
            else if (name2.StartsWith("HKEY_CURRENT_USER\\"))
            {
                //keyRoot = Registry.CurrentUser;
                rootKeyName = "HKEY_CURRENT_USER\\";
            }
            else if (name2.StartsWith("HKEY_LOCAL_MACHINE\\"))
            {
                //keyRoot = Registry.LocalMachine;
                rootKeyName = "HKEY_LOCAL_MACHINE\\";
            }
            else if (name2.StartsWith("HKEY_USERS\\"))
            {
                //keyRoot = Registry.Users;
                rootKeyName = "HKEY_USERS\\";
            }
            else if (name2.StartsWith("HKEY_CURRENT_CONFIG\\"))
            {
                keyRoot = Registry.CurrentConfig;
                rootKeyName = "HKEY_CURRENT_CONFIG\\";
            }
            else
            {
                throw new Exception("注册表根目录不存在");
            }
            subKeyName = name2.Substring(rootKeyName.Length);
        }

        private void OpenRoot()
        {
            if (keyRoot == null)
            {
                if (rootKeyName.StartsWith("HKEY_CLASSES_ROOT\\"))
                {
                    keyRoot = Registry.ClassesRoot;
                }
                else if (rootKeyName.StartsWith("HKEY_CURRENT_USER\\"))
                {
                    keyRoot = Registry.CurrentUser;
                }
                else if (rootKeyName.StartsWith("HKEY_LOCAL_MACHINE\\"))
                {
                    keyRoot = Registry.LocalMachine;
                }
                else if (rootKeyName.StartsWith("HKEY_USERS\\"))
                {
                    keyRoot = Registry.Users;
                }
                else if (rootKeyName.StartsWith("HKEY_CURRENT_CONFIG\\"))
                {
                    keyRoot = Registry.CurrentConfig;
                }
                else
                {
                    throw new Exception("注册表根目录不存在");
                }
            }
        }

        internal void Create()
        {
            OpenRoot();
            keySub = keyRoot.CreateSubKey(subKeyName);
        }

        internal void Open()
        {
            OpenRoot();
                keySub = keyRoot.OpenSubKey(subKeyName,true);
        }

        internal void Delete()
        {
            OpenRoot();
            keyRoot.DeleteSubKey(subKeyName, true);
            keyRoot.Close();
        }

        internal void Close()
        {
            keyRoot.Close();
        }

        public void WriteSub(string name,string value)
        {
            Open();
            keySub.SetValue(name, value);
            Close();
        }

        public 列表<注册表键> GetSubs()
        {
            Open();
            string[] subkeyNames = keySub.GetSubKeyNames();
            列表<注册表键> list = new 列表<注册表键>();
            foreach (var name in subkeyNames) 
            {
                注册表键 rko = new 注册表键(this, name);
                list.Add(rko);
            }
            return list;
        }

        //public string Read(string name)
        //{
        //    Open();
        //    string value = keySub.GetValue(name).ToString();
        //    Close();
        //    return value;
        //}

        public void DeleteSub(string name)
        {
            Open();
            keySub.DeleteValue(name);
            Close();
        }
    }
}
