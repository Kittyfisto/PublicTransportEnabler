using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using PublicTransportEnabler.DataModel;
using PublicTransportEnabler.Enum;
using PublicTransportEnabler.Model;
using Location = PublicTransportEnabler.Model.Location;
using Point = PublicTransportEnabler.Model.Point;

namespace PublicTransportEnabler
{
	public interface INetworkProvider
	{
		NetworkId Id();

		bool HasCapabilities(IEnumerable<Capability> capabilities);

		[Pure]
		Task<OuterCoordInfoRequest> QueryNearbyStationsAsync(Location location, int maxDistance, int maxStations);

		[Pure]
		Task<DepartureMonitorRequest> QueryDeparturesAsync(int stationId, int maxDepartures, bool equivs);

		[Pure]
		Task<StopFinderRequest> AutocompleteStationsAsync(string constraint);

		IEnumerable<Product> DefaultProducts();

		[Pure]
		Task<TripRequest> QueryConnectionsAsync(Location from, Location via, Location to, DateTime date, bool dep,
		                             int numConnections, List<Product> products, WalkSpeed walkSpeed,
		                             Accessibility accessibility,
		                             HashSet<Option> options);

		QueryConnectionsResult QueryMoreConnections(IQueryConnectionsContext context, bool later, int numConnections);

		Style LineStyle(String line);

		Point[] GetArea();
	}
}