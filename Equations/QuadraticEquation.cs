using System;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public class QuadraticEquation : Equation, ISolvableEquation
    {
        public VariableIdentifierCollection ResultingVariable { get; private set; }

        public QuadraticEquation(Equation equation) : base(equation.LeftSide, equation.RightSide)
        {
            VariableIdentifierCollection resultingVariable;

            SolvableEquationFunctions.CheckIfValid(this, 1, 2, new[] { 2d }, out resultingVariable);

            ResultingVariable = resultingVariable;
        }

        public VariableCollection[] Solve()
        {
            double a = 0;
            double b = 0;
            double c = 0;

            foreach (Variable variable in LeftSide)
            {
                if (variable.Identifiers.Count == 0)
                {
                    c = variable.Multiplier;
                    continue;
                }

                if (variable.Identifiers[0].Exponent == 1)
                {
                    b = variable.Multiplier;
                    continue;
                }

                if (variable.Identifiers[0].Exponent == 2)
                {
                    a = variable.Multiplier;
                    continue;
                }

                throw new ArgumentException("You are trying to solve a qudratic equation which is not quadratic!");
            }

            if(a == 0)
                throw new ArgumentException("Inputted equation is linear and not quadratic!");

            double discriminant = b * b - 4 * a * c;
            if(discriminant > 0)
            {
                double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                return new VariableCollection[] { x1, x2 };
            }
            else if(discriminant < 0)
            {
                Variable i = new Variable(new[] { 'i' }, new[] { 1d }, 1);

                VariableCollection x1 = (-b + i * Math.Sqrt(-discriminant)) / (2 * a);
                VariableCollection x2 = (-b - i * Math.Sqrt(-discriminant)) / (2 * a);

                return new VariableCollection[] { x1, x2 };
            }
            else
            {
                return new VariableCollection[] { -(b / (2 * a)) };
            }
        }
    }
}
