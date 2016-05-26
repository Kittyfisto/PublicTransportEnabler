using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	public class Location : Coordinate
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlAttribute("stopName")]
		public string StopName { get; set; }

		[XmlAttribute("stopID")]
		public int StopId { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", StopId, StopName);
		}
	}
}