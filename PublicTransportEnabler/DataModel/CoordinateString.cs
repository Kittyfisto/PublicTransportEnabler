using System;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdCoordinateString")]
	public class CoordinateString
	{
		[XmlAttribute("decimal")]
		public string Decimal { get; set; }

		[XmlAttribute("cs")]
		public string Cs { get; set; }

		[XmlAttribute("ts")]
		public string Ts { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlIgnore]
		public Coordinate Coordinate
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Text) || string.IsNullOrWhiteSpace(Cs))
					return null;

				string[] latlon = Text.Split(Cs.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				if (latlon.Length != 2)
					return null;

				int x = Int32.Parse(latlon[0]);
				int y = Int32.Parse(latlon[1]);

				return new Coordinate
					{
						X = x,
						Y = y
					};
			}
		}
	}
}