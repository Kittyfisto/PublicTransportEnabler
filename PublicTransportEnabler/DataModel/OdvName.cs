using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/*
     * <itdOdvPlace state="identified" method="itp">
     */


	[XmlType("itdOdvName")]
	[DebuggerDisplay("{NameElements.Count} - {State}")]
	public class OdvName
	{
		[XmlAttribute("state")]
		public string State { get; set; }

		[XmlAttribute("method")]
		public string Method { get; set; }

		[XmlElement("odvNameInput")]
		public string NameInput { get; set; }

		[XmlElement("odvNameElem", typeof (NameElement))]
		public List<NameElement> NameElements { get; set; }

		[XmlElement("itdMessage")]
		public Message Message { get; set; }
	}
}