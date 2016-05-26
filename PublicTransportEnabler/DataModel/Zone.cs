using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdZones")]
	public class Zone
	{
		[XmlAttribute("value")]
		public int Value { get; set; }

		[XmlAttribute("selected")]
		public int Selected { get; set; }

		[XmlElement("zoneElem")]
		public string ZoneElement { get; set; }
	}
}