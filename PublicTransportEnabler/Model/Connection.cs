using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PublicTransportEnabler.Model
{
	public class Connection : EquatableBase<Connection>
	{
		private string id;

		public Connection(string id, Location from, Location to, List<Part> parts, List<Fare> fares, int[] capacity,
		                  int numChanges)
		{
			this.id = id;
			this.from = from;
			this.to = to;
			this.parts = parts;
			this.fares = fares;
			this.capacity = capacity;
			this.numChanges = numChanges;
		}

		public Location from { get; private set; }
		public Location to { get; private set; }
		public List<Part> parts { get; private set; }
		public List<Fare> fares { get; private set; }
		public int[] capacity { get; private set; }
		public int numChanges { get; private set; }

		public DateTime? GetFirstDepartureTime()
		{
			if (parts != null)
			{
				int mins = 0;

				foreach (Part part in parts)
				{
					if (part is Footway)
						mins += ((Footway) part).Min;
					else if (part is Trip)
						return new DateTime(((Trip) part).GetDepartureTime().Subtract(TimeSpan.FromHours(1)).Ticks);
				}
			}

			return null;
		}

		public Trip GetFirstTrip()
		{
			if (parts != null)
				return parts.OfType<Trip>().FirstOrDefault();
			return null;
		}

		public DateTime? GetFirstTripDepartureTime()
		{
			Trip firstTrip = GetFirstTrip();
			if (firstTrip != null)
				return firstTrip.GetDepartureTime();

			return null;
		}

		public DateTime? GetLastArrivalTime()
		{
			if (parts != null)
			{
				int mins = 0;
				for (int i = parts.Count - 1; i >= 0; i--)
				{
					Part part = parts[i];
					if (part is Footway)
						mins += ((Footway) part).Min;
					else if (part is Trip)
						return new DateTime(((Trip) part).GetDepartureTime().Add(TimeSpan.FromHours(1)).Ticks);
				}
			}

			return null;
		}

		public Trip GetLastTrip()
		{
			if (parts != null)
			{
				for (int i = parts.Count - 1; i >= 0; i--)
				{
					Part part = parts[i];
					if (part is Trip)
						return (Trip) part;
				}
			}

			return null;
		}

		public DateTime? GetLastTripArrivalTime()
		{
			Trip lastTrip = GetLastTrip();
			if (lastTrip != null)
				return lastTrip.GetArrivalTime();

			return null;
		}

		public string GetId()
		{
			if (id == null)
				id = BuildSubstituteId();

			return id;
		}

		private string BuildSubstituteId()
		{
			var builder = new StringBuilder();

			if (parts != null && parts.Count > 0)
			{
				foreach (Part part in parts)
				{
					builder.Append(part.Departure.HasId()
						               ? part.Departure.Id
						               : part.Departure.Lat + '/' + part.Departure.Lon).Append('-');
					builder.Append(part.Arrival.HasId() ? part.Arrival.Id : part.Arrival.Lat + '/' + part.Arrival.Lon)
					       .Append('-');

					if (part is Footway)
					{
						builder.Append(((Footway) part).Min);
					}
					else if (part is Trip)
					{
						var trip = (Trip) part;
						builder.Append(trip.DepartureStop.PlannedDepartureTime.HasValue
							               ? trip.DepartureStop.PlannedDepartureTime.Value.ToString("d")
							               : string.Empty).Append('-');
						builder.Append(trip.ArrivalStop.PlannedArrivalTime.HasValue
							               ? trip.ArrivalStop.PlannedArrivalTime.Value.ToString("d")
							               : string.Empty).Append('-');
						builder.Append(trip.Line.Label);
					}

					builder.Append('|');
				}
			}
			return builder.ToString();
		}


		public override string ToString()
		{
			var str = new StringBuilder(GetId());
			str.Append(' ');
			DateTime? firstTripDepartureTime = GetFirstTripDepartureTime();
			str.Append(firstTripDepartureTime.HasValue ? firstTripDepartureTime.Value.ToString("E HH:mm") : "null");
			str.Append('-');
			DateTime? lastTripArrivalTime = GetLastTripArrivalTime();
			str.Append(lastTripArrivalTime.HasValue ? lastTripArrivalTime.Value.ToString("E HH:mm") : "null");
			str.Append(' ').Append(numChanges).Append("ch");

			return str.ToString();
		}


		public override bool Equals(Connection other)
		{
			return GetId().Equals(other.GetId());
		}

		public override int InstanceGetHashCode()
		{
			return GetId().GetHashCode();
		}
	}
}