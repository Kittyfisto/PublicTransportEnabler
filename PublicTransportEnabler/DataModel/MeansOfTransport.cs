using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdMeansOfTransport")]
	public class MeansOfTransport : ServingLine
	{
		[XmlAttribute("shortname")]
		public string Shortname { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("productName")]
		public string ProductName { get; set; }

		[XmlAttribute("destination")]
		public string Destination { get; set; }

		[XmlAttribute("tC")]
		public string Tc { get; set; }
	}
}