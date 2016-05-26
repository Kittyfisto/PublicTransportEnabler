using System;

namespace PublicTransportEnabler.Model
{
	public abstract class EquatableBase<T> : IEquatable<T>, IComparable<T>
	{
		public virtual int CompareTo(T other)
		{
			return 0;
		}

		public abstract bool Equals(T other);

		public abstract int InstanceGetHashCode();

		public override int GetHashCode()
		{
			return InstanceGetHashCode();
		}

		public override bool Equals(Object o)
		{
			if (o == this)
				return true;

			if (o is T)
				return Equals((T) o);

			return false;
		}

		protected bool NullSafeEquals(Object o1, Object o2)
		{
			if (o1 == null && o2 == null)
				return true;
			if (o1 != null && o1.Equals(o2))
				return true;
			return false;
		}

		protected int NullSafeHashCode(Object o)
		{
			if (o == null)
				return 0;
			return o.GetHashCode();
		}
	}
}