using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class Fare
	{
		public Fare(string network, Type type, Currency currency, decimal value, string unitName, string units)
		{
			Network = network;
			Type = type;
			Currency = currency;
			Value = value;
			UnitName = unitName;
			Units = units;
		}

		public string Network { get; private set; }
		public Type Type { get; private set; }
		public Currency Currency { get; private set; }
		public decimal Value { get; private set; }
		public string UnitName { get; private set; }
		public string Units { get; private set; }
	}
}