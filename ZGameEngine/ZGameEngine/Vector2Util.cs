using OpenTK;
using ZOpen2D;

namespace ZLogoEngine
{
    public static class Vector2Util
    {
        public static Vector2 Copy(Vector2 vector2)
        {
            return new Vector2(vector2.X,vector2.Y );
        }

        public static Vector2 Add(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X+v2.X,v1.Y+v2.Y);
        }

        public static Vector2 GetPointByPolar(float x, float y, float distance, float degrees)
        {
            var degrees2 = degrees;
            var sv = MathUtil.Cos(degrees2);
            var cv = MathUtil.Sin(degrees2);
            var mx = (float)(distance * sv);
            var my = (float)(distance *cv);
            return new Vector2(x + mx, y + my);
        }

        public static bool Eq(Vector2 v1,Vector2 v2)
        {
            return Eq(v1.X, v1.Y, v2);
        }

        public static bool Eq(float x,float y, Vector2 v2)
        {
            return x == v2.X && y == v2.Y;
        }

        public static bool IsOutWindow(Vector2 vector2, float x, float y)
        {
            return IsOutWindow(vector2.X, vector2.Y, x, y);
        }

        public static bool IsOutWindow(float width,float height, float x, float y)
        {
            if (x < 0 || y < 0) return true;
            if (x > width || y > height) return true;
            return false;
        }

        public static Vector2 GetInWindow(float width, float height, float x, float y)
        {
            var x2 = GetInMinMax(x, 0, width);
            var y2 = GetInMinMax(y, 0, height);
            return new Vector2(x2,y2);
        }

        private static float GetInMinMax(float value, float minValue, float maxValue)
        {
            if (value < minValue) return minValue;
            if (value > maxValue) return maxValue;
            return value;
        }
    }
}
