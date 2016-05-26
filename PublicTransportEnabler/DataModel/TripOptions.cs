using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdTripOptions")]
	public class TripOptions
	{
		[XmlElement("itdPtOptions")]
		public PtOptions PtOptions { get; set; }

		[XmlElement("itdItOptions")]
		public ItOptions ItOptions { get; set; }

		[XmlAttribute("userDefined")]
		public string UserDefined { get; set; }

		[XmlElement("itdUsedOptions")]
		public UsedOptions UsedOptions { get; set; }
	}
}