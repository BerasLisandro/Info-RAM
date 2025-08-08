using System;
using System.Linq;

namespace InfoRAMApp.Utils
{
    public static class SafeConvert
    {
        public static int ToInt(object? value, int defaultValue = 0)
        {
            return value is int i
                ? i
                : int.TryParse(value?.ToString(), out int result) ? result : defaultValue;
        }

        public static double ToDouble(object? value, double defaultValue = 0.0)
        {
            return value is double d
                ? d
                : double.TryParse(value?.ToString(), out double result) ? result : defaultValue;
        }

        public static string ToStr(object? value, string defaultValue = "-")
        {
            return value?.ToString() ?? defaultValue;
        }

        public static string LimpiarCodificacion(string input)
        {
            return new string(input.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
        }
    }
}