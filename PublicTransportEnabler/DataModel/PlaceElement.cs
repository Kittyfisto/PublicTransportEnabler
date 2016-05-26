using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	//  omc="5111000" placeID="6" value="5111000:6" span="0" type="remote" mainPlace="1">Düsseldorf</odvPlaceElem>

	[XmlType("odvPlaceElem")]
	[DebuggerDisplay("{PlaceId} - {Text}")]
	public class PlaceElement
	{
		[XmlAttribute("omc")]
		public int Omc { get; set; }

		[XmlAttribute("placeID")]
		public int PlaceId { get; set; }

		[XmlAttribute("value")]
		public string Value { get; set; }

		[XmlAttribute("span")]
		public int Span { get; set; }

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("mainPlace")]
		public int Mainplace { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}