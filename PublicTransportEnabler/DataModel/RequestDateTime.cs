using System;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/// <summary>
	/// I have no idea what this represents (is it the datetime the request was made? If so, why does it represent a range?)
	/// </summary>
	/// <example>
	/// itdDateTime ttpFrom="20121201" ttpTo="20131231"
	/// </example>
	[XmlType("itdDateTime")]
	public class RequestDateTime
	{
		/// <summary>
		/// 
		/// </summary>
		[XmlElement("itdDate")]
		public Date Date { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[XmlElement("itdTime")]
		public Time Time { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("ttpFrom")]
		public string From { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("ttpTo")]
		public string To { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Range<DateTime?> Valid
		{
			get { return new Range<DateTime?>(ParseDateTime(From), ParseDateTime(To)); }
		}

		/// <summary>
		/// 
		/// </summary>
		public DateTime DateTime
		{
			get { return new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, 0); }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0} - {1}",
			                     Date,
								 Time);
		}

		private DateTime? ParseDateTime(string input)
		{
			if (string.IsNullOrWhiteSpace(input) || input.Trim().Length < 8)
				return null;
			return new DateTime(Int32.Parse(input.Substring(0, 4)), Int32.Parse(input.Substring(4, 2)),
			                    Int32.Parse(input.Substring(6, 2)));
		}
	}
}