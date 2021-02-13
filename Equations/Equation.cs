using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equations
{
    public class Equation
    {
        private VariableCollection leftSide;
        private VariableCollection rightSide;

        public Equation(VariableCollection leftSide, VariableCollection rightSide)
        {
            this.leftSide = leftSide;
            this.rightSide = rightSide;
        }

        public void Simplify()
        {
            leftSide.Simplify();
            rightSide.Simplify();
        }

        public void SolveForZero()
        {
            leftSide -= rightSide;
            rightSide = 0;
            leftSide.Simplify();
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

        public override string ToString()
        {
            return $"{ leftSide } = { rightSide }";
        }
    }
}
