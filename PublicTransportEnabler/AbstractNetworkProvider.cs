using System;
using System.Collections.Generic;
using System.Linq;
using PublicTransportEnabler.Contract;
using PublicTransportEnabler.DataModel;
using PublicTransportEnabler.Enum;
using PublicTransportEnabler.Model;
using PublicTransportEnabler.Util;
using Location = PublicTransportEnabler.Model.Location;
using Point = PublicTransportEnabler.Model.Point;

namespace PublicTransportEnabler
{
	public abstract class AbstractNetworkProvider : INetworkProvider
	{
		protected static HashSet<Product> ALL_EXCEPT_HIGHSPEED;

		static AbstractNetworkProvider()
		{
			ALL_EXCEPT_HIGHSPEED = new HashSet<Product>(ProductHelper.ALL);
			ALL_EXCEPT_HIGHSPEED.Remove(Product.HIGH_SPEED_TRAIN);
		}

		public abstract NetworkId Id();
		public abstract bool HasCapabilities(IEnumerable<Capability> capabilities);
		public abstract OuterCoordInfoRequest QueryNearbyStations(Location location, int maxDistance, int maxStations);
		public abstract DepartureMonitorRequest QueryDepartures(int stationId, int maxDepartures, bool equivs);
		public abstract StopFinderRequest AutocompleteStations(string constraint);

		public IEnumerable<Product> DefaultProducts()
		{
			return ALL_EXCEPT_HIGHSPEED.ToList();
		}

		public abstract TripRequest QueryConnections(Location from, Location via, Location to, DateTime date, bool dep,
		                                             int numConnections, List<Product> products, WalkSpeed walkSpeed,
		                                             Accessibility accessibility,
		                                             HashSet<Option> options);

		public abstract QueryConnectionsResult QueryMoreConnections(IQueryConnectionsContext context, bool later,
		                                                            int numConnections);

		public virtual Style LineStyle(string line)
		{
			if (line.Length == 0)
				return null;

			return StandardColors.LINES[line[0]];
		}

		public Point[] GetArea()
		{
			return null;
		}
	}
}