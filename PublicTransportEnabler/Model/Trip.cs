using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransportEnabler.Model
{
	public class Trip : Part
	{
		public Trip(Line line, Location destination, Stop departureStop, Stop arrivalStop,
		            List<Stop> intermediateStops, List<Point> path, string message)
			: base(
				departureStop != null ? departureStop.Location : null, arrivalStop != null ? arrivalStop.Location : null,
				path)
		{
			Line = line;
			Destination = destination;
			DepartureStop = departureStop;
			ArrivalStop = arrivalStop;
			IntermediateStops = intermediateStops;
			Message = message;
		}

		public Line Line { get; private set; }
		public Location Destination { get; private set; }
		public Stop DepartureStop { get; private set; }
		public Stop ArrivalStop { get; private set; }
		public List<Stop> IntermediateStops { get; private set; }
		public string Message { get; private set; }

		public DateTime GetDepartureTime()
		{
			DateTime? departureTime = DepartureStop.GetDepartureTime();

			if (!departureTime.HasValue)
				throw new ArgumentNullException("DepartureTime");

			return departureTime.Value;
		}

		public bool IsDepartureTimePredicted()
		{
			return DepartureStop.IsDepartureTimePredicted();
		}

		public TimeSpan? GetDepartureDelay()
		{
			return DepartureStop.GetDepartureDelay();
		}

		public string GetDeparturePosition()
		{
			return DepartureStop.GetDeparturePosition();
		}

		public bool IsDeparturePositionPredicted()
		{
			return DepartureStop.IsDeparturePositionPredicted();
		}

		public DateTime GetArrivalTime()
		{
			DateTime? arrivalTime = ArrivalStop.GetArrivalTime();

			if (arrivalTime == null)
				throw new ArgumentNullException("ArrivalTime");

			return arrivalTime.Value;
		}

		public bool IsArrivalTimePredicted()
		{
			return ArrivalStop.IsArrivalTimePredicted();
		}

		public TimeSpan? GetArrivalDelay()
		{
			return ArrivalStop.GetArrivalDelay();
		}

		public string GetArrivalPosition()
		{
			return ArrivalStop.GetArrivalPosition();
		}

		public bool IsArrivalPositionPredicted()
		{
			return ArrivalStop.IsArrivalPositionPredicted();
		}

		public override string ToString()
		{
			var builder = new StringBuilder(GetType().Name + "[");
			builder.Append("line=").Append(Line);
			if (Destination != null)
			{
				builder.Append(",");
				builder.Append("destination=").Append(Destination.ToDebugString());
			}
			builder.Append(",");
			builder.Append("departure=").Append(DepartureStop);
			builder.Append(",");
			builder.Append("arrival=").Append(ArrivalStop);
			builder.Append("]");
			return builder.ToString();
		}
	}
}