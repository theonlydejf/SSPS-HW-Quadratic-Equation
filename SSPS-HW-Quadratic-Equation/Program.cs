using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Equations;

namespace SSPS_HW_Quadratic_Equation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine('ᐧ');

            while (true)
            {
                try
                {
                    //TODO opravit parsovani zapornych exponentu

                    Console.Write("Enter equation: ");
                    Equation equation = Equation.Parse(Console.ReadLine());
                    Console.Write("Enter variable to solve for: ");
                    Variable variable = Variable.Parse(Console.ReadLine());
                    Console.WriteLine(variable);

                    LinearEquation quadraticEquation = new LinearEquation(equation, variable.Identifiers);
                    VariableCollection[] result = quadraticEquation.Solve();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    for (int i = 0; i < result.Length; i++)
                    {
                        Console.WriteLine($"{ quadraticEquation.ResultingVariable }{ ToStringHelper.IntToSubscript(i + 1) } = { result[i].ToString(true) }");
                    }
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    WriteException(ex, true);
                }            
            }

            Console.ReadKey(true);
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
