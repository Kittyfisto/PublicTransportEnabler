using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("individualTransport")]
	[DebuggerDisplay("{MeansCode} - {Value} - {Selected}")]
	public class IndividualTransport
	{
		[XmlAttribute("meansCode")]
		public int MeansCode { get; set; }

		[XmlAttribute("value")]
		public int Value { get; set; }

		[XmlAttribute("speed")]
		public string Speed { get; set; }

		[XmlAttribute("selected")]
		public int Selected { get; set; }
	}
}