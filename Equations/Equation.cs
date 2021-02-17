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
            LeftSide = leftSide;
            RightSide = rightSide;
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

        public VariableCollection GetLeftSide() => LeftSide.Clone();
        public VariableCollection GetRightSide() => RightSide.Clone();

        public override string ToString() => ToString(false);

        public string ToString(bool useUnicodeCharacters)
        {
            return $"{ LeftSide.ToString(useUnicodeCharacters) } = { RightSide.ToString(useUnicodeCharacters) }";
        }

        public void SolveFor(VariableIdentifierCollection resultVariable)
        {
            SimpleSolveForZero();
            bool foundResultingVariable = false;
            VariableCollection _rightSide = new VariableCollection();
            foreach (Variable variable in LeftSide)
            {
                int matchingVariableCount = 0;
                foreach (VariableIdentifier varIdentifier in variable.Identifiers)
                {
                    if (resultVariable.Contains(varIdentifier))
                        matchingVariableCount++;
                }
                if (matchingVariableCount == 0)
                    continue;

                if (matchingVariableCount == resultVariable.Count)
                {
                    _rightSide.Add(variable);
                    foundResultingVariable = true;
                    continue;
                }

                throw new InvalidOperationException("Cannot solve this equation, beacuse ResultingVariable isn't a single variable in this context!");
            }

            if (LeftSide.Count == 0 || (LeftSide.Count == 1 && LeftSide[0].Multiplier == 0))
                throw new InfinitlyManySolutionsException();

            if (!foundResultingVariable)
                throw new NoSolutionException("The equation has no solution for the following variable: " + resultVariable);

            LeftSide -= _rightSide;
            RightSide = -(_rightSide);
            SimplifyBothSides();

            Variable finalVariable = new Variable(resultVariable, 1);

            int rightTermsCount = RightSide.Count;
            foreach (Variable variable in RightSide)
            {
                Variable divider = variable / finalVariable;
                LeftSide /= divider;
            }
            LeftSide /= rightTermsCount;
            RightSide = finalVariable;

            LeftSide.Simplify();

            VariableCollection leftSideTmp = LeftSide.Clone();
            LeftSide = RightSide;
            RightSide = leftSideTmp;
        }
    }
}
