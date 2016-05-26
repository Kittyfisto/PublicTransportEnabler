using System.Collections.Generic;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdSingleTicket")]
	public class SingleTicket : Ticket
	{
		[XmlAttribute("unitName")]
		public string UnitName { get; set; }

		[XmlAttribute("fareAdult")]
		public decimal FareAdult { get; set; }

		[XmlAttribute("fareChild")]
		public decimal FareChild { get; set; }

		[XmlAttribute("unitsAdult")]
		public string UnitsAdult { get; set; }

		[XmlAttribute("unitsChild")]
		public string UnitsChild { get; set; }

		[XmlAttribute("fareBikeAdult")]
		public decimal FareBikeAdult { get; set; }

		[XmlAttribute("fareBikeChild")]
		public decimal FareBikeChild { get; set; }

		[XmlAttribute("unitsBikeAdult")]
		public string UnitsBikeAdult { get; set; }

		[XmlAttribute("unitsBikeChild")]
		public string UnitsBikeChild { get; set; }

		[XmlAttribute("levelAdult")]
		public string LevelAdult { get; set; }

		[XmlAttribute("levelChild")]
		public string LevelChild { get; set; }

		[XmlAttribute("idAdult")]
		public string IdAdult { get; set; }

		[XmlAttribute("idChild")]
		public string IdChild { get; set; }

		[XmlArray("itdGenericTicketList")]
		[XmlArrayItem("itdGenericTicketGroup", typeof (GenericTicketGroup))]
		public List<GenericTicketGroup> GenericTicketList { get; set; }
	}
}