using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public struct VariableIdentifierCollection : IEnumerable<VariableIdentifier>
    {
        VariableIdentifier[] identifiers;

        public int Count { get => identifiers.Length; }

        public VariableIdentifierCollection(params VariableIdentifier[] identifiers)
        {
            this.identifiers = new VariableIdentifier[identifiers.Length];
            identifiers.CopyTo(this.identifiers, 0);
            Array.Sort(this.identifiers, (x, y) => ((string)x).CompareTo(y));
        }

        public VariableIdentifier this[int index]
        {
            get => identifiers[index];
        }

        public VariableIdentifierCollection Clone()
        {
            VariableIdentifier[] identifiers = new VariableIdentifier[this.identifiers.Length];
            this.identifiers.CopyTo(identifiers, 0);
            return new VariableIdentifierCollection(identifiers);
        }

        public char[] GetMarkers()
        {
            char[] _out = new char[identifiers.Length];
            for (int i = 0; i < identifiers.Length; i++)
            {
                _out[i] = identifiers[i].Marker;
            }
            return _out;
        }

        public double[] GetExponents()
        {
            double[] _out = new double[identifiers.Length];
            for (int i = 0; i < identifiers.Length; i++)
            {
                _out[i] = identifiers[i].Exponent;
            }
            return _out;
        }

        public bool HasSameMarkersAs(VariableIdentifierCollection identifiers)
        {
            if (Count != identifiers.Count)
                return false;

            for (int i = 0; i < Count; i++)
            {
                if (!this.identifiers[i].Marker.Equals(identifiers[i].Marker))
                    return false;
            }

            return true;
        }

        public bool ContainsMarker(char marker)
        {
            foreach (VariableIdentifier identifier in identifiers)
            {
                if (identifier.Marker == 'i')
                    return true;
            }
            return false;
        }

        public IEnumerator<VariableIdentifier> GetEnumerator()
        {
            IEnumerator enumerator = identifiers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (VariableIdentifier)enumerator.Current;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => identifiers.GetEnumerator();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (VariableIdentifier identifier in identifiers)
            {
                sb.Append(identifier);
            }
            return sb.ToString();
        }

        public static implicit operator string(VariableIdentifierCollection identifiers) => identifiers.ToString();

        public override bool Equals(object obj)
        {
            return obj.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return -1101011761 + EqualityComparer<VariableIdentifier[]>.Default.GetHashCode(identifiers);
        }
    }
}
