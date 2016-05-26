using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdImage")]
	public class Image
	{
		[XmlAttribute("src")]
		public string Source { get; set; }
	}
}