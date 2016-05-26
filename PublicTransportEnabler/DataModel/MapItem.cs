using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdMapItem")]
	public class MapItem
	{
		[XmlAttribute("text")]
		public string Text { get; set; }

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlElement("itdImage")]
		public Image Image { get; set; }
	}
}