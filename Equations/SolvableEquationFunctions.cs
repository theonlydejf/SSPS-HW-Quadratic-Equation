using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Equations
{
    internal static class SolvableEquationFunctions
    {
        internal static void CheckIfValid(Equation equation, double minExponent, double maxExponent, double[] neededExponents, out VariableIdentifierCollection resultingVariable)
        {
            equation.SimpleSolveForZero();
            VariableCollection LeftSide = equation.GetLeftSide();

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
            {
                if (LeftSide[0].Multiplier != 0)
                    throw new NoSolutionException();
                throw new InfinitlyManySolutionsException();
            }

            bool[] foundExponents = new bool[neededExponents.Length];
            foreach (Variable variable in LeftSide)
            {
                VariableIdentifierCollection identifiers = variable.Identifiers;
                if (identifiers.ContainsMarker('i'))
                    throw new NotImplementedException("Cannot solve equations with imaginary numbers in them!");

                for (int i = 0; i < identifiers.Count; i++)
                {
                    if (!(identifiers.HasSameMarkersAs(defaultIdentifiers.Value) || identifiers.Count == 0))
                        throw new ArgumentException("Inputted equation isn't a solvable equation (there are two or more different variables)!");

                    double[] exponents = identifiers.GetExponents();
                    for (int j = 1; j < exponents.Length; j++)
                    {
                        if (exponents[j] != exponents[j - 1])
                            throw new ArgumentException($"Inputted equation isn't a solvable equation (two or more different exponents in one term)!");
                    }

                    if (exponents[0] > maxExponent || exponents[0] < minExponent)
                        throw new ArgumentException($"Inputted equation isn't a required type equation (one or more terms have wrong exponent)!");
                    int exponentIndex = Array.IndexOf(neededExponents, exponents[0]);
                    if (exponentIndex >= 0)
                        foundExponents[exponentIndex] = true;
                }

            }
            if (!foundExponents.All(x => x))
                throw new ArgumentException("Inputted equation doesn't meet necessary requirements (doesn't have all expected exponents)!");

            VariableIdentifier[] solvableIdentifiers = new VariableIdentifier[defaultIdentifiers.Value.Count];
            for (int i = 0; i < solvableIdentifiers.Length; i++)
            {
                solvableIdentifiers[i] = new VariableIdentifier(defaultIdentifiers.Value[i].Marker, 1);
            }

            resultingVariable = new VariableIdentifierCollection(solvableIdentifiers);
        }
    }
}
