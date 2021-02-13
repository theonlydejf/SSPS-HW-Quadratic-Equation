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
                    Console.WriteLine("Enter first variable:");
                    Variable a = Variable.Parse(Console.ReadLine());
                    Console.WriteLine("Enter second variable:");
                    Variable b = Variable.Parse(Console.ReadLine());
                    Console.WriteLine("a + b: " + (a + b));
                    Console.WriteLine("a - b: " + (a - b));

                    try
                    {
                        Console.WriteLine("a * b: " + (a * b));
                    }
                    catch (Exception ex)
                    {
                        WriteException(ex);
                    }

                    try
                    {
                        Console.WriteLine("a / b: " + (a / b));
                    }
                    catch (Exception ex)
                    {
                        WriteException(ex);
                    }
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
