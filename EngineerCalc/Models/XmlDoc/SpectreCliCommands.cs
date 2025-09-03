using System.Xml.Serialization;

namespace EngineerCalc.Models.XmlDoc;

[Serializable]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false, ElementName = "Model")]
public class SpectreCliCommands
{
    [XmlElement("Command")]
    public required Command[] Commands { get; set; }
}
