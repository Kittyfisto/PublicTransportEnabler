using System.Text;

namespace PublicTransportEnabler.Model
{
	public class LineDestination : EquatableBase<LineDestination>
	{
		public LineDestination(Line line, Location destination)
		{
			Line = line;
			Destination = destination;
		}

		public Line Line { get; private set; }
		public Location Destination { get; private set; }


		public override string ToString()
		{
			var builder = new StringBuilder("LineDestination(");
			builder.Append(Line != null ? Line.ToString() : "null");
			builder.Append(",");
			builder.Append(Destination != null ? Destination.ToString() : "null");
			builder.Append(")");
			return builder.ToString();
		}


		public override bool Equals(LineDestination other)
		{
			if (!NullSafeEquals(Line, other.Line))
				return false;
			if (!NullSafeEquals(Destination, other.Destination))
				return false;
			return true;
		}

		public override int InstanceGetHashCode()
		{
			int hashCode = 0;
			hashCode += NullSafeHashCode(Line);
			hashCode *= 29;
			hashCode += NullSafeHashCode(Destination);
			return hashCode;
		}
	}
}