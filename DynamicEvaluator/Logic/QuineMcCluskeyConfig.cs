namespace DynamicEvaluator.Logic;

internal sealed class QuineMcCluskeyConfig
{
    /// <summary>
    /// If set, returns hazard free version of the expression
    /// </summary>
    public bool HazardFree { get; set; }

    /// <summary>
    /// If set, A variable is treated as the least significant
    /// </summary>
    public bool AIsLsb { get; set; }

    /// <summary>
    /// Negate the result expresion or not
    /// </summary>
    public bool Negate { get; set; }

    /// <summary>
    /// Variable names to use
    /// </summary>
    public string[] VariableNamesToUse { get; set; }

    public QuineMcCluskeyConfig() 
    {
        VariableNamesToUse = new string[26];
        for (int i =0; i< VariableNamesToUse.Length; i++)
        {
            VariableNamesToUse[i] = $"{Convert.ToChar(65 + i)}";
        }
    }
}
