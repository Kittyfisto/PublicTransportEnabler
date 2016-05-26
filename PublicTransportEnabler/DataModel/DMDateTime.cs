using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdDMDateTime")]
	public class DMDateTime
	{
		[XmlAttribute("deparr")]
		public string Deparr { get; set; }
	}
}