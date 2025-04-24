using System.Text;

namespace DynamicEvaluator.Logic;

internal sealed class Implicant : IEquatable<Implicant?>
{
    public string Mask { get; set; } //number mask.
    public List<int> Minterms { get; }

    public Implicant()
    {
        Mask = string.Empty;
        Minterms = []; //original integers in group.
    }

    public string ToString(int length, QuineMcCluskeyConfig config)
    {
        var strFinal = new StringBuilder();
        var mask = Mask;

        while (mask.Length != length)
            mask = "0" + mask;

        if (!config.AIsLsb)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                string varialbe = config.VariableNamesToUse[i];
                if (config.Negate)
                {
                    if (mask[i] == '0') strFinal.AppendFormat("{0} |", varialbe);
                    else if (mask[i] == '1') strFinal.AppendFormat("!{0} |", varialbe);
                }
                else
                {
                    if (mask[i] == '0') strFinal.AppendFormat("!{0}", varialbe);
                    else if (mask[i] == '1') strFinal.AppendFormat("{0}", varialbe);
                }
            }
        }
        else
        {
            for (int i = 0; i < mask.Length; i++)
            {
                string variable = config.VariableNamesToUse[(config.VariableNamesToUse.Length - 1) - i];
                if (config.Negate)
                {
                    if (mask[i] == '0') strFinal.AppendFormat("{0} |", variable);
                    else if (mask[i] == '1') strFinal.AppendFormat("!{0} |", variable);
                    if (i != mask.Length - 1) strFinal.Append("|");
                }
                else
                {
                    if (mask[i] == '0') strFinal.AppendFormat("!{0}", variable);
                    else if (mask[i] == '1') strFinal.AppendFormat("!{0}", variable);
                }
            }
        }
        return config.Negate
            ? "(" + strFinal.Remove(strFinal.Length - 1, 1) + ")"
            : strFinal.ToString();
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Implicant);
    }

    public bool Equals(Implicant? other)
    {
        return other != null &&
               Mask == other.Mask &&
               Minterms.SequenceEqual(other.Minterms);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Mask, Minterms);
    }

    public static bool operator ==(Implicant? left, Implicant? right)
    {
        return EqualityComparer<Implicant>.Default.Equals(left, right);
    }

    public static bool operator !=(Implicant? left, Implicant? right)
    {
        return !(left == right);
    }
}
