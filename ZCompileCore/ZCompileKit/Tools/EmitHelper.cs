using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace ZCompileKit.Tools
{
    public static class EmitHelper
    {
        public static void TypeOf(ILGenerator il, Type type)
        {
            il.Emit(OpCodes.Ldtoken, type);
            il.Emit(OpCodes.Call, typeof(System.Type).GetMethod("GetTypeFromHandle"));
        }

        public static void EmitThis(ILGenerator il, bool isStatic)
        {
            if (!isStatic)
            {
                il.Emit(OpCodes.Ldarg_0); // this
            }
        }

        public static void Inc(ILGenerator il, LocalBuilder local)
        {
            EmitHelper.LoadVar(il, local);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            EmitHelper.StormVar(il, local);
        }

        public static void LoadString(ILGenerator il, string str)
        {
            il.Emit(OpCodes.Ldstr, str);         
        }

        public static void LoadInt(ILGenerator il, int value)
        {
            if (value == 0)
            {
                il.Emit(OpCodes.Ldc_I4_0);
            }
            else if (value == 1)
            {
                il.Emit(OpCodes.Ldc_I4_1);
            }
            else if (value == 2)
            {
                il.Emit(OpCodes.Ldc_I4_2);
            }
            else if (value == 3)
            {
                il.Emit(OpCodes.Ldc_I4_3);
            }
            else if (value == 4)
            {
                il.Emit(OpCodes.Ldc_I4_4);
            }
            else if (value == 5)
            {
                il.Emit(OpCodes.Ldc_I4_5);
            }
            else if (value == 6)
            {
                il.Emit(OpCodes.Ldc_I4_6);
            }
            else if (value == 7)
            {
                il.Emit(OpCodes.Ldc_I4_7);
            }
            else if (value == 8)
            {
                il.Emit(OpCodes.Ldc_I4_8);
            }
            else if (value == -1)
            {
                il.Emit(OpCodes.Ldc_I4_M1);
            }
            else if ( value>=-127 && value <= 128 )
            {
                il.Emit(OpCodes.Ldc_I4_S,value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4,value);
            }
        }

        public static void SetLocalArrayElementValue(ILGenerator il,LocalBuilder arrayLocal,int index,Action emitValue )
        {
            EmitHelper.LoadVar(il, arrayLocal);
            EmitHelper.LoadInt(il, index);
            emitValue();
            il.Emit(OpCodes.Stelem_Ref);
        }

        public static void NewArray(ILGenerator il, int length,Type type/*,LocalBuilder arrayLocal*/)
        {
            LoadInt(il,length);
            il.Emit(OpCodes.Newarr, type);
            //EmitHelper.SaveVar(il, arrayLocal);
        }

        public static void LoadArg(ILGenerator il, int argIndex)
        {
            il.Emit(OpCodes.Ldarg, argIndex);
        }

        public static void StormArg(ILGenerator il, int argIndex)
        {
            il.Emit(OpCodes.Starg, argIndex);
        }

        public static void StormVar(ILGenerator il, LocalBuilder local)
        {
            il.Emit(OpCodes.Stloc, local);
        }

        public static void LoadVar(ILGenerator il, LocalBuilder local)
        {
            //il.Emit(OpCodes.Ldloca_S, local);
            il.Emit(OpCodes.Ldloc, local);
        }

        public static void EmitBool(ILGenerator il, bool b)
        {
            if(b)
                il.Emit(OpCodes.Ldc_I4_1);
            else
                il.Emit(OpCodes.Ldc_I4_0);
        }

        public static void StormField(ILGenerator il, FieldInfo field)
        {
            if (field.IsStatic)
            {
                il.Emit(OpCodes.Stsfld, field);
            }
            else
            {
                il.Emit(OpCodes.Stfld, field);
            }
        }

        public static void LoadField(ILGenerator il, FieldInfo field)
        {
            if(field.IsLiteral)//针对const字段
            {
                object value = field.GetValue(null);
                if(value is int)
                {
                    EmitHelper.LoadInt(il, (int)value);
                }
                else if (value is float)
                {
                    il.Emit(OpCodes.Ldc_R4, (float)value);
                }
                else if (value is string)
                {
                    il.Emit(OpCodes.Ldstr, (string)value);
                }
                else if (value is bool)
                {
                    bool bv = (bool)value;
                    if(bv)
                    {
                        il.Emit(OpCodes.Ldc_I4_1);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                    }
                }
                else
                {
                    throw new Exception("编译器不支持" + field.FieldType.Name+"类型");
                }
            }
            else if (field.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, field);
            }
            else
            {
                il.Emit(OpCodes.Ldfld, field);
            }
        }

        public static void NewObj(ILGenerator il, ConstructorInfo newMethod)
        {
            il.Emit(OpCodes.Newobj, newMethod);
        }

        public static void CallPropertyMethod(ILGenerator il, MethodInfo method)
        {
            il.Emit(OpCodes.Call, method);
        }

        public static void Call(ILGenerator il, MethodInfo method)
        {
            il.Emit(OpCodes.Call, method);
        }

        public static void CallDynamic(ILGenerator il, MethodInfo method)
        {
            if (method.IsStatic)
            {
                il.Emit(OpCodes.Call, method);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, method);
            }
        }

        public static void EmitCallBaseConstructorZero(ILGenerator IL,Type type)
        {
            ConstructorInfo zeroConstructor = type.GetConstructor(new Type[] { });
            if (zeroConstructor != null)
            {
                EmitHelper.Emit_LoadThis(IL);
                IL.Emit(OpCodes.Call, zeroConstructor);
            }
        }

        //public static void CallDynamic(ILGenerator il, ZMethodInfo exMethod)
        //{
        //    CallDynamic(il, exMethod.SharpMethod);//, exMethod.IsSelf);
        //}

        //public static void CallDynamic(ILGenerator il, MethodInfo method)
        //{
            //il.Emit(OpCodes.Callvirt, method);
            /*if (method.IsAbstract || method.IsVirtual)
            {
                il.Emit(OpCodes.Callvirt, method);
            }
            else
            {
                il.Emit(OpCodes.Call, method);
            }*/
           /* Console.WriteLine(string.Format("{0} IsStatic:{1}", method.Name, method.IsStatic));
            if (method.IsStatic)
            {
                il.Emit(OpCodes.Call, method);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, method);
            }
        }*/
        /*
        public static void CallDynamic(ILGenerator il, MethodInfo method,Type objType)
        {
            if (objType == method.DeclaringType)
                il.Emit(OpCodes.Call, method);
            else
                il.Emit(OpCodes.Callvirt, method);
        }*/

        public static void Emit_LoadThis(ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg_0);
        }

        public static void Emit_LoadThis(ILGenerator il, bool isStatic)
        {
            if (!isStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
        }

        /*
        public static void Box(ILGenerator il, Type type, Type callArgType)
        {
        //    if (type == typeof(bool) || type == typeof(byte) || type == typeof(char)
        //               || type == typeof(int) || type == typeof(long)
        //               || type == typeof(float) || type == typeof(double)
        //               || type == typeof(decimal)
        //               || type.IsValueType
        //        || type.IsEnum
        //               )
            if (callArgType == typeof(object))
            {
                if (type.IsValueType)
                {
                    il.Emit(OpCodes.Box, type);
                }
            }
        }*/

        public static void EmitConv(ILGenerator il ,Type targetType,Type curType)
        {
            if (targetType == null) return;
            //ILGenerator il = context.ILout;
            if (targetType == typeof(object) && curType.IsValueType)
            {
                il.Emit(OpCodes.Box, curType);
            }
            if (targetType == typeof(float) && curType == typeof(int))
            {
                il.Emit(OpCodes.Conv_R4);
            }
        }
    }
}
