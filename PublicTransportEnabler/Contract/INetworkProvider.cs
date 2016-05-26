using System;
using System.Collections.Generic;
using PublicTransportEnabler.DataModel;
using PublicTransportEnabler.Enum;
using PublicTransportEnabler.Model;
using Location = PublicTransportEnabler.Model.Location;
using Point = PublicTransportEnabler.Model.Point;

namespace PublicTransportEnabler.Contract
{
	public interface INetworkProvider
	{
		NetworkId Id();

		bool HasCapabilities(IEnumerable<Capability> capabilities);

		OuterCoordInfoRequest QueryNearbyStations(Location location, int maxDistance, int maxStations);

		DepartureMonitorRequest QueryDepartures(int stationId, int maxDepartures, bool equivs);

		StopFinderRequest AutocompleteStations(string constraint);

		IEnumerable<Product> DefaultProducts();

		TripRequest QueryConnections(Location from, Location via, Location to, DateTime date, bool dep,
		                             int numConnections, List<Product> products, WalkSpeed walkSpeed,
		                             Accessibility accessibility,
		                             HashSet<Option> options);

		QueryConnectionsResult QueryMoreConnections(IQueryConnectionsContext context, bool later, int numConnections);

		Style LineStyle(String line);

		Point[] GetArea();
	}
}