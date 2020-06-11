using System.Text.RegularExpressions;

namespace _0506_RegEx
{
    class Reg
    {
        private Regex Rex;

        public bool RegExp(string s, string rx)
        {           
                    Rex = new Regex(rx);
                    return Rex.IsMatch(s);
        }
    }
}
