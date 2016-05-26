using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdDate")]
	[DebuggerDisplay("{Year} - {Month} - {Day}")]
	public class Date
	{
		[XmlAttribute("day")]
		public int Day { get; set; }

		[XmlAttribute("month")]
		public int Month { get; set; }

		[XmlAttribute("year")]
		public int Year { get; set; }

		public DayOfWeek WeekDay
		{
			get { return ToDateTime().DayOfWeek; }
		}

		public DateTime ToDateTime()
		{
			return new DateTime(Year, Month, Day);
		}
	}
}