using System.Collections.Generic;
using System.Text;

namespace PublicTransportEnabler.Model
{
	public class Footway : Part
	{
		public Footway(int min, int distance, bool transfer, Location departure, Location arrival,
		               List<Point> path) : base(departure, arrival, path)
		{
			Min = min;
			Distance = distance;
			Transfer = transfer;
		}

		public int Min { get; private set; }
		public int Distance { get; private set; }
		public bool Transfer { get; private set; }

		public override string ToString()
		{
			var builder = new StringBuilder(GetType().Name + "[");
			builder.Append("min=").Append(Min);
			builder.Append(",");
			builder.Append("distance=").Append(Distance);
			builder.Append(",");
			builder.Append("transfer=").Append(Transfer);
			builder.Append(",");
			builder.Append("departure=").Append(Departure.ToDebugString());
			builder.Append(",");
			builder.Append("arrival=").Append(Arrival.ToDebugString());
			builder.Append("]");
			return builder.ToString();
		}
	}
}