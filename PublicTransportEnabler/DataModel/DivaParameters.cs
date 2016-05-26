using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("motDivaParams")]
	public class DivaParameters
	{
		[XmlAttribute("project")]
		public string Project { get; set; }

		[XmlAttribute("direction")]
		public string Direction { get; set; }

		[XmlAttribute("supplement")]
		public string Supplement { get; set; }

		[XmlAttribute("network")]
		public string Network { get; set; }

		[XmlAttribute("line")]
		public string Line { get; set; }
	}
}