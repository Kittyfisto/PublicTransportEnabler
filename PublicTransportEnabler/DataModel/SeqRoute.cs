using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("seqRoute")]
	public class SeqRoute
	{
		[XmlAttribute("publicDuration")]
		public string PublicDuration { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}