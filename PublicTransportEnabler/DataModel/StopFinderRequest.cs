using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdStopFinderRequest")]
	[DebuggerDisplay("{RequestId}")]
	public class StopFinderRequest
	{
		[XmlAttribute("requestID")]
		public int RequestId { get; set; }

		[XmlElement("itdOdv")]
		public Odv Odv { get; set; }

		[XmlElement("itdDateTime")]
		public RequestDateTime DateTime { get; set; }
	}
}