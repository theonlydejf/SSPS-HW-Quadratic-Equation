using System;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public class LinearEquation : Equation, ISolvableEquation
    {
        public LinearEquation(Equation equation) : base(equation.LeftSide, equation.RightSide)
        {
            VariableIdentifierCollection resultingVariable;

            SolvableEquationFunctions.CheckIfValid(this, 1, 1, new double[0], out resultingVariable);

            ResultingVariable = resultingVariable;
        }

        public VariableIdentifierCollection ResultingVariable { get; private set; }


        public VariableCollection[] Solve()
        {
            SolveFor(ResultingVariable);
            return new[] { RightSide };
        }
    }
}
