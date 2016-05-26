using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdNoTrain")]
	[DebuggerDisplay("{Name}")]
	public class NoTrain
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}
}