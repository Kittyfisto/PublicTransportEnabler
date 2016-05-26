using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	//  <odvNameElem x="6763414" y="51211657" mapName="WGS84" stopID="20018108" value="20018108:1" isTransferStop="0" matchQuality="100000">Bilker Kirche</odvNameElem>

	[XmlType("odvNameElem")]
	public class NameElement : Location
	{
		[XmlAttribute("value")]
		public string Value { get; set; }

		[XmlAttribute("isTransferStop")]
		public int IsTransferStop { get; set; }

		[XmlAttribute("matchQuality")]
		public int MatchQuality { get; set; }

		[XmlAttribute("listIndex")]
		public string ListIndex { get; set; }

		[XmlAttribute("selected")]
		public string Selected { get; set; }

		[XmlAttribute("anyType")]
		public string AnyType { get; set; }

		[XmlAttribute("omc")]
		public int Omc { get; set; }

		[XmlAttribute("placeID")]
		public int PlaceId { get; set; }

		[XmlAttribute("anyTypeSort")]
		public string AnyTypeSort { get; set; }

		[XmlAttribute("locality")]
		public string Locality { get; set; }

		[XmlAttribute("objectName")]
		public string ObjectName { get; set; }

		[XmlAttribute("buildingName")]
		public string BuildingName { get; set; }

		[XmlAttribute("buildingNumber")]
		public string BuildingNumber { get; set; }

		[XmlAttribute("postCode")]
		public string PostCode { get; set; }

		[XmlAttribute("streetName")]
		public string StreetName { get; set; }

		[XmlAttribute("nameKey")]
		public string NameKey { get; set; }

		[XmlAttribute("posttown")]
		public string Posttown { get; set; }

		[XmlText]
		public string Text
		{
			get { return StopName; }
			set { StopName = value; }
		}
	}
}