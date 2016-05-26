using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdCommuterFares")]
	public class CommuterFares : Ticket
	{
		[XmlAttribute("weekAdult")]
		public string WeekAdult { get; set; }

		[XmlAttribute("weekChild")]
		public string WeekChild { get; set; }

		[XmlAttribute("monthAdult")]
		public string MonthAdult { get; set; }

		[XmlAttribute("monthChild")]
		public string MonthChild { get; set; }

		[XmlAttribute("weekEducation")]
		public string WeekEducation { get; set; }

		[XmlAttribute("monthEducation")]
		public string MonthEducation { get; set; }

		[XmlAttribute("yearAdults")]
		public string YearAdults { get; set; }

		[XmlAttribute("yearChildren")]
		public string YearChildren { get; set; }

		[XmlAttribute("yearStudents")]
		public string YearStudents { get; set; }

		[XmlAttribute("dayAdults")]
		public string DayAdults { get; set; }

		[XmlAttribute("dayChildren")]
		public string DayChildren { get; set; }

		[XmlAttribute("dayStudents")]
		public string DayStudents { get; set; }

		[XmlAttribute("levelAdult")]
		public int LevelAdult { get; set; }

		[XmlAttribute("levelChild")]
		public int LevelChild { get; set; }
	}
}