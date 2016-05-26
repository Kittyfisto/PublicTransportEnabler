using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdCoordInfo")]
	public class CoordInfo
	{
		[XmlElement("coordInfoRequest")]
		public CoordInfoRequest CoordInfoRequest { get; set; }

		[XmlArray("coordInfoItemList")]
		[XmlArrayItem("coordInfoItem", typeof (CoordInfoItem))]
		public List<CoordInfoItem> CoordInfoItemList { get; set; }
	}
}