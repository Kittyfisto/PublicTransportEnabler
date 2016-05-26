using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/*
     *   <itdDepartureMonitorRequest requestID="0">
     */

	[XmlType("itdDepartureMonitorRequest")]
	public class DepartureMonitorRequest
	{
		[XmlAttribute("requestID")]
		public string RequestId { get; set; }

		[XmlElement("itdOdv")]
		public Odv Odv { get; set; }

		[XmlElement("itdDateTime")]
		public RequestDateTime DateTime { get; set; }

		[XmlElement("itdDMDateTime")]
		public DMDateTime DMDateTime { get; set; }

		[XmlArray("itdDateRange")]
		public Date[] DateRange { get; set; }

		[XmlElement("itdTripOptions")]
		public TripOptions TripOptions { get; set; }

		[XmlArray("itdServingLines")]
		public ServingLine[] ServingLines { get; set; }

		[XmlArray("itdDepartureList")]
		public Departure[] DepartureList { get; set; }
	}
}