using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ZCompileCore.Contexts;

namespace ZCompileCore.Engines
{
    static class CompileUtil
    {
        public static void DeletePDB(ContextProject projectContext)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string fileNameNoEx = projectContext.ProjectModel.BinaryFileNameNoEx;
            string pdbfile = Path.Combine(folder, fileNameNoEx + ".pdb");
            if (File.Exists(pdbfile))
            {
                File.Delete(pdbfile);
            }
        }

        public static void MoveBinary(ContextProject projectContext)
        {
            string exBinFileName = projectContext.ProjectModel.GetBinaryNameEx();
            string fromFileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exBinFileName);
            string toFileFullPath = Path.Combine(projectContext.ProjectModel.BinarySaveDirectoryInfo.FullName, exBinFileName);
            if (File.Exists(toFileFullPath))
            {
                File.Delete(toFileFullPath);
            }
            if (File.Exists(fromFileFullPath) && fromFileFullPath!= toFileFullPath)
            {
                File.Move(fromFileFullPath, toFileFullPath);
                File.Delete(fromFileFullPath);
            }
            CompileUtil.DeletePDB(projectContext);
        }

        public static void GenerateBinary(ContextProject projectContext)
        {
            string binFileName = projectContext.ProjectModel.GetBinaryNameEx();
            string projectPackageName = projectContext.ProjectModel.ProjectPackageName;

            projectContext.CreateProjectEmitContext(AppDomain.CurrentDomain, projectPackageName, projectPackageName, binFileName);
            setAttr(projectContext.EmitContext.AssemblyBuilder, projectContext);
        }

        private static void setAttr(AssemblyBuilder builder, ContextProject context)
        {
            {
                Type myType = typeof(AssemblyVersionAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { "0.4.0" });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(DebuggableAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyCompanyAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { "Z语言" });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyConfigurationAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { "" });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyCopyrightAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { "Copyright ©  " + DateTime.Now.Year });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyDescriptionAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { "" });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyFileVersionAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { "" });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyProductAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { context.ProjectModel.ProjectPackageName });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyTitleAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { context.ProjectModel.ProjectPackageName });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(AssemblyTrademarkAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { "" });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(CompilationRelaxationsAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(int) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { 8 });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(RuntimeCompatibilityAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(ComVisibleAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(bool) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { false });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(GuidAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { Guid.NewGuid().ToString() });
                builder.SetCustomAttribute(attributeBuilder);
            }
            {
                Type myType = typeof(TargetFrameworkAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { ".NETFramework,Version=v4.0" });
                builder.SetCustomAttribute(attributeBuilder);
            }
        }
    }
}
