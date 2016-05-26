using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdUsedOptions")]
	public class UsedOptions
	{
		[XmlAttribute("calcNumberOfTrips")]
		public int CalcNumberOfTrips { get; set; }

		[XmlAttribute("dwellTime")]
		public string DwellTime { get; set; }

		[XmlAttribute("nextDepsPerLeg")]
		public int NextDepsPerLeg { get; set; }

		[XmlAttribute("calcCO2")]
		public int CalcCO2 { get; set; }

		[XmlAttribute("realtime")]
		public string Realtime { get; set; }

		[XmlAttribute("itemType")]
		public string ItemType { get; set; }
	}
}