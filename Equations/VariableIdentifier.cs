using System;
using System.Collections.Generic;

namespace Equations
{
    public struct VariableIdentifier : IEquatable<VariableIdentifier>
    {
        public char Marker { get; }
        public double Exponent { get; }

        public VariableIdentifier(char marker, double exponent)
        {
            Marker = marker;
            Exponent = exponent;
        }

        public static implicit operator string(VariableIdentifier identifier) => identifier.ToString();

        public override bool Equals(object obj)
        {
            if (!(obj is VariableIdentifier))
            {
                return false;
            }

            var identifier = (VariableIdentifier)obj;
            return Marker == identifier.Marker && Exponent == identifier.Exponent;
        }

        public override int GetHashCode()
        {
            var hashCode = -599005067;
            hashCode = hashCode * -1521134295 + EqualityComparer<char?>.Default.GetHashCode(Marker);
            hashCode = hashCode * -1521134295 + Exponent.GetHashCode();
            return hashCode;
        }

        public override string ToString() => ToString(false);

        public string ToString(bool useUnicodeCharacters)
        {
            if (Exponent != 1)
            {
                if (!useUnicodeCharacters)
                    return Marker + "^" + Exponent.ToString().Replace(',', '.');
                else
                {
                    if ((Exponent % 1) == 0)
                        return Marker + ToStringHelper.IntToSuperscript((int)Exponent);
                    else
                        return "(" + Marker + "^" + Exponent.ToString().Replace(',', '.') + ")";
                }
            }
            return Marker.ToString();
        }

        public bool Equals(VariableIdentifier other)
        {
            return Marker == other.Marker && Exponent == other.Exponent;
        }
    }
}