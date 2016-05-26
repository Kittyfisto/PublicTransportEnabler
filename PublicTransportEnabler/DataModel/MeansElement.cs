using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("meansElem")]
	[DebuggerDisplay("Value: {Value} - {Text}")]
	public class MeansElement
	{
		[XmlAttribute("value")]
		public int Value { get; set; }

		[XmlAttribute("selected")]
		public int Selected { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}