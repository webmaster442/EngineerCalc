﻿using System.Globalization;

using DynamicEvaluator;

namespace EngineerCalc.Endpoints;

public class State
{
    public VariablesAndConstantsCollection Variables { get; }

    public CultureInfo Culture { get; set; }

    public State()
    {
        Variables = new VariablesAndConstantsCollection();
        Culture = CultureInfo.InvariantCulture;
    }
}
