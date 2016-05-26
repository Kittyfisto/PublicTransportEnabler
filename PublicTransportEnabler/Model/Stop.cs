using System;
using System.Text;

namespace PublicTransportEnabler.Model
{
	public class Stop
	{
		public Stop(Location location, DateTime plannedArrivalTime, DateTime predictedArrivalTime,
		            string plannedArrivalPosition,
		            string predictedArrivalPosition, DateTime plannedDepartureTime, DateTime predictedDepartureTime,
		            string plannedDeparturePosition, string predictedDeparturePosition)
		{
			Location = location;
			PlannedArrivalTime = plannedArrivalTime;
			PredictedArrivalTime = predictedArrivalTime;
			PlannedArrivalPosition = plannedArrivalPosition;
			PredictedArrivalPosition = predictedArrivalPosition;
			PlannedDepartureTime = plannedDepartureTime;
			PredictedDepartureTime = predictedDepartureTime;
			PlannedDeparturePosition = plannedDeparturePosition;
			PredictedDeparturePosition = predictedDeparturePosition;
		}

		public Stop(Location location, bool departure, DateTime plannedTime, DateTime predictedTime,
		            string plannedPosition,
		            string predictedPosition)
		{
			Location = location;
			PlannedArrivalTime = !departure ? plannedTime : (DateTime?) null;
			PredictedArrivalTime = !departure ? predictedTime : (DateTime?) null;
			PlannedArrivalPosition = !departure ? plannedPosition : null;
			PredictedArrivalPosition = !departure ? predictedPosition : null;
			PlannedDepartureTime = departure ? plannedTime : (DateTime?) null;
			PredictedDepartureTime = departure ? predictedTime : (DateTime?) null;
			PlannedDeparturePosition = departure ? plannedPosition : null;
			PredictedDeparturePosition = departure ? predictedPosition : null;
		}

		public Stop(Location location, DateTime plannedArrivalTime, string plannedArrivalPosition,
		            DateTime plannedDepartureTime,
		            string plannedDeparturePosition)
		{
			Location = location;
			PlannedArrivalTime = plannedArrivalTime;
			PredictedArrivalTime = null;
			PlannedArrivalPosition = plannedArrivalPosition;
			PredictedArrivalPosition = null;
			PlannedDepartureTime = plannedDepartureTime;
			PredictedDepartureTime = null;
			PlannedDeparturePosition = plannedDeparturePosition;
			PredictedDeparturePosition = null;
		}

		public Location Location { get; private set; }
		public DateTime? PlannedArrivalTime { get; private set; }
		public DateTime? PredictedArrivalTime { get; private set; }
		public string PlannedArrivalPosition { get; private set; }
		public string PredictedArrivalPosition { get; private set; }
		public DateTime? PlannedDepartureTime { get; private set; }
		public DateTime? PredictedDepartureTime { get; private set; }
		public string PlannedDeparturePosition { get; private set; }
		public string PredictedDeparturePosition { get; private set; }

		public DateTime? GetArrivalTime()
		{
			if (PredictedArrivalTime.HasValue)
				return PredictedArrivalTime.Value;

			if (PlannedArrivalTime.HasValue)
				return PlannedArrivalTime.Value;

			return null;
		}

		public bool IsArrivalTimePredicted()
		{
			return PredictedArrivalTime.HasValue;
		}

		public TimeSpan? GetArrivalDelay()
		{
			if (PlannedArrivalTime.HasValue && PredictedArrivalTime.HasValue)
				return PredictedArrivalTime.Value - PlannedArrivalTime.Value;
			return null;
		}

		public string GetArrivalPosition()
		{
			if (PredictedArrivalPosition != null)
				return PredictedArrivalPosition;

			if (PlannedArrivalPosition != null)
				return PlannedArrivalPosition;

			return null;
		}

		public bool IsArrivalPositionPredicted()
		{
			return PredictedArrivalPosition != null;
		}

		public DateTime? GetDepartureTime()
		{
			if (PredictedDepartureTime.HasValue)
				return PredictedDepartureTime.Value;

			if (PlannedDepartureTime.HasValue)
				return PlannedDepartureTime.Value;

			return null;
		}

		public bool IsDepartureTimePredicted()
		{
			return PredictedDepartureTime.HasValue;
		}

		public TimeSpan? GetDepartureDelay()
		{
			if (PlannedDepartureTime.HasValue && PredictedDepartureTime.HasValue)
				return PredictedDepartureTime.Value - PlannedDepartureTime.Value;

			return null;
		}

		public string GetDeparturePosition()
		{
			if (PredictedDeparturePosition != null)
				return PredictedDeparturePosition;

			if (PlannedDeparturePosition != null)
				return PlannedDeparturePosition;

			return null;
		}

		public bool IsDeparturePositionPredicted()
		{
			return PredictedDeparturePosition != null;
		}


		public override string ToString()
		{
			var builder = new StringBuilder("Stop('");
			builder.Append(Location);
			builder.Append("', arr: ");
			builder.Append(PlannedArrivalTime.HasValue ? PlannedArrivalTime.Value.ToString("d") : "-");
			builder.Append("/");
			builder.Append(PredictedArrivalTime.HasValue ? PredictedArrivalTime.Value.ToString("d") : "-");
			builder.Append(", ");
			builder.Append(PlannedArrivalPosition ?? "-");
			builder.Append("/");
			builder.Append(PredictedArrivalPosition ?? "-");
			builder.Append(", dep: ");
			builder.Append(PlannedDepartureTime.HasValue ? PlannedDepartureTime.Value.ToString("d") : "-");
			builder.Append("/");
			builder.Append(PredictedDepartureTime.HasValue ? PredictedDepartureTime.Value.ToString("d") : "-");
			builder.Append(", ");
			builder.Append(PlannedDeparturePosition ?? "-");
			builder.Append("/");
			builder.Append(PredictedDeparturePosition ?? "-");
			builder.Append(")");
			return builder.ToString();
		}
	}
}