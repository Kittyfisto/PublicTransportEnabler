using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("infoText")]
	[DebuggerDisplay("{Subject}")]
	public class InfoText
	{
		[XmlElement("content")]
		public string Content { get; set; }

		[XmlElement("subtitle")]
		public string Subtitle { get; set; }

		[XmlElement("subject")]
		public string Subject { get; set; }

		[XmlElement("additionalText")]
		public string AdditionalText { get; set; }

		[XmlElement("image")]
		public string Image { get; set; }

		[XmlElement("outputClientText")]
		public OutputClientText OutputClientText { get; set; }
	}
}