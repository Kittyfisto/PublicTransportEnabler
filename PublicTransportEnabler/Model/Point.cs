using System;

namespace PublicTransportEnabler.Model
{
	public class Point : EquatableBase<Point>
	{
		public Point(float lat, float lon)
		{
			Lat = (int) Math.Round(lat*1E6);
			Lon = (int) Math.Round(lon*1E6);
		}

		public Point(int lat, int lon)
		{
			Lat = lat;
			Lon = lon;
		}

		public int Lat { get; private set; }
		public int Lon { get; private set; }

		public override string ToString()
		{
			return "[" + Lat + "/" + Lon + "]";
		}

		public override bool Equals(Point other)
		{
			if (Lat != other.Lat)
				return false;
			if (Lon != other.Lon)
				return false;
			return true;
		}

		public override int InstanceGetHashCode()
		{
			return Lat + 27*Lon;
		}
	}
}