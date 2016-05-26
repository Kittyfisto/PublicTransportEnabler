using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdPoint")]
	public class Point : Location
	{
		[XmlAttribute("area")]
		public string Area { get; set; }

		[XmlAttribute("platform")]
		public string Platform { get; set; }

		[XmlAttribute("platformName")]
		public string PlatformName { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("nameWO")]
		public string NameWo { get; set; }

		[XmlAttribute("usage")]
		public string Usage { get; set; }

		[XmlAttribute("placeID")]
		public int PlaceId { get; set; }

		[XmlAttribute("omc")]
		public int Omc { get; set; }

		[XmlAttribute("locality")]
		public string Locality { get; set; }

		[XmlArray("itdMapItemList")]
		[XmlArrayItem("itdMapItem", typeof (MapItem))]
		public List<MapItem> MapItemList { get; set; }

		[XmlElement("itdDateTime")]
		public RequestDateTime DateTime { get; set; }

		[XmlElement("itdDateTimeTarget")]
		public RequestDateTime DateTimeTarget { get; set; }
	}
}