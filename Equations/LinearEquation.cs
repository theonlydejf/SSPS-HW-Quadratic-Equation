using System;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public class LinearEquation : Equation, ISolvableEquation
    {
        public LinearEquation(Equation equation, VariableIdentifierCollection variableToSolveFor) : base(equation.LeftSide, equation.RightSide)
        {
            ResultingVariable = variableToSolveFor;
            SimpleSolveForZero();
            foreach (Variable ii in LeftSide)
            {
                foreach (double jj in ii.Identifiers.GetExponents())
                {
                    if (jj != 1)
                        throw new ArgumentException("Inpputed equation is not linear!");
                }
            }
        }

        public VariableIdentifierCollection ResultingVariable { get; private set; }


        public VariableCollection[] Solve()
        {
            bool foundResultingVariable = false;
            VariableCollection _rightSide = new VariableCollection();
            char[] neededMarkers = ResultingVariable.GetMarkers();
            foreach (Variable variable in LeftSide)
            {
                int matchingMarkersCount = 0;
                foreach (char marker in variable.Identifiers.GetMarkers())
                {
                    if (Array.IndexOf(neededMarkers, marker) >= 0)
                        matchingMarkersCount++;
                }
                if (matchingMarkersCount == 0)
                    continue;

                if(matchingMarkersCount == ResultingVariable.Count)
                {
                    _rightSide.Add(variable);
                    foundResultingVariable = true;
                    continue;
                }

                throw new InvalidOperationException("Cannot solve this equation, beacuse ResultingVariable isn't a single variable in this context!");
            }

            if(!foundResultingVariable)
                throw new InvalidOperationException("Cannot solve this equation for this variable: " + ResultingVariable);

            LeftSide -= _rightSide;
            RightSide = -_rightSide;
            SimplifyBothSides();

            Variable finalVariable = new Variable(ResultingVariable, 1);

            int rightTermsCount = RightSide.Count;
            foreach (Variable variable in RightSide)
            {
                Variable divider = variable / finalVariable;
                LeftSide /= divider;
            }
            LeftSide /= rightTermsCount;
            RightSide = finalVariable;

            LeftSide.Simplify();

            return new[] { LeftSide };
        }
    }
}
