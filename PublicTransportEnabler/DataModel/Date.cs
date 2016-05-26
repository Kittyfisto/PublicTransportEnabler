using System;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/// <summary>
	/// Represents a date in the gregorian calendar.
	/// </summary>
	[XmlType("itdDate")]
	public class Date
	{
		/// <summary>
		/// The day of the month, starting at 1.
		/// </summary>
		[XmlAttribute("day")]
		public int Day { get; set; }

		/// <summary>
		/// The month of the year, starting at 1 (January).
		/// </summary>
		[XmlAttribute("month")]
		public int Month { get; set; }

		/// <summary>
		/// The year.
		/// </summary>
		[XmlAttribute("year")]
		public int Year { get; set; }

		/// <summary>
		/// The day of the week (e.g. monday, tuesday etc..) that this date represents.
		/// </summary>
		public DayOfWeek DayOfWeek
		{
			get { return DateTime.DayOfWeek; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0:D2}/{1:D2}/{2}", Day, Month, Year);
		}

		/// <summary>
		/// Converts this date into a <see cref="DateTime"/>.
		/// </summary>
		public DateTime DateTime
		{
			get{return new DateTime(Year, Month, Day);}
		}
	}
}