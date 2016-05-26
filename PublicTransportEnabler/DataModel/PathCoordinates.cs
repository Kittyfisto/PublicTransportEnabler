using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdPathCoordinates")]
	public class PathCoordinates
	{
		[XmlElement("coordEllipsoid")]
		public string CoordEllipsoid { get; set; }

		[XmlElement("coordType")]
		public string CoordType { get; set; }

		[XmlElement("itdCoordinateString")]
		public CoordinateString CoordinateString { get; set; }
	}
}