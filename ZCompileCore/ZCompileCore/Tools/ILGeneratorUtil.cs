using System;
using System.Reflection.Emit;

namespace ZCompileCore.Tools
{
    public static class ILGeneratorUtil
    {
        public static void LoadInt(ILGenerator ilGenerator, int value)
        {
            switch (value)
            {
                case -1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    ilGenerator.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    ilGenerator.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    ilGenerator.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    ilGenerator.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    ilGenerator.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    ilGenerator.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    ilGenerator.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    ilGenerator.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    ilGenerator.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                ilGenerator.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldc_I4, value);
            }
        }

        public static void LoadLocal(ILGenerator ilGenerator, LocalBuilder localBuilder)
        {
            LoadLocal(ilGenerator,localBuilder.LocalIndex);
        }

        public static void LoadLocal(ILGenerator ilGenerator, int localIndex)
        {
            switch (localIndex)
            {
                case 0:
                    ilGenerator.Emit(OpCodes.Ldloc_0);
                    return;
                case 1:
                    ilGenerator.Emit(OpCodes.Ldloc_1);
                    return;
                case 2:
                    ilGenerator.Emit(OpCodes.Ldloc_2);
                    return;
                case 3:
                    ilGenerator.Emit(OpCodes.Ldloc_3);
                    return;
            }
            if (localIndex > 0 && localIndex <= 255)
            {
                ilGenerator.Emit(OpCodes.Ldloc_S, localIndex);
                return;
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldloc, localIndex);
                return;
            }
        }

        public static void StormLocal(ILGenerator ilGenerator, LocalBuilder localBuilder)
        {
            StormLocal(ilGenerator, localBuilder.LocalIndex);
        }

        public static void StormLocal(ILGenerator ilGenerator, int localIndex )
        {
            switch (localIndex)
            {
                case 0:
                    ilGenerator.Emit(OpCodes.Stloc_0);
                    return;
                case 1:
                    ilGenerator.Emit(OpCodes.Stloc_1);
                    return;
                case 2:
                    ilGenerator.Emit(OpCodes.Stloc_2);
                    return;
                case 3:
                    ilGenerator.Emit(OpCodes.Stloc_3);
                    return;
            }
            if (localIndex > 0 && localIndex <= 255)
            {
                ilGenerator.Emit(OpCodes.Stloc_S, localIndex);
                return;
            }
            else
            {
                ilGenerator.Emit(OpCodes.Stloc, localIndex);
                return;
            }
        }

        public static void LoadArg(ILGenerator ilGenerator, int argIndex)
        {
            switch (argIndex)
            {
                case 0:
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    return;
                case 1:
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    return;
                case 2:
                    ilGenerator.Emit(OpCodes.Ldarg_2);
                    return;
                case 3:
                    ilGenerator.Emit(OpCodes.Ldarg_3);
                    return;
            }
            if (argIndex > 0 && argIndex <= 255)
            {
                ilGenerator.Emit(OpCodes.Ldarg_S, argIndex);
                return;
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldarg, argIndex);
                return;
            }
        }

        public static void StormArg(ILGenerator ilGenerator, int argIndex)
        {
            if (argIndex > 0 && argIndex <= 255)
            {
                ilGenerator.Emit(OpCodes.Starg_S, argIndex);
                return;
            }
            else
            {
                ilGenerator.Emit(OpCodes.Starg, argIndex);
                return;
            }
        }
    }
}
