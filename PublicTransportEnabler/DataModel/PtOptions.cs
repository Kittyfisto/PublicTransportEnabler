using System.Diagnostics;
using System.Xml.Serialization;

namespace PublicTransportEnabler.DataModel
{
	[XmlType("itdPtOptions")]
	[DebuggerDisplay("ExcludedMeans: {ExcludedMeans.Length}")]
	public class PtOptions
	{
		[XmlArray("excludedMeans")]
		public MeansElement[] ExcludedMeans { get; set; }

		[XmlArray("tariffZones")]
		[XmlArrayItem("itdTariffzones", typeof (TariffZone))]
		public TariffZoneList TariffZones { get; set; }

		[XmlAttribute("active")]
		public int Active { get; set; }

		[XmlAttribute("maxChanges")]
		public int MaxChanges { get; set; }

		[XmlAttribute("maxTime")]
		public int MaxTime { get; set; }

		[XmlAttribute("maxWait")]
		public int MaxWait { get; set; }

		[XmlAttribute("routeType")]
		public string RouteType { get; set; }

		[XmlAttribute("changeSpeed")]
		public string ChangeSpeed { get; set; }

		[XmlAttribute("lineRestriction")]
		public int LineRestriction { get; set; }

		[XmlAttribute("useProxFootSearch")]
		public int UseProxFootSearch { get; set; }

		[XmlAttribute("useProxFootSearchOrigin")]
		public int UseProxFootSearchOrigin { get; set; }

		[XmlAttribute("useProxFootSearchDestination")]
		public int UseProxFootSearchDestination { get; set; }

		[XmlAttribute("bike")]
		public int Bike { get; set; }

		[XmlAttribute("plane")]
		public int Plane { get; set; }

		[XmlAttribute("noCrowded")]
		public int NoCrowded { get; set; }

		[XmlAttribute("noSolidStairs")]
		public int NoSolidStairs { get; set; }

		[XmlAttribute("noEscalators")]
		public int NoEscalators { get; set; }

		[XmlAttribute("noElevators")]
		public int NoElevators { get; set; }

		[XmlAttribute("lowPlatformVhcl")]
		public int LowPlatformVhcl { get; set; }

		[XmlAttribute("wheelchair")]
		public int Wheelchair { get; set; }

		[XmlAttribute("needElevatedPlt")]
		public int NeedElevatedPlt { get; set; }

		[XmlAttribute("SOSAvail")]
		public int SOSAvail { get; set; }

		[XmlAttribute("noLonelyTransfer")]
		public int NoLonelyTransfer { get; set; }

		[XmlAttribute("illumTransfer")]
		public int IllumTransfer { get; set; }

		[XmlAttribute("overgroundTransfer")]
		public int OvergroundTransfer { get; set; }

		[XmlAttribute("noInsecurePlaces")]
		public int NoInsecurePlaces { get; set; }

		[XmlAttribute("privateTransport")]
		public int PrivateTransport { get; set; }
	}
}