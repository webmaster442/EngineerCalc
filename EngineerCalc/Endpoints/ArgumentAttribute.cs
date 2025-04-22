namespace EngineerCalc.Endpoints;

[AttributeUsage(AttributeTargets.Property)]
internal class ArgumentAttribute : Attribute
{
    public ArgumentAttribute(int index, string description)
    {
        Index = index;
        Description = description;
    }

    public int Index { get; }
    public string Description { get; }
}
