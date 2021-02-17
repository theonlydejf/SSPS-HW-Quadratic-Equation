using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Equations;

namespace SSPS_HW_Quadratic_Equation
{
    class Program
    {
        // TODO:
        // - Graphs
        // - Documentation

        internal delegate VariableCollection[] PerformAction(Equation equation);

        static void Main(string[] args)
        {
            Func<Equation, ISolvableEquation>[] SupportedSolvableEquations = new Func<Equation, ISolvableEquation>[]
            {
                (equation) => new LinearEquation(equation),
                (equation) => new QuadraticEquation(equation)
            };

            Console.OutputEncoding = Encoding.Unicode;

            while (true)
            {
                List<Tuple<string, Func<string>>> Actions = new List<Tuple<string, Func<string>>>();

                try
                {
                    Console.WriteLine("Enter equation (or type 'help' for help): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string input = Console.ReadLine().ToLower();
                    if(input == "help")
                    {
                        EquationIntegration.PrintHelp();
                        continue;
                    }
                    Console.ResetColor();
                    Equation equation = null;
                    VariableCollection expression = null;
                    try
                    {
                        equation = Equation.Parse(input);
                    }
                    catch (FormatException)
                    {
                        expression = VariableCollection.Parse(input);
                    }

                    if(equation == null)
                    {
                        Actions.Add(new Tuple<string, Func<string>>(
                            "Simplify", 
                            () => { expression.Simplify(); return expression.ToString(true); }));
                    }
                    else
                    {
                        foreach (var ctor in SupportedSolvableEquations)
                        {
                            ISolvableEquation solvableEquation = null;
                            try
                            {
                                solvableEquation = ctor.Invoke(equation);
                            }
                            catch(Exception ex)
                            {
                                if (!(ex is FormatException || ex is ArgumentException))
                                    throw ex;
                                continue;
                            }
                            Actions.Add(new Tuple<string, Func<string>>(
                                "Solve as " + solvableEquation.Name,
                                () => EquationIntegration.EquationResultToString(solvableEquation.Solve(), solvableEquation.ResultingVariable.ToString(true))));
                        }
                        Actions.Add(new Tuple<string, Func<string>>(
                            "Solve for zero",
                            () => SolveForZero(equation)));
                        Actions.Add(new Tuple<string, Func<string>>(
                             "Solve for variable",
                             () => SolveFor(equation)));
                    }

                    int index = 1;
                    Console.WriteLine();
                    foreach(var ii in Actions)
                    {
                        Console.WriteLine(index + ": " + ii.Item1);
                        index++;
                    }
                    Console.WriteLine();

                    while(true)
                    {
                        Console.Write("Enter number of action you want to perform (or 'e' to exit): ");
                        string s = Console.ReadLine();
                        if (s.ToLower() == "e")
                            break;
                        if(uint.TryParse(s, out uint number))
                        {
                            if(number <= Actions.Count && number > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(Actions[(int)number - 1].Item2.Invoke());
                                Console.ResetColor();
                                break;
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\"{ s }\" either isn't a number or couldn't be used to perform an action!");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    WriteException(ex, false);
                }            
            }

        }

        static string SolveForZero(Equation equation)
        {
            equation.SimpleSolveForZero();
            return equation.ToString(true);
        }

        static string SolveFor(Equation equation)
        {
            string msg1 = "!! Same variables with different exponents are treated as different variables !!";
            string msg2 = "!! eg. x² + x = 2, solving for x => x = 2 - x² !!";
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - msg1.Length) / 2, Console.CursorTop);
            Console.WriteLine(msg1);
            Console.SetCursorPosition((Console.WindowWidth - msg2.Length) / 2, Console.CursorTop);
            Console.WriteLine(msg2 + "\n");

            Console.Write("Enter variable to solve for: ");
            Variable var = Variable.Parse(Console.ReadLine());
            equation.SolveFor(var.Identifiers);
            return equation.ToString(true);
        }

        static void WriteException(Exception ex, bool askForStacktrace = true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exception occured: " + ex.Message);
            Console.ResetColor();

            if (!askForStacktrace)
                return;
            Console.Write("Do you want to see the Stack Trace? (y for yes) ");
            Console.ResetColor();
            char keyChar = Console.ReadKey(false).KeyChar;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            if(keyChar == 'y' || keyChar == 'Y')
                Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
        }
    }
}
