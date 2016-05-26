using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/*
     * <itdOdv type="stop" usage="dm">
     */


	[XmlType("itdOdv")]
	[DebuggerDisplay("{OdvPlace.PlaceElements.Count} - {OdvName.NameElements.Count}")]
	public class Odv
	{
		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("usage")]
		public string Usage { get; set; }

		[XmlAttribute("anyObjFilter")]
		public string AnyObjFilter { get; set; }

		[XmlAttribute("regionID")]
		public int RegionId { get; set; }

		[XmlElement("itdOdvPlace")]
		public OdvPlace OdvPlace { get; set; }

		[XmlElement("itdOdvName")]
		public OdvName OdvName { get; set; }

		[XmlArray("itdOdvAssignedStops")]
		[XmlArrayItem("itdOdvAssignedStop")]
		public AssignedStopList AssignedStops { get; set; }

		[XmlArray("genAttrList")]
		[XmlArrayItem("genAttrElem", typeof (AttrElement))]
		public List<AttrElement> AttrList { get; set; }
	}
}