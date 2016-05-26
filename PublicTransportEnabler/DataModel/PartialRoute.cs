using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdPartialRoute")]
	public class PartialRoute
	{
		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("active")]
		public int Active { get; set; }

		[XmlAttribute("timeMinute")]
		public int TimeMinute { get; set; }

		[XmlAttribute("bookingCode")]
		public string BookingCode { get; set; }

		[XmlAttribute("partialRouteType")]
		public string PartialRouteType { get; set; }

		[XmlElement("itdPoint")]
		public List<Point> Points { get; set; }

		[XmlElement("itdMeansOfTransport")]
		public MeansOfTransport MeansOfTransport { get; set; }

		[XmlElement("itdRBLControlled")]
		public RBLControlled RBLControlled { get; set; }

		[XmlArray("itdStopSeq")]
		[XmlArrayItem("itdPoint", typeof (Point))]
		public List<Point> StopSequence { get; set; }

		[XmlElement("infoLink")]
		public InfoLink InfoLink { get; set; }

		[XmlElement("itdPathCoordinates")]
		public PathCoordinates PathCoordinates { get; set; }

		[XmlArray("nextDeps")]
		[XmlArrayItem("itdDateTime", typeof (RequestDateTime))]
		public List<RequestDateTime> NextDepartures { get; set; }
	}
}