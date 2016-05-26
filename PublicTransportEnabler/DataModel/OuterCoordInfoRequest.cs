using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdCoordInfoRequest")]
	public class OuterCoordInfoRequest
	{
		[XmlElement("itdCoordInfo")]
		public CoordInfo CoordInfo { get; set; }

		[XmlAttribute("requestID")]
		public int RequestId { get; set; }
	}
}