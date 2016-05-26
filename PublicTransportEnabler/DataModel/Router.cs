using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itRouter")]
	public class Router
	{
		[XmlAttribute("logASCII")]
		public bool LogAscii { get; set; }

		[XmlAttribute("logSVG")]
		public bool LogSVG { get; set; }
	}
}