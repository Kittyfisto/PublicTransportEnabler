using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdGenericTicket")]
	public class GenericTicket
	{
		[XmlElement("ticket")]
		public string Ticket { get; set; }

		[XmlElement("value")]
		public string Value { get; set; }
	}
}