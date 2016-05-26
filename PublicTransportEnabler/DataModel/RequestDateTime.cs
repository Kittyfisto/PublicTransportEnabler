using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	//<itdDateTime ttpFrom="20121201" ttpTo="20131231">

	[XmlType("itdDateTime")]
	[DebuggerDisplay("{Date.Year}/{Date.Month}/{Date.Day} - {Time.Hour}:{Time.Minute}")]
	public class RequestDateTime
	{
		[XmlElement("itdDate")]
		public Date Date { get; set; }

		[XmlElement("itdTime")]
		public Time Time { get; set; }

		[XmlAttribute("ttpFrom")]
		public string From { get; set; }

		[XmlAttribute("ttpTo")]
		public string To { get; set; }

		public Range<DateTime?> Valid
		{
			get { return new Range<DateTime?>(ParseDateTime(From), ParseDateTime(To)); }
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