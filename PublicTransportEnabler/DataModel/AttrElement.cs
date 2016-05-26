using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("genAttrElem")]
	public class AttrElement
	{
		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("value")]
		public string Value { get; set; }
	}
}