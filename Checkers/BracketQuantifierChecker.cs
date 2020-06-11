using System;
using System.Collections.Generic;
using System.Text;

namespace Sastopeit.RegexTester.Checkers
{
    class BracketQuantifierChecker
    {
        public bool Check(int position, string regexString)
        {
            return !(regexString.Substring(0, position).EndsWith('}') | regexString.Substring(position).StartsWith('{'));
        }   
    }
}
