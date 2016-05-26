using System.Collections.Generic;

namespace PublicTransportEnabler.Model
{
	public class Part
	{
		public Part(Location departure, Location arrival, List<Point> path)
		{
			Departure = departure;
			Arrival = arrival;
			Path = path;
		}

		public Location Departure { get; private set; }
		public Location Arrival { get; private set; }
		public List<Point> Path { get; private set; }
	}
}