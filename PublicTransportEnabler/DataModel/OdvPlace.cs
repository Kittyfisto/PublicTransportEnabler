using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/*
     * <itdOdvPlace state="identified" method="itp">
     */


	[XmlType("itdOdvPlace")]
	[DebuggerDisplay("{PlaceElements.Count} - {State}")]
	public class OdvPlace
	{
		[XmlAttribute("state")]
		public string State { get; set; }

		[XmlAttribute("method")]
		public string Method { get; set; }

		[XmlAttribute("odvPlaceInput")]
		public string PlaceInput { get; set; }

		[XmlElement("odvPlaceElem")]
		public List<PlaceElement> PlaceElements { get; set; }
	}
}