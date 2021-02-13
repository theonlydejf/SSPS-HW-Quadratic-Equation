using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Equations
{
    public struct Variable
    {
        private const string PARSE_NUMBER_PATTERN = @"[+-]?\d*([.,]\d+)?";
        private const string PARSE_IDENTIFIER_PATTERN = @"[a-zA-Z](\^\d+)?";
        private static string IsStringVariablePattern { get => $@"^(?=.+)({ PARSE_NUMBER_PATTERN })?({ PARSE_IDENTIFIER_PATTERN })?$"; }

        public char? Marker { get; }
        public int Exponent { get; }
        public double Multiplier { get; }
        public string Identifier 
        {
            get
            {
                if (Exponent != 1)
                    return Marker + "^" + Exponent;
                return Marker == null ? "null" : Marker.ToString();
            }
        }

        public Variable(char? marker, int exponent, double multiplier)
        {
            Marker = marker;
            Exponent = exponent;
            Multiplier = multiplier;

            if (multiplier == 0)
                Marker = null;
        }

        public static Variable Parse(string s)
        {
            if (!IsStringVariable(s, out Match match))
                throw new FormatException();

            string multiplierString = match.Groups[1].Value;
            double multiplier = 1;
            if (multiplierString == "-")
                multiplier = -1;
            else if (multiplierString != string.Empty && multiplierString != "+")
                multiplier = double.Parse(multiplierString, NumberStyles.Any, CultureInfo.InvariantCulture);

            string identifier = match.Groups[3].Value;
            char? marker = null;
            if (identifier != string.Empty)
                marker = identifier[0];

            int exponent = 1;
            if(identifier.Length >= 3)
                exponent = int.Parse(identifier.Substring(2, identifier.Length - 2));

            if (exponent < 0)
                throw new NotImplementedException("Negative exponents are not yet implemented!");

            return new Variable(marker, exponent, multiplier);
        }

        private static bool IsStringVariable(string s, out Match match)
        {
            match = Regex.Match(s, IsStringVariablePattern);
            return match.Success;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            if (!(obj is Variable))
                return false;

            Variable var = (Variable)obj;

            return var.Marker == Marker &&
                var.Identifier == var.Identifier;
        }

        public override int GetHashCode()
        {
            int hashCode = -1645715709;
            hashCode = hashCode * -1521134295 + Marker.GetHashCode();
            hashCode = hashCode * -1521134295 + Exponent.GetHashCode();
            hashCode = hashCode * -1521134295 + Multiplier.GetHashCode();
            return hashCode;
        }

        public static VariableCollection operator +(Variable a, Variable b)
        {
            if(a.Identifier == b.Identifier)
            {
                return new Variable(a.Marker, a.Exponent, a.Multiplier + b.Multiplier);
            }
            return new VariableCollection(a, b);
        }

        public static Variable operator -(Variable a)
        {
            return new Variable(a.Marker, a.Exponent, -a.Multiplier);
        }

        public static VariableCollection operator -(Variable a, Variable b)
        {
            return a + -b;
        }

        public static Variable operator *(Variable a, Variable b)
        {
            if (b.Marker == null)
                return new Variable(a.Marker, a.Exponent, a.Multiplier * b.Multiplier);

            if (a.Marker != b.Marker)
                throw new NotImplementedException("Multiplying two different variables is not yet implemented!");

            return new Variable(a.Marker, a.Exponent + b.Exponent, a.Multiplier * b.Multiplier);
        }

        public static Variable operator /(Variable a, Variable b)
        {
            if (b.Marker.HasValue)
                throw new NotImplementedException("Dividing by a variable is not yet implemented!");

            if (b.Multiplier == 0)
                throw new DivideByZeroException();

            return new Variable(a.Marker, a.Exponent, a.Multiplier / b.Multiplier);
        }

        public static explicit operator double(Variable variable)
        {
            if (variable.Marker.HasValue)
                throw new ArgumentException("Inputed variable is not a raw number!");

            return variable.Multiplier;
        }

        public override string ToString() => ToString(false);

        internal string ToString(bool withoutSigns)
        {
            double val = Multiplier;
            if(withoutSigns)
                val = Math.Abs(Multiplier);

            if (Marker == null)
                return Multiplier.ToString();
            if (val == 1)
                return Identifier;
            if (val == -1)
                return "-" + Identifier;

            return val + Identifier;
        }
    }
}
