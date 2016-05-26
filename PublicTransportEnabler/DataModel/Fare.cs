using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdFare")]
	public class Fare
	{
		[XmlElement("itdSingleTicket")]
		public SingleTicket SingleTicket { get; set; }

		[XmlElement("itdCommuterFares")]
		public CommuterFares CommuterFares { get; set; }

		[XmlElement("itdTariffzones")]
		public TariffZone TariffZone { get; set; }
	}
}