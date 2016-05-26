using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itBicycle")]
	public class Bicycle : Pedestrian
	{
		[XmlAttribute("preferAsphaltTracks")]
		public bool PreferAsphaltTracks { get; set; }

		[XmlAttribute("preferGreenTracks")]
		public bool PreferGreenTracks { get; set; }

		[XmlAttribute("usePseudoRouting")]
		public bool UsePseudoRouting { get; set; }

		[XmlAttribute("useSignedRoute")]
		public bool UseSignedRoute { get; set; }

		[XmlAttribute("prefHikePath")]
		public bool PrefHikePath { get; set; }

		[XmlAttribute("cycleSpeed")]
		public int CycleSpeed { get; set; }

		[XmlAttribute("elevFac")]
		public int ElevationFactor { get; set; }

		[XmlAttribute("bikeProf")]
		public string BikeProf { get; set; }
	}
}