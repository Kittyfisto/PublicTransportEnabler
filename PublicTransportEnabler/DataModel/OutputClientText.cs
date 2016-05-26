using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("outputClientText")]
	public class OutputClientText
	{
		[XmlElement("htmlText")]
		public string Html { get; set; }

		[XmlElement("wmlText")]
		public string Wml { get; set; }

		[XmlElement("smsText")]
		public string Sms { get; set; }

		[XmlElement("speechText")]
		public string Speech { get; set; }
	}
}