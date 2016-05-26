using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdAddress")]
	public class Address
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("street")]
		public string Street { get; set; }

		[XmlAttribute("place")]
		public string Place { get; set; }

		[XmlAttribute("addressExt1")]
		public string AddressExtension1 { get; set; }

		[XmlAttribute("addressExt2")]
		public string AddressExtension2 { get; set; }
	}
}