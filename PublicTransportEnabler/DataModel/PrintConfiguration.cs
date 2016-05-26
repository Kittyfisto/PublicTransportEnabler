using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdPrintConfiguration")]
	public class PrintConfiguration
	{
		[XmlAttribute("active")]
		public string Active { get; set; }

		[XmlAttribute("printerDirect")]
		public int PrinterDirect { get; set; }

		[XmlAttribute("layout")]
		public string Layout { get; set; }

		[XmlAttribute("outputFormat")]
		public string OutputFormat { get; set; }

		[XmlAttribute("fontSizeNormal")]
		public int FontSizeNormal { get; set; }

		[XmlAttribute("shading")]
		public int Shading { get; set; }

		[XmlAttribute("commuterFaresOutput")]
		public int CommuterFaresOutput { get; set; }

		[XmlAttribute("fareUnitOrientation")]
		public string FareUnitOrientation { get; set; }

		[XmlAttribute("serverAddress")]
		public string ServerAddress { get; set; }

		[XmlAttribute("serverInfo")]
		public string ServerInfo { get; set; }
	}
}