using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("infoLink")]
	[DebuggerDisplay("{InfoText}")]
	public class InfoLink
	{
		[XmlArray("paramList")]
		[XmlArrayItem("param", typeof (Parameter))]
		public List<Parameter> ParamList { get; set; }

		[XmlElement("infoLinkText")]
		public string InfoLinkText { get; set; }

		[XmlElement("infoLinkURL")]
		public string InfoLinkURL { get; set; }

		[XmlElement("infoLinkImage")]
		public string InfoLinkImage { get; set; }

		[XmlElement("infoText")]
		public InfoText InfoText { get; set; }
	}
}