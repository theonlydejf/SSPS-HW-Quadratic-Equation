using System.Collections.Generic;

namespace Equations
{
    public struct VariableIdentifier
    {
        public char Marker { get; }
        public double Exponent { get; }

        public VariableIdentifier(char marker, double exponent)
        {
            Marker = marker;
            Exponent = exponent;
        }

        public static implicit operator string(VariableIdentifier identifier)
        {
            if (identifier.Exponent != 1)
                return identifier.Marker + "^" + identifier.Exponent;
            return identifier.Marker.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VariableIdentifier))
            {
                return false;
            }

            var identifier = (VariableIdentifier)obj;
            return EqualityComparer<char?>.Default.Equals(Marker, identifier.Marker) &&
                   Exponent == identifier.Exponent;
        }

        public override int GetHashCode()
        {
            var hashCode = -599005067;
            hashCode = hashCode * -1521134295 + EqualityComparer<char?>.Default.GetHashCode(Marker);
            hashCode = hashCode * -1521134295 + Exponent.GetHashCode();
            return hashCode;
        }
    }
}