using System;
using System.Collections.Generic;
using System.Text;

namespace Sastopeit.RegexTester.Checkers
{
    class QuantifierCheckerList
    {
        public bool Check(int position, string regexString)
        {
            return (new BracketQuantifierChecker()).Check(position, regexString)
                    & (new PlusQuantifierChecker()).Check(position, regexString)
                    & (new QuestionmarkQuantifierChecker()).Check(position, regexString)
                    & (new AsteriskQuantifierChecker()).Check(position, regexString);
        }
    }
}
