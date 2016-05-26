using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdItOptions")]
	[DebuggerDisplay("DepartureTransport: {DepartureTransports.Length}")]
	public class ItOptions
	{
		[XmlElement("itRouter")]
		public Router Router { get; set; }

		[XmlElement("itPedestrian")]
		public Pedestrian Pedestrian { get; set; }

		[XmlElement("itBicycle")]
		public Bicycle Bicycle { get; set; }

		[XmlElement("mitCar")]
		public Car Car { get; set; }

		[XmlArray("departureTransport")]
		public IndividualTransport[] DepartureTransports { get; set; }

		[XmlArray("arrivalTransport")]
		public IndividualTransport[] ArrivalTransport { get; set; }
	}
}