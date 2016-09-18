using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class RegexExtractor
    {
        public string regexstring;
        public RegexExtractor(string regex)
        {
            this.regexstring = regex;
        }
        public string[] extractFromString(string line, out int charnum)
        {
            Regex regex = new Regex(regexstring);

            Match match = regex.Match(line);
            if (match.Success)
            {
                string[] result = new string[match.Groups.Count - 1];
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    result[i - 1] = match.Groups[i].Value;
                }
                charnum = match.Groups[0].Length;
                return result;
            }
            charnum = -1;
            return null;
        }
    }
}
