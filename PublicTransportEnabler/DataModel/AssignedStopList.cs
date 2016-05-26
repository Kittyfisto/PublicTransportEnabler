using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdOdvAssignedStops")]
	public class AssignedStopList : List<AssignedStop>
	{
		[XmlAttribute("select")]
		public int Select { get; set; }
	}
}