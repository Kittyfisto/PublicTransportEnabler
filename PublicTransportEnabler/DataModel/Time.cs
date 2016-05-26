using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdTime")]
	[DebuggerDisplay("{Hour}:{Minute}")]
	public class Time
	{
		[XmlAttribute("hour")]
		public int Hour { get; set; }

		[XmlAttribute("minute")]
		public int Minute { get; set; }

		public TimeSpan ToTimeSpan()
		{
			return new TimeSpan(Hour, Minute, 0);
		}
	}
}