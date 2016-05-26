using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("mitCar")]
	public class Car : Pedestrian
	{
		[XmlAttribute("mitProfileData")]
		public bool MitProfileData { get; set; }

		[XmlAttribute("mitIncidentData")]
		public bool MitIncidentData { get; set; }

		[XmlAttribute("mitOnlineData")]
		public bool MitOnlineData { get; set; }

		[XmlAttribute("noHighway")]
		public bool NoHighway { get; set; }

		[XmlAttribute("noTollRoad")]
		public bool NoTollRoad { get; set; }
	}
}