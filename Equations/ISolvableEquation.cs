using System;
using System.Collections.Generic;
using System.Text;

namespace Equations
{
    interface ISolvableEquation
    {
        VariableCollection[] Solve();
    }
}
