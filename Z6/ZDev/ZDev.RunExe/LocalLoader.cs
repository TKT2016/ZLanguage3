using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZDev.RunExe
{
    public class LocalLoader
    {
        private AppDomain appDomain;
        private RemoteLoader remoteLoader;

        public LocalLoader(AppDomain appDomain)
        {
            this.appDomain = appDomain;
            string name = Assembly.GetExecutingAssembly().GetName().FullName;
            remoteLoader = (RemoteLoader)appDomain.CreateInstanceAndUnwrap(
                name,
                typeof(RemoteLoader).FullName);
        }

        public void LoadAssembly(string fullName)
        {
            remoteLoader.LoadAssembly(fullName);
        }

        public void Unload()
        {
            AppDomain.Unload(appDomain);
            appDomain = null;
        }

        public string FullName
        {
            get
            {
                return remoteLoader.FullName;
            }
        }
    }  
}
