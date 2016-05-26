using System;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class Location : EquatableBase<Location>
	{
		private static readonly string[] NON_UNIQUE_NAMES =
			{
				"Hauptbahnhof", "Hbf", "Bahnhof", "Dorf", "Kirche", "Nord", "Ost",
				"Süd", "West"
			};

		public Location(LocationType type, int id, int lat, int lon, string place, string name)
		{
			if (id < 0)
				throw new ArgumentException("assert failed: id=" + id);

			Array.Sort(NON_UNIQUE_NAMES);

			Type = type;
			Id = id;
			Lat = lat;
			Lon = lon;
			Place = place;
			Name = name;
		}

		public Location(LocationType type, int id, string place, string name)
			: this(type, id, 0, 0, place, name)
		{
		}

		public Location(LocationType type, int id, int lat, int lon)
			: this(type, id, lat, lon, null, null)
		{
		}

		public Location(LocationType type, int id)
			: this(type, id, 0, 0, null, null)
		{
		}

		public Location(LocationType type, int lat, int lon)
			: this(type, 0, lat, lon, null, null)
		{
		}

		public LocationType Type { get; private set; }
		public int Id { get; private set; }
		public int Lat { get; private set; }
		public int Lon { get; private set; }
		public string Place { get; private set; }
		public string Name { get; private set; }

		public static Location FromWgs84(LocationType type, int id, double latitude, double longitude)
		{
			const int multiplier = 1000000;
			var lat = (int) (multiplier*latitude);
			var lon = (int) (multiplier*longitude);
			return new Location(type, id, lat, lon);
		}

		public static Location FromId(LocationType type, int id)
		{
			return new Location(type, id);
		}

		public bool HasId()
		{
			return Id != 0;
		}

		public bool HasLocation()
		{
			return Lat != 0 || Lon != 0;
		}

		public bool IsIdentified()
		{
			if (Type == LocationType.STATION)
				return HasId();

			if (Type == LocationType.POI)
				return true;

			if (Type == LocationType.ADDRESS)
				return HasLocation();

			return false;
		}

		public string UniqueShortName()
		{
			if (Name != null && Array.BinarySearch(NON_UNIQUE_NAMES, Name) >= 0)
				return Place + ", " + Name;

			if (Name != null)
				return Name;

			if (HasId())
				return Id.ToString();

			return null;
		}

		public override string ToString()
		{
			return Name; // invoked by AutoCompleteTextView in landscape orientation
		}

		public string ToDebugString()
		{
			return "[" + Type + " " + Id + " " + Lat + "/" + Lon + " " + (Place != null ? "'" + Place + "'" : "null") +
			       " '" + Name + "']";
		}

		public override int InstanceGetHashCode()
		{
			int hashCode = 0;
			hashCode += Type.GetHashCode();
			hashCode *= 29;
			if (Id != 0)
			{
				hashCode += Id;
			}
			else if (Lat != 0 || Lon != 0)
			{
				hashCode += Lat;
				hashCode *= 29;
				hashCode += Lon;
			}
			return hashCode;
		}

		public override bool Equals(Location other)
		{
			if (other == this)
				return true;
			if (Type != other.Type)
				return false;
			if (Id != 0 && Id == other.Id)
				return true;
			if (Lat != 0 && Lon != 0 && Lat == other.Lat && Lon == other.Lon)
				return true;

			if (!NullSafeEquals(Name, other.Name)) // only discriminate by name if no ids are given
				return false;
			return true;
		}
	}
}