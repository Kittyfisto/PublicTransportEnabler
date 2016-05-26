using System;
using System.Text;

namespace PublicTransportEnabler.Model
{
	public class Departure : EquatableBase<Departure>
	{
		public Departure(DateTime? plannedTime, DateTime? predictedTime, Line line, string position,
		                 Location destination, int[] capacity, String message)
		{
			PlannedTime = plannedTime;
			PredictedTime = predictedTime;
			Line = line;
			Position = position;
			Destination = destination;
			Capacity = capacity;
			Message = message;
		}

		public DateTime? PlannedTime { get; private set; }
		public DateTime? PredictedTime { get; private set; }
		public Line Line { get; private set; }
		public string Position { get; private set; }
		public Location Destination { get; private set; }
		public int[] Capacity { get; private set; }
		public string Message { get; private set; }

		public override string ToString()
		{
			var builder = new StringBuilder("Departure(");
			builder.Append(PlannedTime.HasValue ? PlannedTime.Value.ToString("d") : "null");
			builder.Append(",");
			builder.Append(PredictedTime.HasValue ? PredictedTime.Value.ToString("d") : "null");
			builder.Append(",");
			builder.Append(Line != null ? Line.ToString() : "null");
			builder.Append(",");
			builder.Append(Position ?? "null");
			builder.Append(",");
			builder.Append(Destination != null ? Destination.ToString() : "null");
			builder.Append(")");
			return builder.ToString();
		}

		public override bool Equals(Departure other)
		{
			if (!NullSafeEquals(PlannedTime, other.PlannedTime))
				return false;
			if (!NullSafeEquals(PredictedTime, other.PredictedTime))
				return false;
			if (!NullSafeEquals(Line, other.Line))
				return false;
			if (!NullSafeEquals(Destination, other.Destination))
				return false;
			return true;
		}

		public override int InstanceGetHashCode()
		{
			int hashCode = 0;
			hashCode += NullSafeHashCode(PlannedTime);
			hashCode *= 29;
			hashCode += NullSafeHashCode(PredictedTime);
			hashCode *= 29;
			hashCode += NullSafeHashCode(Line);
			hashCode *= 29;
			hashCode += NullSafeHashCode(Destination);
			return hashCode;
		}
	}
}