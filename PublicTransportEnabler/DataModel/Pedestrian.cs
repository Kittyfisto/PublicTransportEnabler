using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itPedestrian")]
	public class Pedestrian
	{
		[XmlAttribute("computeMonomodalTrip")]
		public bool ComputeMonomodalTrip { get; set; }

		[XmlAttribute("computationType")]
		public string ComputationType { get; set; }

		[XmlAttribute("itIncidentData")]
		public bool ItIncidentData { get; set; }

		[XmlAttribute("useElevation")]
		public bool UseElevation { get; set; }

		[XmlAttribute("speedFactor")]
		public int SpeedFactor { get; set; }

		[XmlAttribute("costFactor")]
		public int CostFactor { get; set; }

		[XmlAttribute("distanceFactor")]
		public int DistanceFactor { get; set; }

		[XmlAttribute("traveltimeFactor")]
		public int TraveltimeFactor { get; set; }

		[XmlAttribute("noTunnel")]
		public bool NoTunnel { get; set; }

		[XmlAttribute("noBridge")]
		public bool NoBridge { get; set; }

		[XmlAttribute("noFerry")]
		public bool NoFerry { get; set; }

		[XmlAttribute("maxTime")]
		public int MaxTime { get; set; }

		[XmlAttribute("maxLength")]
		public int MaxLength { get; set; }

		[XmlAttribute("ignoreRestrictions")]
		public bool IgnoreRestrictions { get; set; }
	}
}