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
            VariableCollection variables = new VariableCollection();
            string parsable = Regex.Replace(s, @"\s", "").Replace("-", "+-");
            string[] variablesStr = parsable.Split('+');

            for (int i = 0; i < variablesStr.Length; i++)
            {
                string varData = variablesStr[i];

                if(varData.Contains("("))
                {
                    VariableCollection result = HandleBrackets(variablesStr, ref i);
                    variables.AddRange(result);
                    continue;
                }

                if (varData == string.Empty)
                    continue;

                variables.Add(Variable.Parse(varData));
            }

            return variables;
        }

        private static VariableCollection HandleBrackets(string[] variablesStr, ref int i)
        {
            int skipCount = 1;
            int multiplier = 1;
            Variable multiplyingVar = 1;
            if(variablesStr[i][0] != '-' || variablesStr[i][1] != '(')
            {
                string[] data = variablesStr[i].Split('(');
                skipCount = data[0].Length + 1;
                multiplyingVar = Variable.Parse(data[0]);
            }
            else if(variablesStr[i][0] == '-')
            {
                multiplier = -1;
                skipCount = 2;
            }
            VariableCollection variables = new VariableCollection();
            variablesStr[i] = variablesStr[i].Substring(skipCount, variablesStr[i].Length - skipCount);
            i--;

            while(true)
            {
                i++;
                string varData = variablesStr[i];

                if (varData.Contains("("))
                {
                    VariableCollection result = HandleBrackets(variablesStr, ref i);
                    variables.AddRange(result);
                    continue;
                }

                if (varData == string.Empty)
                    continue;

                if (varData.Contains(")"))
                    break;

                variables.Add(Variable.Parse(varData) * multiplier * multiplyingVar);
            }

            variables.Add(Variable.Parse(variablesStr[i].Substring(0, variablesStr[i].Length - 1)) * multiplier * multiplyingVar);

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
