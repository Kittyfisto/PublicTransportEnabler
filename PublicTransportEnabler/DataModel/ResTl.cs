using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdResTL")]
	public class ResTl
	{
		[XmlAttribute("RVB")]
		public string RVB { get; set; }
	}
}