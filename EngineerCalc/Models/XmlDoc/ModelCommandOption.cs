//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

[Serializable]
[XmlType(AnonymousType = true)]
public class ModelCommandArgument
{
    public required string Description { get; set; }

    [XmlAttribute()]
    public required string Name { get; set; }

    [XmlAttribute()]
    public int Position { get; set; }

    [XmlAttribute()]
    public required bool Required { get; set; }

    [XmlAttribute()]
    public required string Kind { get; set; }

    [XmlAttribute()]
    public required string ClrType { get; set; }
}
