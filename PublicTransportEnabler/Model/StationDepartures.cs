using System.Collections.Generic;
using System.Text;

namespace PublicTransportEnabler.Model
{
	public class StationDepartures
	{
		public StationDepartures(Location location, List<Departure> departures, List<LineDestination> lines)
		{
			Location = location;
			Departures = departures;
			Lines = lines;
		}

		public Location Location { get; private set; }
		public List<Departure> Departures { get; private set; }
		public List<LineDestination> Lines { get; private set; }

		public override string ToString()
		{
			var builder = new StringBuilder(GetType().Name);
			builder.Append("[");
			if (Location != null)
				builder.Append(Location.ToDebugString());
			if (Departures != null)
				builder.Append(" ").Append(Departures.Count).Append(" departures");
			builder.Append("]");
			return builder.ToString();
		}
	}
}