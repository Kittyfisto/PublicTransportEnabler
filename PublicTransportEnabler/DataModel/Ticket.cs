using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	public class Ticket
	{
		[XmlAttribute("net")]
		public string Net { get; set; }

		[XmlAttribute("toPR")]
		public int ToPr { get; set; }

		[XmlAttribute("fromPR")]
		public int FromPr { get; set; }

		[XmlAttribute("currency")]
		public string Currency { get; set; }
	}
}