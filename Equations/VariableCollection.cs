using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Equations
{
    public class VariableCollection : List<Variable>
    {
        public VariableCollection(params Variable[] variables) : base(variables)
        {
            
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

                if(!variables.ContainsKey(variable.Identifier))
                {
                    variables.Add(variable.Identifier, variable);
                    continue;
                }

                variables[variable.Identifier] = (Variable)(variables[variable.Identifier] + variable);
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

        public static VariableCollection Parse(string s)
        {
            s = Regex.Replace(s, @"\s", string.Empty);
            List<string> rawTerms = new List<string>();
            List<VariableCollection> bracketValues = new List<VariableCollection>();

            bool wasLastBracket = false;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char acChar = s[i];
                if (acChar == '+' || acChar == '-')
                {
                    if (sb.ToString() != string.Empty)
                    {
                        rawTerms.Add(sb.ToString());
                        sb.Clear();
                        sb.Append(acChar);
                    }
                }
                else if(acChar == '(')
                {
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
                        while(true)
                        {
                            acChar = s[i];

                            if(insideBracket)
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
                                if(acChar == '+' || acChar == '-')
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
                    sb.Clear();
                    if(i < s.Length)
                        acChar = s[i];

                    Variable multiplier1;
                    if (partsOfCurrentTerm[0] == "-")
                        multiplier1 = -1;
                    else if (partsOfCurrentTerm[0] == "+" || partsOfCurrentTerm[0] == "")
                        multiplier1 = 1;
                    else
                        multiplier1 = Variable.Parse(partsOfCurrentTerm[0]);

                    VariableCollection termResult = Parse(partsOfCurrentTerm[1]);

                    partsOfCurrentTerm.RemoveRange(0, 2);

                    foreach (string term in partsOfCurrentTerm)
                    {
                        if (term == string.Empty)
                            continue;

                        termResult *= Parse(term);
                    }
                    bracketValues.Add(termResult * multiplier1);
                    wasLastBracket = true;
                    sb.Append(acChar);
                }
                else
                {
                    sb.Append(acChar);
                    wasLastBracket = false;
                }
            }
            if(!wasLastBracket && sb.ToString() != string.Empty)
                rawTerms.Add(sb.ToString());

            VariableCollection variables = new VariableCollection();
            rawTerms.ForEach(val => variables.Add(Variable.Parse(val)));
            bracketValues.ForEach(collection => variables += collection);

            return variables;
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
            return new Variable(null, 1, number);
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

        public override string ToString()
        {
            if (Count == 1)
                return this[0].ToString();

            StringBuilder sb = new StringBuilder();
            foreach (Variable variable in this)
            {
                if (sb.Length > 0 && variable.Multiplier >= 0)
                    sb.Append(" + ");
                else if (sb.Length > 0 && variable.Multiplier < 0)
                    sb.Append(" - ");

                sb.Append(variable.ToString(sb.Length > 0));
            }
            return sb.ToString();
        }
    }
}
