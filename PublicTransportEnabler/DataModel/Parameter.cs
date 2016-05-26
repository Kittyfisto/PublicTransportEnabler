using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("param")]
	[DebuggerDisplay("{Name}")]
	public class Parameter
	{
		[XmlAttribute("edit")]
		public int Edit { get; set; }

		[XmlElement("type")]
		public string Type { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("value")]
		public string Value { get; set; }
	}
}