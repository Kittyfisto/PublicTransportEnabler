using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdServingLine")]
	public class ServingLine
	{
		[XmlAttribute("key")]
		public int Key { get; set; }

		[XmlElement("motDivaParams")]
		public DivaParameters DivaParameters { get; set; }

		[XmlElement("itdNoTrain")]
		public NoTrain NoTrain { get; set; }

		[XmlElement("itdRouteDescText")]
		public string RouteDescText { get; set; }

		[XmlElement("itdOperator")]
		public Operator Operator { get; set; }

		[XmlAttribute("selected")]
		public int Selected { get; set; }

		[XmlAttribute("code")]
		public int Code { get; set; }

		[XmlAttribute("number")]
		public string Number { get; set; }

		[XmlAttribute("symbol")]
		public string Symbol { get; set; }

		[XmlAttribute("motType")]
		public int MotType { get; set; }

		[XmlAttribute("realtime")]
		public int Realtime { get; set; }

		[XmlAttribute("direction")]
		public string Direction { get; set; }

		[XmlAttribute("valid")]
		public string Valid { get; set; }

		[XmlAttribute("compound")]
		public int Compound { get; set; }

		[XmlAttribute("TTB")]
		public int TTB { get; set; }

		[XmlAttribute("STT")]
		public int STT { get; set; }

		[XmlAttribute("ROP")]
		public int ROP { get; set; }

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("spTr")]
		public string SpTr { get; set; }

		[XmlAttribute("destID")]
		public string DestId { get; set; }

		[XmlAttribute("stateless")]
		public string Stateless { get; set; }

		[XmlAttribute("trainName")]
		public string TrainName { get; set; }

		[XmlAttribute("index")]
		public string Index { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", Number, Direction);
		}
	}
}