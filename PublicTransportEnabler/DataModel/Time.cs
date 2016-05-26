using System;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/// <summary>
	///     Represents the time of day.
	/// </summary>
	[XmlType("itdTime")]
	public class Time
	{
		/// <summary>
		///     The hour starting at 0 up to 23.
		/// </summary>
		[XmlAttribute("hour")]
		public int Hour { get; set; }

		/// <summary>
		///     The minute since the beginning of the last hour, starting at 0.
		/// </summary>
		[XmlAttribute("minute")]
		public int Minute { get; set; }

		/// <summary>
		/// Converts this time into a <see cref="TimeSpan"/>.
		/// </summary>
		public TimeSpan TimeSpan
		{
			get { return new TimeSpan(Hour, Minute, 0); }
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0:D2}:{1:D2}", Hour, Minute);
		}
	}
}