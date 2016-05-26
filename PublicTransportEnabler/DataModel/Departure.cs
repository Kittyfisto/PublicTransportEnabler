using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdDeparture")]
	public class Departure : Point
	{
		[XmlAttribute("countdown")]
		public int Countdown { get; set; }

		/*[XmlElement("itdDateTime")]
		public RequestDateTime DateTime { get; set; }*/

		[XmlElement("itdRTDateTime")]
		public RequestDateTime RtDateTime { get; set; }

		[XmlElement("itdServingLine")]
		public ServingLine ServingLine { get; set; }

		[XmlElement("itdInfoLinkList")]
		public InfoLinkList InfoLinkList { get; set; }

		[XmlElement("itdServingTrip")]
		public ServingTrip ServingTrip { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}, {2}", PlatformName, ServingLine, DateTime);
		}
	}
}