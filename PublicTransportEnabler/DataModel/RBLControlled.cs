using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdRBLControlled")]
	public class RBLControlled
	{
		[XmlAttribute("delayMinutes")]
		public int DelayMinutes { get; set; }

		[XmlAttribute("delayMinutesArr")]
		public int DelayMinutesArrival { get; set; }
	}
}