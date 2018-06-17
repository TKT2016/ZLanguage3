using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZDev.RunExe
{
    public class RemoteLoader : MarshalByRefObject
    {
        private Assembly assembly;

        public void LoadAssembly(string fullName)
        {
            assembly = Assembly.LoadFrom(fullName);
        }

        public string FullName
        {
            get { return assembly.FullName; }
        }
    }  
}
