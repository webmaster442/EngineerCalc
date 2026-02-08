//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Logic;

internal sealed class ImplicantRelationship
{
    public Implicant A { get; set; }
    public Implicant B { get; set; }

    public ImplicantRelationship(Implicant first, Implicant second)
    {
        A = first;
        B = second;
    }

    public bool Contains(Implicant implicant)
    {
        return A == implicant
            || B == implicant;
    }
}
