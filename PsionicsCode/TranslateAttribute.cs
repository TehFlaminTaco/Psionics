using System;
using System.Collections.Generic;
using System.Linq;
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
}
