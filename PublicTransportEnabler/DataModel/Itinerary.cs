using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdItinerary")]
	public class Itinerary
	{
		[XmlArray("itdRouteList")]
		[XmlArrayItem("itdRoute", typeof (Route))]
		public List<Route> RouteList { get; set; }
	}
}