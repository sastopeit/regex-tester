using System.Text.RegularExpressions;

namespace Sastopeit.RegexTester
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
