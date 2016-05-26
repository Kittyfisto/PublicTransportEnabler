using System;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdTime")]
	public class Time
	{
		[XmlAttribute("hour")]
		public int Hour { get; set; }

		[XmlAttribute("minute")]
		public int Minute { get; set; }

		public override string ToString()
		{
			return string.Format("{0}:{1}", Hour, Minute);
		}

		public TimeSpan ToTimeSpan()
		{
			return new TimeSpan(Hour, Minute, 0);
		}
	}
}