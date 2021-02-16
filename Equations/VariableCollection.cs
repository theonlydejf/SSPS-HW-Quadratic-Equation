using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Equations
{
    public class VariableCollection : List<Variable>
    {
        private const string NUMBER_PATTERN = @"([+-]?(?=\.?\d)\d*([.,]\d+)?)";
        //(?=.*\d\*(([+-].?\d)|(.?\d)))
        private static string MultiplyNumbersPattern { get => $@"{ NUMBER_PATTERN }\*{ NUMBER_PATTERN }"; }
        private static string DivideNumberByNumberPattern { get => $@"{ NUMBER_PATTERN }\/{ NUMBER_PATTERN }"; }
        private static string DivideVariableByNumberPattern { get => $@"(?<=.)(?<!\d)\/{ NUMBER_PATTERN }(?=[+\-\)\/]|$)"; }

        public int Root { get; set; }

        public VariableCollection(params Variable[] variables) : base(variables)
        {
            Root = 1;
        }

        public void Simplify()
        {
            Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
            int simplifyCount = 0;
            foreach (Variable variable in this)
            {
                if (variable.Multiplier == 0)
                {
                    simplifyCount++;
                    continue;
                }

                if (!variables.ContainsKey(variable.Identifiers.ToString()))
                {
                    variables.Add(variable.Identifiers.ToString(), variable);
                    continue;
                }

                variables[variable.Identifiers.ToString()] = (Variable)(variables[variable.Identifiers.ToString()] + variable);
                simplifyCount++;
            }

            Clear();

            foreach(Variable variable in variables.Values)
            {
                Add(variable);
            }

            if (simplifyCount > 0)
                Simplify();
        }

        private static bool replaced = false;
        public static VariableCollection Parse(string s)
        {
            s = Regex.Replace(s, @"\s", string.Empty).Replace(',', '.');
                
            List<string> rawTerms = new List<string>();
            List<VariableCollection> bracketValues = new List<VariableCollection>();
            do
            {
                replaced = false;
                s = Regex.Replace(s, MultiplyNumbersPattern, ReplaceMultiplyOperand);
            } while (replaced);

            s = Regex.Replace(s, @"(?<!\*)\*(?!\*)", "");

            do
            {
                replaced = false;
                s = Regex.Replace(s, DivideVariableByNumberPattern, ReplaceDivideVariableOperand);
            } while (replaced);

            do
            {
                replaced = false;
                s = Regex.Replace(s, DivideNumberByNumberPattern, ReplaceDivideNumbersOperand);
            } while (replaced);

            bool wasLastBracket = false;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char acChar = s[i];
                if(acChar == '^')
                {
                    sb.Append(acChar);
                    i++;
                    sb.Append(s[i]);
                    continue;
                }
                if (acChar == '+' || acChar == '-')
                {
                    if (sb.ToString() != string.Empty)
                    {
                        rawTerms.Add(sb.ToString());
                        sb.Clear();
                    }
                }
                else if(acChar == '(')
                {
                    bracketValues.Add(ParseBracket(s, ref i, ref sb));
                    if (i < s.Length)
                        acChar = s[i];
                    sb.Clear();
                    wasLastBracket = true;
                }
                else
                {
                    wasLastBracket = false;
                }
                sb.Append(acChar);
            }
            if(!wasLastBracket && sb.ToString() != string.Empty)
                rawTerms.Add(sb.ToString());

            VariableCollection variables = new VariableCollection();
            rawTerms.ForEach(val => variables.Add((Variable)Variable.Parse(val)));
            bracketValues.ForEach(collection => variables += collection);

            return variables;
        }

        private static VariableCollection ParseBracket(string s, ref int i, ref StringBuilder sb)
        {
            char acChar;
            List<string> partsOfCurrentTerm = new List<string>();
            bool trueEnd = false;
            while (!trueEnd)
            {
                partsOfCurrentTerm.Add(sb.ToString());
                sb.Clear();
                bool insideBracket = s[i] == '(';

                int bracketCount = 0;
                if (insideBracket)
                    i++;
                while (true)
                {
                    acChar = s[i];

                    if (insideBracket)
                    {
                        if (acChar == '(')
                            bracketCount++;
                        else if (acChar == ')')
                            bracketCount--;
                        i++;
                        if (bracketCount < 0)
                            break;
                        sb.Append(acChar);
                    }
                    else
                    {
                        if (acChar == '(')
                            break;
                        if (acChar == '+' || acChar == '-')
                        {
                            i--;
                            break;
                        }
                        sb.Append(acChar);
                        i++;
                        if (i >= s.Length)
                            break;
                    }
                }

                if (i >= s.Length || s[i] == '+' || s[i] == '-')
                    break;

            }
            partsOfCurrentTerm.Add(sb.ToString());

            //Variable multiplier1;
            //bool dividing = false;
            if (partsOfCurrentTerm[0] == "-")
                partsOfCurrentTerm[0] = "-1";
            else if (partsOfCurrentTerm[0] == "+" || partsOfCurrentTerm[0] == "")
                partsOfCurrentTerm[0] = "1";

            // Create shallow copy of partsOfCurrentTerm (in C# string is a value type,
            // so we don't need to create a deep copy)
            List<string> partsOfCurrentTermCopy = partsOfCurrentTerm.GetRange(0, partsOfCurrentTerm.Count);
            partsOfCurrentTerm.Clear();

            //TODO kopirovat partsOfCurrentTerm a iterovat zkrz to
            for(int jj = 0; jj < partsOfCurrentTermCopy.Count; jj++)
            {
                string term = partsOfCurrentTermCopy[jj];

                if (!term.Contains("/") || term == "/")
                {
                    partsOfCurrentTerm.Add(term);
                    continue;
                }

                string[] parts = term.Split('/');

                for (int j = 0; j < parts.Length; j++)
                {
                    if (parts[j] == string.Empty)
                        parts[j] = "1";
                    else if (parts[j] == "-" && j != 0)
                        parts[j] = "-1";

                    partsOfCurrentTerm.Add(parts[j]);
                    partsOfCurrentTerm.Add("/");
                }
            }

            //if (partsOfCurrentTerm[partsOfCurrentTerm.Count - 1] == "/")
            //    throw new FormatException();

            List<VariableCollection> dividers = new List<VariableCollection>();
            dividers.Add(1);

            for (int j = partsOfCurrentTerm.Count - 1; j >= 0; j--)
            {
                if (partsOfCurrentTerm[j] == string.Empty)
                    continue;

                if (partsOfCurrentTerm[j] == "/")
                {
                    dividers[dividers.Count - 1].Simplify();
                    dividers.Add(1);
                    continue;
                }

                dividers[dividers.Count - 1] *= Parse(partsOfCurrentTerm[j]);
            }
            dividers[dividers.Count - 1].Simplify();

            VariableCollection termResult = 1;
            if (dividers.Count == 1)
            {
                termResult = dividers[0];
            }
            else
            {
                termResult = dividers[dividers.Count - 1];
                for (int j = dividers.Count - 2; j >= 0; j--)
                {
                    termResult = termResult / dividers[j];
                }
            }

            return termResult;
        }

        private static string ReplaceMultiplyOperand(Match match)
        {
            double number1 = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            double number2 = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
            replaced = true;
            return (number1 * number2).ToString().Replace(',', '.');
        }

        private static string ReplaceDivideNumbersOperand(Match match)
        {
            double number1 = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            double number2 = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
            replaced = true;
            return (number1 / number2).ToString().Replace(',', '.');
        }

        private static string ReplaceDivideVariableOperand(Match match)
        {
            double number1 = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            replaced = true;
            return (1d / number1).ToString().Replace(',','.');
        }

        public static explicit operator Variable(VariableCollection collection)
        {
            if (collection.Count != 1)
                throw new ArgumentException("Inputed collection contains none or multiple items!");
            return collection[0];
        }

        public static implicit operator VariableCollection(Variable variable)
        {
            return new VariableCollection(variable);
        }

        public static implicit operator VariableCollection(double number)
        {
            return (Variable)number;
        }

        public static VariableCollection operator +(VariableCollection a, Variable b)
        {
            VariableCollection vars = new VariableCollection();
            vars.AddRange(a);
            vars.Add(b);
            return vars;
        }

        public static VariableCollection operator +(Variable a, VariableCollection b)
        {
            VariableCollection vars = new VariableCollection();
            vars.Add(a);
            vars.AddRange(b);
            return vars;
        }

        public static VariableCollection operator +(VariableCollection a, VariableCollection b)
        {
            VariableCollection vars = new VariableCollection();
            vars.AddRange(a);
            vars.AddRange(b);
            return vars;
        }

        public static VariableCollection operator -(VariableCollection a)
        {
            VariableCollection variables = new VariableCollection();
            foreach (Variable variable in a)
            {
                variables.Add(-variable);
            }
            return variables;
        }

        public static VariableCollection operator -(VariableCollection a, VariableCollection b)
        {
            return a + -b;
        }

        public static VariableCollection operator -(VariableCollection a, Variable b)
        {
            return a + -b;
        }

        public static VariableCollection operator -(Variable a, VariableCollection b)
        {
            return a + -b;
        }

        public static VariableCollection operator *(VariableCollection a, VariableCollection b)
        {
            VariableCollection variables = new VariableCollection();
            foreach (Variable ii in a)
            {
                foreach (Variable jj in b)
                {
                    variables.Add(ii * jj);
                }
            }
            return variables;
        }

        public static VariableCollection operator /(VariableCollection a, VariableCollection b)
        {
            //if (a.Count > 1 )
            //    throw new NotImplementedException("Dividing polynomials is not yet implemented!");
            if(b.Count > 1)
                throw new NotImplementedException("Dividing by polynomials is not yet implemented!");

            VariableCollection result = new VariableCollection();
            Variable divider = (Variable)b;

            foreach (Variable variable in a)
            {
                result.Add(variable / divider);
            }

            return result;
        }

        public override string ToString() => ToString(false);

        public string ToString(bool useUnicodeCharacteres)
        {
            if (Count == 0)
                return "";

            if (Count == 1 && this[0].Multiplier == 0)
                return "0";

            string _out = this[0].ToString(false, useUnicodeCharacteres);
            if (Count > 1)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Variable variable in this)
                {
                    if (sb.Length > 0 && variable.Multiplier >= 0)
                        sb.Append(" + ");
                    else if (sb.Length > 0 && variable.Multiplier < 0)
                        sb.Append(" - ");

                    sb.Append(variable.ToString(sb.Length > 0, useUnicodeCharacteres));
                }
                _out = sb.ToString();
            }

            if (Root == 1)
                return _out;
            else if (Root == 2)
                return ToStringHelper.RadicalSymbol + $"({ _out })";
            else if (Root > 2)
                return ToStringHelper.IntToSuperscript(Root) + ToStringHelper.RadicalSymbol + $"({Root})";

            throw new InvalidOperationException("Root with value " + Root + " doesn't make sense!");
        }
    }
}
