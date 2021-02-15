using System;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public class QuadraticEquation : Equation, ISolvableEquation
    {
        public QuadraticEquation(Equation equation) : base(equation.LeftSide, equation.RightSide)
        {
            SolveForZero();

            VariableIdentifierCollection? defaultIdentifiers = null;
            foreach (var ii in LeftSide)
            {
                if (ii.Identifiers.Count > 0)
                {
                    defaultIdentifiers = ii.Identifiers;
                    break;
                }
            }

            if (!defaultIdentifiers.HasValue)
                throw new ArgumentException("Inputted equation doesn't have any variables!");

            bool foundSecondPower = false;
            foreach (Variable variable in LeftSide)
            {
                VariableIdentifierCollection identifiers = variable.Identifiers;
                if (identifiers.ContainsMarker('i'))
                    throw new NotImplementedException("Cannot solve quadratic equations with imaginary numbers in them!");

                for (int i = 0; i < identifiers.Count; i++)
                {
                    if (!(identifiers.HasSameMarkersAs(defaultIdentifiers.Value) || identifiers.Count == 0))
                        throw new ArgumentException("Inputted equation isn't a quadratic equation (There are two or more different variables)!");

                    double[] exponents = identifiers.GetExponents();
                    for (int j = 1; j < exponents.Length; j++)
                    {
                        if(exponents[j] != exponents[j - 1])
                            throw new ArgumentException($"Inputted equation isn't a quadratic equation (term \"{ variable }\" has two or more different exponents)!");
                    }

                    if(exponents[0] > 2 || exponents[0] < 1)
                        throw new ArgumentException($"Inputted equation isn't a quadratic equation (term \"{ variable }\" has wrong exponent)!");
                    if (exponents[0] == 2)
                        foundSecondPower = true;
                }

            }
                if (!foundSecondPower)
                    throw new ArgumentException("Inputted equation is linear and not quadratic!");
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
