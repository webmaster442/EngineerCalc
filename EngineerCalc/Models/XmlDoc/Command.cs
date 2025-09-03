using System.Xml.Serialization;

namespace EngineerCalc.Models.XmlDoc;

[Serializable]
[XmlType(AnonymousType = true)]
public class Command
{
    public required string Description { get; set; }

    [XmlArrayItem("Option", IsNullable = false)]
    public required ModelCommandOption[] Parameters { get; set; }

    [XmlAttribute]
    public required string Name { get; set; }

    [XmlAttribute]
    public required bool IsBranch { get; set; }

    [XmlAttribute]
    public required string ClrType { get; set; }

    [XmlAttribute]
    public required string Settings { get; set; }
}
