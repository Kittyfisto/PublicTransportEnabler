using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdInfoLinkList")]
	[DebuggerDisplay("LineInfoList: {LineInfoList.Length}")]
	public class InfoLinkList
	{
		[XmlArray("itdLineInfoList")]
		public InfoLink[] LineInfoList { get; set; }
	}
}