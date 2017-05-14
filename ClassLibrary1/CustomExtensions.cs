using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CustomExtensions
{
    public static class StringExtension
    {
        // Make just the first letter in the string uppercase.
        // (culture-insensitive)
        //
        // I was trying to explain that I was figuring out how to extend the string class to do
        // such a thing and was instantly hit by the following suggestions:
        // a) Use Proper() (or ToTitleCase() from system.globalization)
        // b) Use ToUpper()
        //
        // Proper Case Would Capitalise All First Letters And Lowercase All Other Letters.
        // TOUPPER() WOULD CAPITALISE EVERYTHING.

        public static string CapitaliseBeginning(this string str)
        {
            string s;
            s = str.Substring(0, 1).ToUpperInvariant();
            s += str.Substring(1, (str.Length - 1));
            return s;
        }

        // Make just the first letter in the string lowercase.
        // (culture-insensitive)
        public static string LowercaseBeginning(this string str)
        {
            string s;
            s = str.Substring(0, 1).ToLowerInvariant();
            s += str.Substring(1, (str.Length - 1));
            return s;

        }
    }

    public static class ReflectionUtility
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType, Assembly assembly)
        {
            var types = from t in assembly.GetTypes()
                        where t.IsSubclassOf(baseType)
                        select t;

            return types;
        }
    }

}