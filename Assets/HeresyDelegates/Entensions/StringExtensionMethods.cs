using System;

namespace HereticalSolutions
{
    /// <summary>
    /// Extension methods related to strings
    /// </summary>
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Return type.ToString() without namespace
        /// </summary>
        /// <param name="type">Target type</param>
        /// <returns>Type name</returns>
        public static string ToBeautifulString(this Type type)
        {
            string[] parts = type.ToString().Split('.');

            return parts[parts.Length - 1];
        }
    }
}