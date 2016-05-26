using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("coordInfoRequest")]
	public class CoordInfoRequest
	{
		[XmlAttribute("max")]
		public int Max { get; set; }

		[XmlAttribute("purpose")]
		public string Purpose { get; set; }

		[XmlElement("itdCoord")]
		public Coordinate Coordinate { get; set; }

		[XmlArray("coordInfoFilterItemList")]
		[XmlArrayItem("coordInfoFilterItem", typeof (CoordInfoFilterItem))]
		public List<CoordInfoFilterItem> CoordInfoItemList { get; set; }
	}
}