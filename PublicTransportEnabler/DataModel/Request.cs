using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	/*
     * <itdRequest 
     * version="9.16.29.17-UNITTEST" 
     * language="de" 
     * lengthUnit="METER" 
     * sessionID="0" 
     * client="NC6" 
     * clientIP="176.199.167.68" 
     * serverID="efamobil1.vrr.de_" 
     * virtDir="standard" 
     * now="2013-04-28T14:40:28" 
     * nowWD="1">
     */

	[XmlType("itdRequest")]
	[DebuggerDisplay("{Version} - {Client} - {LengthUnit} - {SessionId}")]
	public class Request
	{
		[XmlAttribute("version")]
		public string Version { get; set; }

		[XmlAttribute("language")]
		public string Language { get; set; }

		[XmlAttribute("lengthUnit")]
		public string LengthUnit { get; set; }

		[XmlAttribute("sessionID")]
		public string SessionId { get; set; }

		[XmlAttribute("client")]
		public string Client { get; set; }

		[XmlAttribute("clientIP")]
		public string ClientIp { get; set; }

		[XmlAttribute("serverID")]
		public string ServerId { get; set; }

		[XmlAttribute("now")]
		public DateTime Now { get; set; }

		[XmlIgnore]
		public DayOfWeek WeekDay
		{
			get { return Now.DayOfWeek; }
		}

		[XmlElement("itdDepartureMonitorRequest")]
		public DepartureMonitorRequest DepartureMonitorRequest { get; set; }

		[XmlElement("itdStopFinderRequest")]
		public StopFinderRequest StopFinderRequest { get; set; }

		[XmlElement("itdCoordInfoRequest")]
		public OuterCoordInfoRequest CoordInfoRequest { get; set; }

		[XmlElement("itdTripRequest")]
		public TripRequest TripRequest { get; set; }
	}
}