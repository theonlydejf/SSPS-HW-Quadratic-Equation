using System;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public static class ToStringHelper
    {
        public const char SubscriptAsciiStart = '₀';
        public const char SubscriptNegativeSign = '₋';

        public const char RadicalSymbol = '\u221A';

        public static char[] Superscripts { get => new[] { '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹' }; }
        public const char SuperscriptNegativeSign = '⁻';

        public static string IntToSubscript(int i)
        {
            StringBuilder indexSB = new StringBuilder(i < 0 ? SubscriptNegativeSign.ToString() : "");

            foreach (char digit in Math.Abs(i).ToString())
            {
                indexSB.Append((char)(int.Parse(digit.ToString()) + SubscriptAsciiStart));
            }
            return indexSB.ToString();
        }

        public static string IntToSuperscript(int i)
        {
            StringBuilder indexSB = new StringBuilder(i < 0 ? SuperscriptNegativeSign.ToString() : "");
            var superscript = Superscripts;

            foreach (char digit in Math.Abs(i).ToString())
            {
                indexSB.Append(superscript[int.Parse(digit.ToString())]);
            }
            return indexSB.ToString();
        }
    }
}
