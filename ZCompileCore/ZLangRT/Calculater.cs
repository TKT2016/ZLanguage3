﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLangRT
{
    public static class Calculater
    {
        public static T Cast<T>(object obj)
        {
            T t = (T)obj;
            return t;

            //if(typeof(T)== typeof(int))
            //{
            //    if (obj.GetType() == typeof(int))
            //    {
            //        return (T)((int)(obj));
            //    }
            //    else if (obj.GetType() == typeof(float))
            //    {

            //    }
            //    else
            //    {
            //        throw new InvalidCastException();
            //    }
            //}
            //else if (typeof(T) == typeof(float))
            //{

            //}
            //else
            //{
            //    T t = (T)obj;
            //    return t;
            //}
        }

        public static int AddInt(int a, int b)
        {
            return a + b;
        }

        public static float AddFloat(float a, float b)
        {
            return a + b;
        }

        public static int SubInt(int a, int b)
        {
            return a - b;
        }

        public static float SubFloat(float a, float b)
        {
            return a + b;
        }

        public static int MulInt(int a, int b)
        {
            return a * b;
        }

        public static float MulFloat(float a, float b)
        {
            return a * b;
        }

        public static float DivFloat(float a, float b)
        {
            return a / b;
        }


        public static float DivInt(int a, int b)
        {
            float fa =(float)a;
            float fb = (float)b;
            float c= (float)(fa/ fb);
            //Console.WriteLine(string.Format("{0} / {1} = {2} ",fa,fb,c));
            return c;
        }
        /*
        private static void debug(string message)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Debug:" + message);
            Console.ForegroundColor = temp;
        }
        */
        public static bool AND(bool a, bool b)
        {
            return a && b;
        }

        public static bool OR(bool a, bool b)
        {
            return a || b;
        }

        public static string ObjToString(object obj)
        {
            if (obj == null) throw new NullReferenceException("obj不能为null"); //ZyyRTException("obj不能为null");
            if (obj is bool)
            {
                return ((bool)obj) ? "是" : "否";
            }
            else
            {
                return obj.ToString();
            }
        }

        public static string AddString(object a, object b)
        {
            if (a == null) throw new NullReferenceException("a不能为null");
            if (b == null) throw new NullReferenceException("b不能为null");
            string str1 = ObjToString(a);
            string str2 = ObjToString(b);
            return str1 + str2;
        }

        public static bool GT(object a,object b)
        {
            //return (double)a > (double)b;
            if (IsNumber(a) && IsNumber(b))
                return Convert.ToDouble(a) > Convert.ToDouble(b);
            throw new ZyyRTException("Calculater.GT失败");
        }

        public static bool GTInt(int a, int b)
        {
            return a > b;
        }

        public static bool GEInt(int a, int b)
        {
            return a > b;
        }

        public static bool GE(object a, object b)
        {
            if(IsNumber(a) && IsNumber(b))
                return Convert.ToDouble(a) >= Convert.ToDouble(b);
            throw new ZyyRTException("Calculater.GE失败");
        }

        public static bool EQBool(bool a, bool b)
        {
            return a == b;
        }

        public static bool EQInt(int a, int b)
        {
            return a == b;
        }

        public static bool EQFloat(float a, float b)
        {
            return a == b;
        }

        public static bool EQRef(object a, object b)
        {
            //return a == b;
            if (IsNumber(a) && IsNumber(b))
                return Convert.ToDouble(a) == Convert.ToDouble(b);
            else if(IsEnumValue(a)&&IsEnumValue(b))
                return (int)a==(int)b;
            else if ((a is string) && (b is string))
                return (string)a == (string)b;
            else
                return a == b;
            //throw new RTException("Calculater.EQ失败");
        }

        public static bool NEBool(bool a, bool b)
        {
            return a != b;
        }


        public static bool NEInt(int a, int b)
        {
            return a != b;
        }

        public static bool NEFloat(float a, float b)
        {
            return a != b;
        }

        public static bool NERef(object a, object b)
        {
            //return a != b;
            if (IsNumber(a) && IsNumber(b))
                return Convert.ToDouble(a) != Convert.ToDouble(b);
            else if (IsEnumValue(a) && IsEnumValue(b))
                return (int)a != (int)b;
            else if ((a is string) && (b is string))
                return (string)a != (string)b;
            else
                return a != b;
            //throw new RTException("Calculater.NE失败");
        }

        public static bool LTRef(object a, object b)
        {
            //return (double)a < (double)b;
            if (IsNumber(a) && IsNumber(b))
                return Convert.ToDouble(a) < Convert.ToDouble(b);
            throw new ZyyRTException("Calculater.LT失败");
        }

        public static bool LTInt(int a, int b)
        {
            return a < b;
        }

        public static bool LERef(object a, object b)
        {
            //return (double)a <= (double)b;
            if (IsNumber(a) && IsNumber(b))
                return Convert.ToDouble(a) <= Convert.ToDouble(b);
            throw new ZyyRTException("Calculater.LE失败");
        }

        public static bool LEInt(int a, int b)
        {
            return a <= b;
        }

        public static bool LEFloat(float a, float b)
        {
            return a <= b;
        }

        public static bool IsNumber(object x)
        {
            if(x is Type)
            {
                throw new ZyyRTException("参数不能为Type类型");
            }
            return x is byte || x is char || x is float || x is int || x is decimal || x is double;
        }

        public static bool IsNumberType(Type type)
        {
            return type==typeof(byte) || type==typeof(char)  ||type==typeof(float)
            || type == typeof(int) || type == typeof(decimal) || type == typeof(double);
        }

        public static bool IsEnumValue(object x)
        {
            Type t = x.GetType();
            return t.IsEnum;
        }

    }
}
