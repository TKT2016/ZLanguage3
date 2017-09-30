using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TKT.CLRTest
{
    public class ILDemo
    {
        public static void CreateDemo()
        {
            string name = "NameSpaceDemo.CreateClass";
            string FileName = name + ".dll";

            #region step1 创建程序集
            AssemblyName MyAssemblyName = new AssemblyName(name);
            AppDomain MyAppDomain = AppDomain.CurrentDomain;
            AssemblyBuilder MyAssemblyBuilder = MyAppDomain.DefineDynamicAssembly(MyAssemblyName, AssemblyBuilderAccess.RunAndSave);
            #endregion

            #region step2 定义模块
            ModuleBuilder MyModule = MyAssemblyBuilder.DefineDynamicModule(name, FileName);
            #endregion

            #region step3 定义类型
            TypeBuilder MyType = MyModule.DefineType(name, TypeAttributes.Public);
            #endregion

            #region step4 第一属性 name
            FieldBuilder FieldName = MyType.DefineField("name", typeof(string), FieldAttributes.Private);
            FieldBuilder FieldAge = MyType.DefineField("age", typeof(int), FieldAttributes.Private);
            FieldName.SetConstant("0");
            FieldAge.SetConstant(0);
            #endregion

            #region step5 定义属性
            PropertyBuilder PropertyName = MyType.DefineProperty("Name", PropertyAttributes.None, typeof(string), null);
            PropertyBuilder PropertyAge = MyType.DefineProperty("Age", PropertyAttributes.None, typeof(int), null);
            #endregion

            #region step 6 定义name get 和set
            MethodBuilder NameGet = MyType.DefineMethod("get", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(string), Type.EmptyTypes);
            ILGenerator ILGNameGet = NameGet.GetILGenerator();
            ILGNameGet.Emit(OpCodes.Ldarg_0);
            ILGNameGet.Emit(OpCodes.Ldfld, FieldName);
            ILGNameGet.Emit(OpCodes.Ret);

            MethodBuilder NameSet = MyType.DefineMethod("set",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null, new Type[] { typeof(string) });
            ILGenerator ILGNameSet = NameSet.GetILGenerator();
            ILGNameSet.Emit(OpCodes.Ldarg_0);
            ILGNameSet.Emit(OpCodes.Ldarg_1);
            ILGNameSet.Emit(OpCodes.Stfld, FieldName);
            ILGNameSet.Emit(OpCodes.Ret);

            PropertyName.SetGetMethod(NameGet);
            PropertyName.SetSetMethod(NameSet);
            #endregion

            #region step7 定义age get和set

            MethodBuilder AgeGet = MyType.DefineMethod("get", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(int), null);
            ILGenerator ILGAgeGet = AgeGet.GetILGenerator();
            ILGAgeGet.Emit(OpCodes.Ldarg_0);
            ILGAgeGet.Emit(OpCodes.Ldfld, FieldAge);
            ILGAgeGet.Emit(OpCodes.Ret);

            MethodBuilder AgeSet = MyType.DefineMethod("set",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null, new Type[] { typeof(int) });
            ILGenerator IGLAgeSet = AgeSet.GetILGenerator();
            IGLAgeSet.Emit(OpCodes.Ldarg_0);
            IGLAgeSet.Emit(OpCodes.Ldarg_1);
            IGLAgeSet.Emit(OpCodes.Stfld, FieldAge);
            IGLAgeSet.Emit(OpCodes.Ret);

            PropertyAge.SetGetMethod(AgeGet);
            PropertyAge.SetSetMethod(AgeSet);
            #endregion

            #region step8 定义构造函数

            ConstructorBuilder CreateClass = MyType.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[] { typeof(string), typeof(int) });

            ILGenerator ILGCreateClass = CreateClass.GetILGenerator();
            ILGCreateClass.Emit(OpCodes.Ldarg_0);
            ILGCreateClass.Emit(OpCodes.Ldarg_1);
            ILGCreateClass.Emit(OpCodes.Stfld, FieldName);
            ILGCreateClass.Emit(OpCodes.Ldarg_0);
            ILGCreateClass.Emit(OpCodes.Ldarg_2);
            ILGCreateClass.Emit(OpCodes.Stfld, FieldAge);
            ILGCreateClass.Emit(OpCodes.Ret);
            #endregion

            MethodBuilder Export = MyType.DefineMethod("Export", MethodAttributes.Public);
            ILGenerator ILGExport = Export.GetILGenerator();
            ILGExport.Emit(OpCodes.Nop);
            ILGExport.Emit(OpCodes.Ldstr, "姓名是{0} 年龄是{1}");
            ILGExport.Emit(OpCodes.Ldarg_0);
            ILGExport.Emit(OpCodes.Call, NameGet);
            ILGExport.Emit(OpCodes.Ldarg_0);
            ILGExport.Emit(OpCodes.Call, AgeGet);
            ILGExport.Emit(OpCodes.Box, typeof(int));
            ILGExport.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string), typeof(object), typeof(object) }));
            ILGExport.Emit(OpCodes.Ret);

            MyType.CreateType();
            MyAssemblyBuilder.Save(FileName);
        }
    }
}
