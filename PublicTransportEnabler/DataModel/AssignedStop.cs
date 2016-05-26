using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdOdvAssignedStops")]
	public class AssignedStop : Location
	{
		[XmlAttribute("value")]
		public string Value { get; set; }

		[XmlAttribute("place")]
		public string Place { get; set; }

		[XmlAttribute("nameWithPlace")]
		public string NameWithPlace { get; set; }

		[XmlAttribute("distanceTime")]
		public int DistanceTime { get; set; }

		[XmlAttribute("isTransferStop")]
		public int IsTransferStop { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}