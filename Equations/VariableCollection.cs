using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public class VariableCollection : List<Variable>
    {
        public VariableCollection(params Variable[] variables) : base(variables)
        {
            
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

                sb.Append(sb.Length > 0 ? Math.Abs(variable.Multiplier) : variable.Multiplier);
                sb.Append(variable.Identifier);
            }
            return sb.ToString();
        }
    }
}
