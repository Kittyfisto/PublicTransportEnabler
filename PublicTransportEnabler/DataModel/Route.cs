using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdRoute")]
	public class Route
	{
		[XmlAttribute("active")]
		public int Active { get; set; }

		[XmlAttribute("selected")]
		public int Selected { get; set; }

		[XmlAttribute("changes")]
		public int Changes { get; set; }

		[XmlAttribute("distance")]
		public int Distance { get; set; }

		[XmlAttribute("routeIndex")]
		public int RouteIndex { get; set; }

		[XmlAttribute("routeTripIndex")]
		public int RouteTripIndex { get; set; }

		[XmlAttribute("alternative")]
		public int Alternative { get; set; }

		[XmlAttribute("print")]
		public int Print { get; set; }

		[XmlAttribute("delete")]
		public int Delete { get; set; }

		[XmlAttribute("searchMode")]
		public int SearchMode { get; set; }

		[XmlAttribute("cTime")]
		public string CTime { get; set; }

		[XmlAttribute("method")]
		public string Method { get; set; }

		[XmlAttribute("vehicleTime")]
		public int VehicleTime { get; set; }

		[XmlAttribute("publicDuration")]
		public string PublicDuration { get; set; }

		[XmlArray("itdMapItemList")]
		[XmlArrayItem("itdMapItem", typeof (MapItem))]
		public List<MapItem> MapItemList { get; set; }

		[XmlArray("itdPartialRouteList")]
		[XmlArrayItem("itdPartialRoute", typeof (PartialRoute))]
		public List<PartialRoute> PartialRouteList { get; set; }

		[XmlElement("itdFare")]
		public Fare Fare { get; set; }

		[XmlElement("itdDaysOfService")]
		public DaysOfService DaysOfService { get; set; }

		[XmlElement("itdResTL")]
		public ResTl ResTl { get; set; }

		[XmlArray("seqRoutes")]
		[XmlArrayItem("seqRoute", typeof (SeqRoute))]
		public List<SeqRoute> SeqRoutes { get; set; }
	}
}