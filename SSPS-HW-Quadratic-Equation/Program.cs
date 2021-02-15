using System;
using System.Collections.Generic;
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

            while (true)
            {
                try
                {

                    //VariableCollection equation = VariableCollection.Parse(Console.ReadLine());
                    //equation.Simplify();

                    Equation equation = Equation.Parse(Console.ReadLine());
                    QuadraticEquation quadraticEquation = new QuadraticEquation(equation);
                    VariableCollection[] result = quadraticEquation.Solve();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    for (int i = 0; i < result.Length; i++)
                    {
                        Console.WriteLine($"x{ i } = { result[i] }");
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
