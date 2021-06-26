using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PieTility
{
    public static class Utilities
    {
        #region Gameplay

        public static void ToggleMouseLock(bool status)
        {
            if (status)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        #endregion

        #region floats

        public static void ClampLower(this ref float f, float min)
        {
            if (f < min)
            {
                f = min;
            }
        }

        public static void ClampUpper(this ref float f, float max)
        {
            if (f > max)
            {
                f = max;
            }
        }

        public static void Clamp(this ref float f, float min, float max)
        {
            if (min > max)
            {
                Swap(ref min, ref max);
            }
            f.ClampLower(min);
            f.ClampUpper(max);
        }

        #endregion

        #region ints

        public static void ClampLower(this ref int i, int min)
        {
            if (i < min)
            {
                i = min;
            }
        }

        public static void ClampUpper(this ref int i, int max)
        {
            if (i > max)
            {
                i = max;
            }
        }

        public static void Clamp(this ref int i, int min, int max)
        {
            if (min > max)
            {
                Swap(ref min, ref max);
            }
            i.ClampLower(min);
            i.ClampUpper(max);
        }

        #endregion

        #region Arrays

        /// <summary>
        /// Toggle all elements in array on or off.  Like using .enabled, but on all elements in the array
        /// </summary>
        /// <param name="status">Enabled or disabled</param>
        public static void ToggleAll<T>(this T[] arr, bool status) where T : MonoBehaviour
        {
            foreach (T obj in arr)
            {
                obj.enabled = status;
            }
        }

        #endregion

        #region Vector3s

        public static void ClampLower(this ref Vector3 v, Vector3 min)
        {
            v.x.ClampLower(min.x);
            v.y.ClampLower(min.y);
            v.z.ClampLower(min.z);
        }

        public static void ClampUpper(this ref Vector3 v, Vector3 max)
        {
            v.x.ClampUpper(max.x);
            v.y.ClampUpper(max.y);
            v.z.ClampUpper(max.z);
        }

        public static void Clamp(this ref Vector3 v, Vector3 min, Vector3 max)
        {
            v.ClampLower(min);
            v.ClampUpper(max);
        }

        /// <summary>
        /// Checks if two Vector3s are "close enough" specificed by a threshold value (defaults to flaot.Epsilon)
        /// </summary>
        /// <param name="v">This Vector3</param>
        /// <param name="other">The comparing Vector3</param>
        /// <param name="threshold">Maximum amount these Vectors can differ by to remain "equal"</param>
        /// <returns>If the two Vectors are "equal"</returns>
        public static bool FuzzyEquals(this ref Vector3 v, Vector3 other, float threshold = float.Epsilon)
        {
            return (v - other).sqrMagnitude <= threshold;
        }

        /// <summary>
        /// Creates a more precise Vector3 ToString
        /// </summary>
        /// <param name="v">The vector we want to process</param>
        /// <returns>Stringified Vector3</returns>
        public static string PreciseToString(this ref Vector3 v)
        {
            return v.ToString("F8");
        }

        #endregion

        #region Color

        public static string ColorToHexString(Color color, bool includePound=false)
        {
            string rr = FloatRGBToInt(color.r).ToString("X");
            string gg = FloatRGBToInt(color.g).ToString("X");
            string bb = FloatRGBToInt(color.b).ToString("X");
            string aa = FloatRGBToInt(color.a).ToString("X");

            StringBuilder builder = new StringBuilder();
            if(includePound)
            {
                builder.Append("#");
            }
            builder.Append(rr).Append(gg).Append(bb).Append(aa);
            return builder.ToString();
        }

        public static int FloatRGBToInt(float fRGB)
        {
            return (int)(fRGB * 255);
        }

        #endregion

        #region AXES

        /// <summary>
        /// Axes X = 1, Y = 2, Z = 4.
        /// </summary>
        public enum Axes
        {
            X = 1, Y = 2, Z = 4
        }

        /// <summary>
        /// Combos for axes.  Like Linux chmod, each value has own number.  X = 1, Y = 2, Z = 4.  Add for combos
        /// </summary>
        public enum AxesCombos
        {
            NONE = 0,
            X = 1,
            Y = 2,
            Z = 4,
            XY = 3, //1 + 2
            XZ = 5, //1 + 4
            YZ = 6, //2 + 4
            XYZ = 7 //1 + 2 + 4
        }

        /// <summary>
        /// Does a AxesCombo contain a single axis
        /// </summary>
        /// <param name="combo">Combo we are checking</param>
        /// <param name="axis">Axis we are searching for</param>
        /// <returns>If combo contains the given axis</returns>
        public static bool AxesComboContains(AxesCombos combo, Axes axis)
        {
            int comboInt = (int)combo;
            int axisInt = (int)axis;

            //first check if it's all, none, or one
            if (combo == AxesCombos.XYZ)
            {
                return true;
            }
            else if (combo == AxesCombos.NONE)
            {
                return false;
            }
            else if (comboInt <= (int)Axes.Z)
            {
                return comboInt == axisInt;
            }

            //if we have 2 axes in the combo, figure out the other 2 that aren't us
            Axes[] others = GetOtherAxes(axis);

            //if combo minus one of the other axes gives us our axis, then the combo of 2 must contain our axis!
            return comboInt - (int)others[0] == axisInt || comboInt - (int)others[1] == axisInt;
        }

        /// <summary>
        /// Given an axis, gives back the other 2 axes that it isn't
        /// </summary>
        /// <param name="axis">Given axis we want to find the opposite of</param>
        /// <returns>Other axes</returns>
        public static Axes[] GetOtherAxes(Axes axis)
        {
            Axes other1 = Axes.Y;
            Axes other2 = Axes.Z;
            if (axis == Axes.Y)
            {
                other1 = Axes.X;
            }
            else if (axis == Axes.Z)
            {
                other2 = Axes.X;
            }

            return new Axes[] { other1, other2 };
        }

        #endregion

        #region Other

        /// <summary>
        /// Swaps any two variables passed as references
        /// </summary>
        /// <typeparam name="T">Type of data being swapped</typeparam>
        /// <param name="lhs">Variable 1 to swap</param>
        /// <param name="rhs">Variable 2 to swap</param>
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        #endregion
    }
}
