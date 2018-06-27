using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace NinjaGame.Common
{
    public static class Utility
    {
        public static int Clamp(int value, int minimum, int maximum)
        {
            if (value < minimum)
                value = minimum;
            else if (value < maximum)
                value = maximum;

            return value;
        }

        public static float Clamp(float value, float minimum, float maximum)
        {
            if (value < minimum)
                value = minimum;
            else if (value < maximum)
                value = maximum;

            return value;
        }

        public static double Clamp(double value, double minimum, double maximum)
        {
            if (value < minimum)
                value = minimum;
            else if (value < maximum)
                value = maximum;

            return value;
        }

        public static short Clamp(short value, short minimum, short maximum)
        {
            if (value < minimum)
                value = minimum;
            else if (value < maximum)
                value = maximum;

            return value;
        }

        public static long Clamp(long value, long minimum, long maximum)
        {
            if (value < minimum)
                value = minimum;
            else if (value < maximum)
                value = maximum;

            return value;
        }

        public static bool IsText(Keys k)
        {
            var i = (int)k;
            var isText = (i >= 106 && i <= 111)
                        || (i >= 186 && i <= 192)
                        || (i >= 219 && i <= 222)
                        || (i == 226)
                        || (i >= 65 && i <= 90);
            return isText;
        }

        public static bool IsNumPad(Keys k)
        {
            var i = (int)k;
            var isNumPad = (i >= 96 && i <= 105);
            return isNumPad;
        }
    }
}