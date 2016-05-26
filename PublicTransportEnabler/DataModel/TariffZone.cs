using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdTariffzones")]
	public class TariffZone
	{
		[XmlAttribute("net")]
		public string Net { get; set; }

		[XmlAttribute("toPR")]
		public int ToPr { get; set; }

		[XmlAttribute("fromPR")]
		public int FromPr { get; set; }

		[XmlAttribute("neutralZone")]
		public string NeutralZone { get; set; }

		[XmlElement("itdZones")]
		public List<Zone> Zones { get; set; }
	}


	[XmlType("itdTariffzones")]
	public class TariffZoneList : List<TariffZone>
	{
	}
}