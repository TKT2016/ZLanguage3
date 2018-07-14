using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZOpen2D
{
    public static class MathUtil
    {
        public static float RoundFloat(float value)
        {
            double result = Math.Round(value, 6);
            return (float)result;
        }

        public static float Angle(float degrees)
        {
            var degrees2 = degrees;
            double angle = Math.PI * degrees2 / 180.0;
            return (float)angle;
        }

        public static double Sin(float degrees)
        {
            double angle = Angle(degrees);
            double result = Math.Sin(angle);
            result = Math.Round(result, 6);
            return result;
        }

        public static double Cos(float degrees)
        {
            double angle = Angle(degrees);
            double result = Math.Cos(angle);
            result = Math.Round(result, 6);
            return result;
        }
    }
}
