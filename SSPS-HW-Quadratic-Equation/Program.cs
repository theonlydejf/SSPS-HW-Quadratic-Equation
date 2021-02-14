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
                    VariableCollection equation = VariableCollection.Parse(Console.ReadLine());
                    equation.Simplify();
                    Console.WriteLine("\n" + equation + "\n");
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
