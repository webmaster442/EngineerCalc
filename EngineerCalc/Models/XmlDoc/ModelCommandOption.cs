using System.Xml.Serialization;

namespace EngineerCalc.Models.XmlDoc;

[Serializable]
[XmlType(AnonymousType = true)]
public class ModelCommandOption
{
    public required string Description { get; set; }

    [XmlAttribute]
    public required string Short { get; set; }

    [XmlAttribute]
    public required string Long { get; set; }

    [XmlAttribute]
    public required string Value { get; set; }

    [XmlAttribute]
    public bool Required { get; set; }

    [XmlAttribute]
    public required string Kind { get; set; }

    [XmlAttribute]
    public required string ClrType { get; set; }
}