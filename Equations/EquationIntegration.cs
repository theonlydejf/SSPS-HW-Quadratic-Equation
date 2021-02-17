using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Equations
{
    public static class EquationIntegration
    {
        public static string EquationResultToString(VariableCollection[] result, string resultVariable = "x")
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.AppendLine($"{ resultVariable }{ ToStringHelper.IntToSubscript(i + 1) } = { result[i].ToString(true) }");
            }
            return sb.ToString();
        }

        public static void PrintHelp()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ResetColor();
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("Equations.help.txt");
            Console.Write(" ");
            using(StreamReader sr = new StreamReader(stream))
            {
                int minMove = 1;
                while(true)
                {
                    int i = 0;
                    int lastY = Console.CursorTop;
                    do
                    {
                        if (sr.EndOfStream)
                            break;
                        char c = (char)sr.Read();
                        if(c == '$')
                        {
                            char[] num = new char[2];
                            sr.ReadBlock(num, 0, 2);
                            Console.ForegroundColor = (ConsoleColor)int.Parse(new string(num));
                            continue;
                        }
                        Console.Write(c);
                        if(Console.CursorTop != lastY)
                        {
                            i++;
                            Console.Write(" ");
                        }
                        lastY = Console.CursorTop;
                    } while (i < minMove || Console.CursorTop < Console.WindowHeight - 1);
                    minMove = 1;
                    ConsoleColor bgColor = ConsoleColor.White;
                    ConsoleColor txtColor = ConsoleColor.Black;
                    if (Console.BackgroundColor == ConsoleColor.White)
                    {
                        bgColor = ConsoleColor.Black;
                        txtColor = ConsoleColor.White;
                    }
                    int x = Console.CursorLeft, y = Console.CursorTop;
                    Console.BackgroundColor = bgColor;
                    Console.ForegroundColor = txtColor;
                    if (sr.EndOfStream)
                        Console.Write("\n: (q: EXIT)");
                    else
                        Console.Write(": (q: EXIT, Spacebar: Move down 5x, any key: Move down)");
                    Console.ResetColor();
                    Console.SetCursorPosition(x, y);
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Q)
                        break;
                    if (key == ConsoleKey.Spacebar)
                        minMove = 5;
                    Console.Write("                                                       ");
                    Console.SetCursorPosition(x, y);
                }
            }
            Console.ResetColor();
            Console.Clear();
        }
    }
}
