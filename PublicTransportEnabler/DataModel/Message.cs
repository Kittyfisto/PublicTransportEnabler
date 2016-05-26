using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdMessage")]
	[DebuggerDisplay("{Type} - {Module} - {Text}")]
	public class Message
	{
		//<itdMessage type="error" module="BROKER" code="-8011"></itdMessage>

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("module")]
		public string Module { get; set; }

		[XmlAttribute("code")]
		public int Code { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}