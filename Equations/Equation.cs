using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equations
{
    public class Equation
    {
        internal protected VariableCollection LeftSide;
        internal protected VariableCollection RightSide;

        public Equation(VariableCollection leftSide, VariableCollection rightSide)
        {
            this.LeftSide = leftSide;
            this.RightSide = rightSide;
        }

        public void SimplifyBothSides()
        {
            LeftSide.Simplify();
            RightSide.Simplify();
        }

        public void SimpleSolveForZero()
        {
            LeftSide -= RightSide;
            RightSide = 0;
            LeftSide.Simplify();
        }

        public static Equation Parse(string s)
        {
            string[] equationSidesStr = s.Split('=');
            if (equationSidesStr.Length != 2)
                throw new FormatException();

            return new Equation(
                VariableCollection.Parse(equationSidesStr[0]),
                VariableCollection.Parse(equationSidesStr[1]));
        }

        public override string ToString() => ToString(false);

        public string ToString(bool useUnicodeCharacters)
        {
            return $"{ LeftSide.ToString(useUnicodeCharacters) } = { RightSide.ToString(useUnicodeCharacters) }";
        }
    }
}
