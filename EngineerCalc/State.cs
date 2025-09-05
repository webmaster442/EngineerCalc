using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerCalc;
internal sealed class State
{
    public CultureInfo Culture { get; set; }

    public State()
    {
        Culture = CultureInfo.CurrentUICulture;
    }
}
