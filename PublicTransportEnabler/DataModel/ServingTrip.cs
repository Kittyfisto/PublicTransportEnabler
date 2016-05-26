using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdServingTrip")]
	[DebuggerDisplay("{TripCode} - {AVMSTripId}")]
	public class ServingTrip
	{
		[XmlAttribute("tripCode")]
		public int TripCode { get; set; }

		[XmlAttribute("AVMSTripID")]
		public string AVMSTripId { get; set; }
	}
}