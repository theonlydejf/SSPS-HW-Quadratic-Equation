using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    Equation equation = Equation.Parse(Console.ReadLine());
                    equation.SolveForZero();
                    Console.WriteLine("\n" + equation);
                    /*VariableCollection variables = new VariableCollection();
                    while(true)
                    {
                        string s = Console.ReadLine();
                        if (s == "end")
                            break;
                        
                        try
                        {
                            variables.Add(Variable.Parse(s));
                        }
                        catch (Exception ex)
                        {
                            WriteException(ex);
                        }
                    }
                    variables.Simplify();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(variables);
                    Console.ResetColor();*/
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }            
            }

            Console.ReadKey(true);
        }

        static void WriteException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exception occured when parsing variable:\n" + ex.ToString());
            Console.ResetColor();
        }
    }
}
