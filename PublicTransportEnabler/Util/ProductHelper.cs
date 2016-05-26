using System;
using System.Collections.Generic;
using System.Linq;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Util
{
	public class ProductHelper
	{
		public static IEnumerable<Product> ALL = System.Enum.GetValues(typeof (Product)).Cast<Product>();

		public char Code { get; private set; }

		public static Product FromCode(char code)
		{
			// ('I'), ('R'), ('S'), ('U'), ('T'), ('B'), ('F'), ('C'), ('P');


			switch (code)
			{
				case 'I':
					return Product.HIGH_SPEED_TRAIN;
				case 'R':
					return Product.REGIONAL_TRAIN;
				case 'S':
					return Product.SUBURBAN_TRAIN;
				case 'U':
					return Product.SUBWAY;
				case 'T':
					return Product.TRAM;
				case 'B':
					return Product.BUS;
				case 'F':
					return Product.FERRY;
				case 'C':
					return Product.CABLECAR;
				case 'P':
					return Product.ON_DEMAND;
				default:
					throw new ArgumentException(code.ToString());
			}
		}
	}
}