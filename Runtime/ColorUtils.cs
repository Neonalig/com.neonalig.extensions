#nullable enable
using System;
using System.Diagnostics;
using System.Text;
using Neonalig.Hashing;
using UnityEngine;

namespace Neonalig.Extensions
{
    public static class ColorUtils
    {
        private static int ToSignedSpace(this uint value)
        {
            if (value < 0x80000000)
                return (int)value;
            return (int)(value - 0x100000000);
        }

        /// <summary> Gets a unique colour from the given hash code. </summary>
        public static Color GetFromHash(int hash, float s, float v, float a = 1f)
        {
            float hue = hash % 360f;
            Color c = Color.HSVToRGB(Mathf.Abs(hue) / 360f, s, v);
            c.a = a;
            return c;
        }

        /// <inheritdoc cref="GetFromHash(int,float,float,float)"/>
        public static Color GetFromHash(uint hash, float s, float v, float a = 1f) => GetFromHash(hash.ToSignedSpace(), s, v, a);

        /// <summary> Gets a unique deterministic colour from the given value. </summary>
        public static Color GetUniqueColor(this int value, float s, float v, float a = 1f) => GetFromHash(value.GetFnv32Hash(), s, v, a);

        /// <inheritdoc cref="GetUniqueColor(int,float,float,float)"/>
        public static Color GetUniqueColor(this uint value, float s, float v, float a = 1f) => GetFromHash(value.GetFnv32Hash(), s, v, a);

        /// <inheritdoc cref="GetUniqueColor(int,float,float,float)"/>
        /// <remarks> If the string is <see langword="null"/> or empty, <see cref="Color.white"/> is returned. </remarks>
        public static Color GetUniqueColor(this string str, float s, float v, float a = 1f) => string.IsNullOrEmpty(str) ? Color.white : str.GetFnv32Hash().GetUniqueColor(s, v, a);

        /// <inheritdoc cref="GetUniqueColor(int,float,float,float)"/>
        public static Color GetUniqueColor(this Type type, float s, float v, float a = 1f) => type.Name.GetUniqueColor(s, v, a);

        /// <inheritdoc cref="GetUniqueColor(int,float,float,float)"/>
        public static Color GetUniqueColor(this object? obj, float s, float v, float a = 1f) => obj.GetFnv32Hash().GetUniqueColor(s, v, a);

        /// <summary> Gets a Rich Text Format equivalent string with the given colour. </summary>
        public static string GetColoredString(this string str, Color color) => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{str}</color>";

        /// <summary> Appends a Rich Text Format equivalent string with the given colour to the given <see cref="StringBuilder"/>. </summary>
        public static StringBuilder AppendColoredString(this StringBuilder sb, string str, Color color) => sb.Append("<color=#")
            .Append(ColorUtility.ToHtmlStringRGB(color))
            .Append(">")
            .Append(str)
            .Append("</color>");
    }
}
