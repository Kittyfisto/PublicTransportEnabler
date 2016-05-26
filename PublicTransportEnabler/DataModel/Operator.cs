using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdOperator")]
	[DebuggerDisplay("{Code} - {Name}")]
	public class Operator
	{
		[XmlElement("code")]
		public string Code { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }
	}
}