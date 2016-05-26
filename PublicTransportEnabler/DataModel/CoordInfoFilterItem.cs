using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("coordInfoFilterItem")]
	public class CoordInfoFilterItem
	{
		[XmlAttribute("radius")]
		public int Radius { get; set; }

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("inclDrawClasses")]
		public string InclDrawClasses { get; set; }

		[XmlAttribute("exclLayers")]
		public string ExclLayers { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("ratingMethod")]
		public string RatingMethod { get; set; }

		[XmlAttribute("inclPOIHierarchy")]
		public string InclPOIHierarchy { get; set; }
	}
}