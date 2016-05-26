using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[DebuggerDisplay("{StopId} - {StopName}")]
	public class Location : Coordinate
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlAttribute("stopName")]
		public string StopName { get; set; }

		[XmlAttribute("stopID")]
		public int StopId { get; set; }
	}
}