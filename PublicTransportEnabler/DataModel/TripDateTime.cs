using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdTripDateTime")]
	public class TripDateTime : RequestDateTime
	{
		[XmlAttribute("deparr")]
		public string Deparr { get; set; }

		[XmlElement("itdDateTime")]
		public RequestDateTime DateTime { get; set; }

		[XmlArray("itdDateRange")]
		public Date[] DateRange { get; set; }
	}
}