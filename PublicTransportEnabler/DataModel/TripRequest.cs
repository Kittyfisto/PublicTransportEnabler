using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdTripRequest")]
	public class TripRequest
	{
		[XmlAttribute("requestID")]
		public int RequestId { get; set; }

		[XmlElement("itdPrintConfiguration")]
		public PrintConfiguration PrintConfiguration { get; set; }

		[XmlElement("itdAddress")]
		public Address Address { get; set; }

		[XmlElement("itdOdv", typeof (Odv))]
		public List<Odv> Odvs { get; set; }

		[XmlElement("itdTripDateTime")]
		public TripDateTime TripDateTime { get; set; }

		[XmlElement("itdTripOptions")]
		public TripOptions TripOptions { get; set; }

		[XmlElement("itdItinerary")]
		public Itinerary Itinerary { get; set; }
	}
}