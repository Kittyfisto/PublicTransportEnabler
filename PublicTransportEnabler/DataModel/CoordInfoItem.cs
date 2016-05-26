using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("coordInfoItem")]
	public class CoordInfoItem
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlAttribute("placeID")]
		public int PlaceId { get; set; }

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("locality")]
		public string Locality { get; set; }

		[XmlAttribute("gisLayer")]
		public string GisLayer { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("addName")]
		public string AddName { get; set; }

		[XmlAttribute("stateless")]
		public int Stateless { get; set; }

		[XmlAttribute("gisID")]
		public int GisId { get; set; }

		[XmlAttribute("omc")]
		public int Omc { get; set; }

		[XmlAttribute("distance")]
		public int Distance { get; set; }

		[XmlElement("itdPathCoordinates")]
		public PathCoordinates PathCoordinates { get; set; }

		[XmlArray("genAttrList")]
		[XmlArrayItem("genAttrElem", typeof (AttrElement))]
		public List<AttrElement> AttrList { get; set; }
	}
}