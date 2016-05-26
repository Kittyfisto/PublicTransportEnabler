using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdGenericTicketGroup")]
	public class GenericTicketGroup
	{
		[XmlElement("itdGenericTicket")]
		public List<GenericTicket> GenericTickets { get; set; }
	}
}