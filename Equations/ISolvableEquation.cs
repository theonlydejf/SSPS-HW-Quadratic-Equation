using System;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    public interface ISolvableEquation
    {
        string Name { get; }
        VariableIdentifierCollection ResultingVariable { get; }
        VariableCollection[] Solve();
    }
}
