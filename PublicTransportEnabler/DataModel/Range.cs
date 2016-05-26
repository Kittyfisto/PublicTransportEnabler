namespace PublicTransportEnabler.DataModel
{
	public class Range<T>
	{
		public Range(T from, T to)
		{
			From = from;
			To = to;
		}

		public T From { get; private set; }
		public T To { get; private set; }
	}
}