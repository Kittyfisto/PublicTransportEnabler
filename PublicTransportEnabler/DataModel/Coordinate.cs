using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdCoord")]
	public class Coordinate
	{
		[XmlAttribute("x")]
		public double X { get; set; }

		[XmlAttribute("y")]
		public double Y { get; set; }

		[XmlAttribute("mapName")]
		public string MapName { get; set; }
	}
}