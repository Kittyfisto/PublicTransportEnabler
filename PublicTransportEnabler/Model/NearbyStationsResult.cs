using System.Collections.Generic;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class NearbyStationsResult
	{
		public NearbyStationsResult(ResultHeader header, Status status, List<Location> stations)
		{
			Header = header;
			Status = status;
			Stations = stations;
		}

		public NearbyStationsResult(ResultHeader header, List<Location> stations)
			: this(header, Status.OK, stations)
		{
		}

		public NearbyStationsResult(ResultHeader header, Status status)
			: this(header, status, null)
		{
		}

		public ResultHeader Header { get; set; }
		public Status Status { get; set; }
		public List<Location> Stations { get; set; }
	}
}