using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using ZLangRT.Attributes;

namespace TKT.CLRTest.S4
{
    [ZStatic]
    public static class 注册表操作Test
    {
        public static RegistryKey RootKey = Registry.ClassesRoot;
        public static void CreateSubKey(string subKey)
        {
            //RegistryKey key = Registry.ClassesRoot;
            RegistryKey software = RootKey.CreateSubKey(subKey);
        }

        public static void OpenSubKey(string subKey)
        {
            //RegistryKey key = Registry.LocalMachine;
            RegistryKey software = RootKey.OpenSubKey(subKey, true);
        }

        public static void DeleteSubKey(string subKey)
        {
            //RegistryKey key = Registry.LocalMachine;
            RootKey.DeleteSubKey(subKey, true); //该方法无返回值，直接调用即可
            RootKey.Close();
        }

        public static void WriteSubKeyValue(string subKey, string keyName, string keyValue)
        {
            //RegistryKey key = Registry.LocalMachine;
            RegistryKey software = RootKey.OpenSubKey(subKey, true); //该项必须已存在
            software.SetValue(keyName, keyValue);
            //在HKEY_LOCAL_MACHINE\SOFTWARE\test下创建一个名为“test”，值为“  ddd”的键值。如果该键值原本已经存在，则会修改替换原来的键值，如果不存在则是创建该键值。
            // 注意：SetValue()还有第三个参数，主要是用于设置键值的类型，如：字符串，二进制，Dword等等~~默认是字符串。如：
            // software.SetValue("test", "0", RegistryValueKind.DWord); //二进制信息
            RootKey.Close();
        }

        public static string ReadSubKeyValue(string subKey, string keyName)
        {
            string info = "";
            //RegistryKey Key  = Registry.LocalMachine;
            RegistryKey myreg = RootKey.OpenSubKey(subKey);
            // myreg = Key.OpenSubKey("software\\test",true);
            info = myreg.GetValue(keyName).ToString();
            myreg.Close();
            return info;
        }

        public static void DeleteSubKeyValue(string subKey, string keyName)
        {
            RegistryKey delKey = RootKey.OpenSubKey(subKey, true);
            delKey.DeleteValue(keyName);
            delKey.Close();
        }

        //判断注册表项是否存在
        public static bool IsRegeditItemExist(string subKey, string keyName)
        {
            string[] subkeyNames;
            //RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = RootKey.OpenSubKey(subKey);
            //RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);  
            subkeyNames = software.GetSubKeyNames();
            //取得该项下所有子项的名称的序列，并传递给预定的数组中  
            foreach (string item in subkeyNames)
            {
                if (item == keyName)
                //判断子项的名称  
                {
                    RootKey.Close();
                    return true;
                }
            }
            RootKey.Close();
            return false;
        }

        //判断键值是否存在
        private static bool IsRegeditKeyExit(string subKey, string keyName)
        {
            string[] subkeyNames;
            //RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = RootKey.OpenSubKey(subKey);
            //RegistryKey software = hkml.OpenSubKey("SOFTWARE\\test", true);
            subkeyNames = software.GetValueNames();
            //取得该项下所有键值的名称的序列，并传递给预定的数组中
            foreach (string item in subkeyNames)
            {
                if (item == keyName) //判断键值的名称
                {
                    RootKey.Close();
                    return true;
                }

            }
            RootKey.Close();
            return false;
        }
    }
}
