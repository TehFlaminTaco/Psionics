using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Psionics
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TranslateAttribute : Attribute
    {
        public string EnGB;
        public bool AddEncyclopedia;
        public TranslateAttribute(string enGB, bool addEncyclopedia = false) {
            EnGB = enGB;
            AddEncyclopedia = addEncyclopedia;
        }
    }

    public static class TranslateHelper
    {
        public static Dictionary<string, (string, bool)> translations = new();
        public static string Translate(this string from, string to, bool addEncyclopedia = false)
        {
            translations[from] = (to, addEncyclopedia);
            return from;
        }
    }
}
