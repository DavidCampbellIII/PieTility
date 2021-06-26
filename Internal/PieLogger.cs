using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PieTility
{
    public class PieLogger
    {
        public static string headerDecorator = "==";

        public static Color errorColor = Color.red;
        public static Color warningColor = Color.yellow;

        public static void LogError(string header, string message)
        {
            Log("PieTility Error", errorColor, header, message);
        }

        public static void LogWarning(string header, string message)
        {
            Log("PieTility Warning", warningColor, header, message);
        }

        private static void Log(string title, Color color, string header, string message)
        {
            StringBuilder builder = new StringBuilder("<color=");
            builder.Append(Utilities.ColorToHexString(color, true)).Append(">").Append(title).Append("</color> - ");
            builder.Append(headerDecorator).Append(header).Append(headerDecorator).Append(" ").Append(message);
            Debug.LogError(builder);
        }
    }
}
