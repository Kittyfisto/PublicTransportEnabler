using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using PublicTransportEnabler.Contract;
using PublicTransportEnabler.DataModel;
using PublicTransportEnabler.Enum;
using PublicTransportEnabler.Model;
using Location = PublicTransportEnabler.Model.Location;

namespace PublicTransportEnabler
{
	public abstract class AbstractEfaProvider : AbstractNetworkProvider
	{
		protected static string DEFAULT_DEPARTURE_MONITOR_ENDPOINT = "XSLT_DM_REQUEST";
		protected static string DEFAULT_TRIP_ENDPOINT = "XSLT_TRIP_REQUEST2";
		protected static string DEFAULT_STOPFINDER_ENDPOINT = "XML_STOPFINDER_REQUEST";
		protected static string DEFAULT_COORD_ENDPOINT = "XML_COORD_REQUEST";

		protected static string SERVER_PRODUCT = "efa";
		private static readonly Regex P_LINE_IRE = new Regex("IRE\\d+");
		private static readonly Regex P_LINE_RE = new Regex("RE\\d+");
		private static readonly Regex P_LINE_RB = new Regex("RB\\d+");
		private static readonly Regex P_LINE_VB = new Regex("VB\\d+");
		private static readonly Regex P_LINE_OE = new Regex("OE\\d+");
		private static readonly Regex P_LINE_R = new Regex("R\\d+(/R\\d+|\\(z\\))?");
		private static readonly Regex P_LINE_U = new Regex("U\\d+");
		private static readonly Regex P_LINE_S = new Regex("^(?:%)?(S\\d+)");
		private static Regex P_LINE_NUMBER = new Regex("\\d+");
		private static Regex P_LINE_Y = new Regex("\\d+Y");
		private static readonly Regex P_LINE_SEV = new Regex("SEV.*");
		private static readonly Regex P_STATION_NAME_WHITESPACE = new Regex("\\s+");
		private static Regex P_SESSION_EXPIRED = new Regex("Your session has expired");
		private static Regex P_PLATFORM = new Regex("#?(\\d+)", RegexOptions.IgnoreCase);

		private static Regex P_PLATFORM_NAME =
			new Regex("(?:Gleis|Gl\\.|Bstg\\.)?\\s*(\\d+)\\s*(?:([A-Z])\\s*(?:-\\s*([A-Z]))?)?",
			          RegexOptions.IgnoreCase);

		protected static Dictionary<WalkSpeed, string> WALKSPEED_MAP = new Dictionary<WalkSpeed, string>();

		private readonly string coordEndpoint;
		private readonly string departureMonitorEndpoint;
		private readonly string stopFinderEndpoint;
		private readonly string tripEndpoint;
		private readonly IWebClient webClient;
		private string additionalQueryParameter;
		private bool canAcceptPoiId;
		private bool httpPost;
		private string httpReferer;
		private string httpRefererTrip;
		private bool includeRegionId = true;
		private bool needsSpEncId;
		private string requestUrlEncoding = "ISO-8859-1";
		private bool suppressPositions;
		private bool useRouteIndexAsConnectionId = true;

		static AbstractEfaProvider()
		{
			WALKSPEED_MAP.Add(WalkSpeed.SLOW, "slow");
			WALKSPEED_MAP.Add(WalkSpeed.NORMAL, "normal");
			WALKSPEED_MAP.Add(WalkSpeed.FAST, "fast");
		}

		protected AbstractEfaProvider(string apiBase, IWebClient webClient)
			: this(apiBase, null, null, null, null, webClient)
		{
		}

		protected AbstractEfaProvider(string apiBase, string departureMonitorEndpoint, string tripEndpoint,
		                              string stopFinderEndpoint, string coordEndpoint, IWebClient webClient)
			: this(
				apiBase +
				(departureMonitorEndpoint ?? DEFAULT_DEPARTURE_MONITOR_ENDPOINT), //
				apiBase + (tripEndpoint ?? DEFAULT_TRIP_ENDPOINT), //
				apiBase + (stopFinderEndpoint ?? DEFAULT_STOPFINDER_ENDPOINT), //
				apiBase + (coordEndpoint ?? DEFAULT_COORD_ENDPOINT), webClient)
		{
		}

		private AbstractEfaProvider(string departureMonitorEndpoint, string tripEndpoint, string stopFinderEndpoint,
		                            string coordEndpoint, IWebClient webClient)
		{
			this.webClient = webClient;

			this.departureMonitorEndpoint = departureMonitorEndpoint;
			this.tripEndpoint = tripEndpoint;
			this.stopFinderEndpoint = stopFinderEndpoint;
			this.coordEndpoint = coordEndpoint;
		}

		protected IWebClient WebClient
		{
			get { return webClient; }
		}

		protected void SetRequestUrlEncoding(string requestUrlEncodingIn)
		{
			requestUrlEncoding = requestUrlEncodingIn;
		}

		protected void SetHttpReferer(string httpRefererIn)
		{
			httpReferer = httpRefererIn;
			httpRefererTrip = httpRefererIn;
		}

		public void SetHttpRefererTrip(string httpRefererTripIn)
		{
			httpRefererTrip = httpRefererTripIn;
		}

		protected void SetHttpPost(bool httpPostIn)
		{
			httpPost = httpPostIn;
		}

		protected void SetIncludeRegionId(bool includeRegionIdIn)
		{
			includeRegionId = includeRegionIdIn;
		}

		protected void SetSuppressPositions(bool suppressPositionsIn)
		{
			suppressPositions = suppressPositionsIn;
		}

		protected void SetUseRouteIndexAsConnectionId(bool useRouteIndexAsConnectionIdIn)
		{
			useRouteIndexAsConnectionId = useRouteIndexAsConnectionIdIn;
		}

		protected void SetCanAcceptPoiId(bool canAcceptPoiIdIn)
		{
			canAcceptPoiId = canAcceptPoiIdIn;
		}

		protected void SetNeedsSpEncId(bool needsSpEncIdIn)
		{
			needsSpEncId = needsSpEncIdIn;
		}

		protected void SetAdditionalQueryParameter(string additionalQueryParameterIn)
		{
			additionalQueryParameter = additionalQueryParameterIn;
		}

		/*
        // TODO: timezone
        protected TimeZone TimeZone()
        {
            return System.TimeZone.CurrentTimeZone;
        }*/

		private void AppendCommonRequestParams(StringBuilder uri, string outputFormat)
		{
			uri.Append("?outputFormat=").Append(outputFormat);
			uri.Append("&coordOutputFormat=WGS84");
			if (additionalQueryParameter != null)
				uri.Append('&').Append(additionalQueryParameter);
		}

		protected StopFinderRequest JsonStopfinderRequest(Location constraint)
		{
			var parameters = new StringBuilder();
			AppendCommonRequestParams(parameters, "JSON");
			parameters.Append("&locationServerActive=1");
			if (includeRegionId)
				parameters.Append("&regionID_sf=1"); // prefer own region
			AppendLocation(parameters, constraint, "sf");
			if (constraint.Type == LocationType.ANY)
				// 1=place 2=stop 4=street 8=address 16=crossing 32=poi 64=postcode
				parameters.Append("&anyObjFilter_sf=").Append(2 + 4 + 8 + 16 + 32 + 64);
			parameters.Append("&anyMaxSizeHitList=500");

			var uri = new StringBuilder(stopFinderEndpoint);
			if (!httpPost)
				uri.Append(parameters);

			//// System.out.println(uri);
			//// System.out.println(parameters);

			//string page = webClient.scrape(uri.tostring(), httpPost ? parameters.substring(1) : null, UTF_8, null);

			try
			{
				var results = new List<Location>();

				//JSONObject head = new JSONObject(page.tostring());
				//JSONObject stopFinder = head.optJSONObject("stopFinder");
				//JSONArray stops;
				//if (stopFinder == null)
				//{
				//    stops = head.getJSONArray("stopFinder");
				//}
				//else
				//{
				//    JSONObject points = stopFinder.optJSONObject("points");
				//    if (points != null)
				//    {
				//        JSONObject stop = points.getJSONObject("point");
				//        Location location = parseJsonStop(stop);
				//        results.add(location);
				//        return results;
				//    }

				//    stops = stopFinder.getJSONArray("points");
				//}

				//int nStops = stops.length();

				//for (int i = 0; i < nStops; i++)
				//{
				//    JSONObject stop = stops.optJSONObject(i);
				//    Location location = parseJsonStop(stop);
				//    results.add(location);
				//}

				return null;
			}
			catch (Exception x)
			{
				throw;
				//throw new Exception("cannot parse: '" + page + "' on " + uri, x);
			}
		}

		private Location ParseJsonStop(string stop)
		{
			//string type = stop.getstring("type");
			//if ("any".Equals(type))
			//    type = stop.getstring("anyType");
			//string name = normalizeLocationName(stop.getstring("object"));
			//JSONObject ref = stop.getJSONObject("ref");
			//string place = ref.getstring("place");
			//if (place != null && place.length() == 0)
			//    place = null;
			//string coords = ref.optstring("coords", null);
			//int lat;
			//int lon;
			//if (coords != null)
			//{
			//    string[] coordParts = coords.split(",");
			//    lat = Math.round(Float.parseFloat(coordParts[1]));
			//    lon = Math.round(Float.parseFloat(coordParts[0]));
			//}
			//else
			//{
			//    lat = 0;
			//    lon = 0;
			//}

			//if ("stop".Equals(type))
			//    return new Location(LocationType.STATION, stop.getInt("stateless"), lat, lon, place, name);
			//else if ("poi".Equals(type))
			//    return new Location(LocationType.POI, 0, lat, lon, place, name);
			//else if ("crossing".Equals(type))
			//    return new Location(LocationType.ADDRESS, 0, lat, lon, place, name);
			//else if ("street".Equals(type) || "address".Equals(type) || "singlehouse".Equals(type))
			//    return new Location(LocationType.ADDRESS, 0, lat, lon, place, normalizeLocationName(stop.getstring("name")));
			//else
			//    throw new JSONException("unknown type: " + type);
			return null;
		}

		protected StopFinderRequest XmlStopfinderRequest(Location constraint)
		{
			var parameters = new StringBuilder();
			AppendCommonRequestParams(parameters, "XML");
			parameters.Append("&locationServerActive=1");
			if (includeRegionId)
				parameters.Append("&regionID_sf=1"); // prefer own region
			AppendLocation(parameters, constraint, "sf");
			if (constraint.Type == LocationType.ANY)
			{
				if (needsSpEncId)
					parameters.Append("&SpEncId=0");
				// 1=place 2=stop 4=street 8=address 16=crossing 32=poi 64=postcode
				parameters.Append("&anyObjFilter_sf=").Append(2 + 4 + 8 + 16 + 32 + 64);
				parameters.Append("&reducedAnyPostcodeObjFilter_sf=64&reducedAnyTooManyObjFilter_sf=2");
				parameters.Append("&useHouseNumberList=true");
			}

			var uri = new StringBuilder(stopFinderEndpoint);
			if (!httpPost)
				uri.Append(parameters);

			string resultString = WebClient.Scrape(uri.ToString(), httpPost ? parameters.ToString().Substring(1) : null,
													   requestUrlEncoding, null, 3);

			var serializer = new XmlSerializer(typeof(Request));

			var itdRequest = serializer.Deserialize(new StringReader(resultString)) as Request;

			return itdRequest.StopFinderRequest;
		}

		protected OuterCoordInfoRequest XmlCoordRequest(int lat, int lon, int maxDistance, int maxStations)
		{
			var parameters = new StringBuilder();
			AppendCommonRequestParams(parameters, "XML");
			parameters.Append("&coord=").Append(
				string.Format("{0}:{1}:WGS84",
				              LatLonToDouble(lon).ToString("F6", CultureInfo.InvariantCulture.NumberFormat),
				              LatLonToDouble(lat).ToString("F6", CultureInfo.InvariantCulture.NumberFormat)));
			parameters.Append("&coordListOutputFormat=string");
			parameters.Append("&max=").Append(maxStations != 0 ? maxStations : 50);
			parameters.Append("&inclFilter=1&radius_1=").Append(maxDistance != 0 ? maxDistance : 1320);
			parameters.Append("&type_1=STOP"); // ENTRANCE, BUS_POINT, POI_POINT

			var uri = new StringBuilder(coordEndpoint);
			if (!httpPost)
				uri.Append(parameters);

			string resultString = WebClient.Scrape(uri.ToString(), httpPost ? parameters.ToString().Substring(1) : null,
													   requestUrlEncoding, null, 3);

			var serializer = new XmlSerializer(typeof(Request));

			var itdRequest = serializer.Deserialize(new StringReader(resultString)) as Request;
			return itdRequest.CoordInfoRequest;
		}

		public override OuterCoordInfoRequest QueryNearbyStations(Location location, int maxDistance, int maxStations)
		{
			if (location.HasLocation())
				return XmlCoordRequest(location.Lat, location.Lon, maxDistance, maxStations);

			if (location.Type != LocationType.STATION)
				throw new ArgumentException("cannot handle: " + location.Type);

			if (!location.HasId())
				throw new ArgumentException("at least one of stationId or lat/lon must be given");

			return NearbyStationsRequest(location.Id, maxStations);
		}

		private OuterCoordInfoRequest NearbyStationsRequest(int stationId, int maxStations)
		{
			var parameters = new StringBuilder();
			AppendCommonRequestParams(parameters, "XML");
			parameters.Append("&type_dm=stop&name_dm=").Append(stationId);
			parameters.Append("&itOptionsActive=1");
			parameters.Append("&ptOptionsActive=1");
			parameters.Append("&useProxFootSearch=1");
			parameters.Append("&mergeDep=1");
			parameters.Append("&useAllStops=1");
			parameters.Append("&max=").Append(maxStations != 0 ? maxStations : 50);
			parameters.Append("&mode=direct");

			var uri = new StringBuilder(departureMonitorEndpoint);
			if (!httpPost)
				uri.Append(parameters);

			string resultString = WebClient.Scrape(uri.ToString(), httpPost ? parameters.ToString().Substring(1) : null,
													   requestUrlEncoding, null, 3);


			var serializer = new XmlSerializer(typeof(Request));

			var itdRequest = serializer.Deserialize(new StringReader(resultString)) as Request;

			if (itdRequest == null || itdRequest.DepartureMonitorRequest == null)
				throw new Exception("Invalid XML");


			return itdRequest.CoordInfoRequest;
		}


		public override StopFinderRequest AutocompleteStations(string constraint)
		{
			return JsonStopfinderRequest(new Location(LocationType.ANY, 0, null, constraint));
		}


		protected string parseLine(string mot, string symbol, string name, string longName, string trainType,
		                           string trainNum, string trainName)
		{
			if (mot == null)
			{
				if (trainName != null)
				{
					string str = name ?? "";
					if (trainName.Equals("S-Bahn"))
						return 'S' + str;
					if (trainName.Equals("U-Bahn"))
						return 'U' + str;
					if (trainName.Equals("Straßenbahn"))
						return 'T' + str;
					if (trainName.Equals("Badner Bahn"))
						return 'T' + str;
					if (trainName.Equals("Stadtbus"))
						return 'B' + str;
					if (trainName.Equals("Citybus"))
						return 'B' + str;
					if (trainName.Equals("Regionalbus"))
						return 'B' + str;
					if (trainName.Equals("ÖBB-Postbus"))
						return 'B' + str;
					if (trainName.Equals("Autobus"))
						return 'B' + str;
					if (trainName.Equals("Discobus"))
						return 'B' + str;
					if (trainName.Equals("Nachtbus"))
						return 'B' + str;
					if (trainName.Equals("Anrufsammeltaxi"))
						return 'B' + str;
					if (trainName.Equals("Ersatzverkehr"))
						return 'B' + str;
					if (trainName.Equals("Vienna Airport Lines"))
						return 'B' + str;
				}

				throw new Exception("cannot normalize mot='" + mot + "' symbol='" + symbol + "' name='" + name + "' long='" +
				                    longName
				                    + "' trainType='" + trainType + "' trainNum='" + trainNum + "' trainName='" + trainName + "'");
			}

			int t = int.Parse(mot);

			if (t == 0)
			{
				string[] parts = longName.Split(" ".ToCharArray(), 3);
				string type = parts[0];
				string num = parts.Length >= 2 ? parts[1] : null;
				string str = type + (num != null ? num : "");

				if (type.Equals("EC")) // Eurocity
					return 'I' + str;
				if (type.Equals("EN")) // Euronight
					return 'I' + str;
				if (type.Equals("IC")) // Intercity
					return 'I' + str;
				if ("InterCity".Equals(type))
					return 'I' + str;
				if (type.Equals("ICE")) // Intercity Express
					return 'I' + str;
				if (type.Equals("X")) // InterConnex
					return 'I' + str;
				if (type.Equals("CNL")) // City Night Line
					return 'I' + str;
				if (type.Equals("THA")) // Thalys
					return 'I' + str;
				if (type.Equals("TGV")) // TGV
					return 'I' + str;
				if (type.Equals("RJ")) // railjet
					return 'I' + str;
				if ("WB".Equals(type)) // westbahn
					return 'R' + str;
				if (type.Equals("OEC")) // ÖBB-EuroCity
					return 'I' + str;
				if (type.Equals("OIC")) // ÖBB-InterCity
					return 'I' + str;
				if (type.Equals("HT")) // First Hull Trains, GB
					return 'I' + str;
				if (type.Equals("MT")) // Müller Touren, Schnee Express
					return 'I' + str;
				if (type.Equals("HKX")) // Hamburg-Koeln-Express
					return 'I' + str;
				if (type.Equals("DNZ")) // Nachtzug Basel-Moskau
					return 'I' + str;
				if ("INT".Equals(type)) // SVV
					return 'I' + name;
				if ("IXB".Equals(type)) // ICE International
					return 'I' + name;
				if ("SC".Equals(type)) // SuperCity, Tschechien
					return 'I' + name;
				if ("ECB".Equals(type)) // EC, Verona-München
					return 'I' + name;
				if ("ES".Equals(type)) // Eurostar Italia
					return 'I' + name;
				if ("Eurocity".Equals(trainName)) // Liechtenstein
					return 'I' + name;
				if ("EuroNight".Equals(trainName)) // Liechtenstein
					return 'I' + name;
				if ("railjet".Equals(trainName)) // Liechtenstein
					return 'I' + name;
				if ("ÖBB InterCity".Equals(trainName)) // Liechtenstein
					return 'I' + name;

				if (type.Equals("IR")) // Interregio
					return 'R' + str;
				if ("InterRegio".Equals(type))
					return 'R' + str;
				if (type.Equals("IRE")) // Interregio-Express
					return 'R' + str;
				if (P_LINE_IRE.Match(type).Success)
					return 'R' + str;
				if (type.Equals("RE")) // Regional-Express
					return 'R' + str;
				if (type.Equals("R-Bahn")) // Regional-Express, VRR
					return 'R' + str;
				if ("RB-Bahn".Equals(type)) // Vogtland
					return 'R' + str;
				if (type.Equals("REX")) // RegionalExpress, Österreich
					return 'R' + str;
				if ("EZ".Equals(type)) // ÖBB ErlebnisBahn
					return 'R' + str;
				if (P_LINE_RE.Match(type).Success)
					return 'R' + str;
				if (type.Equals("RB")) // Regionalbahn
					return 'R' + str;
				if (P_LINE_RB.Match(type).Success)
					return 'R' + str;
				if (type.Equals("R")) // Regionalzug
					return 'R' + str;
				if (P_LINE_R.Match(type).Success)
					return 'R' + str;
				if (type.Equals("Bahn"))
					return 'R' + str;
				if (type.Equals("Regionalbahn"))
					return 'R' + str;
				if (type.Equals("D")) // Schnellzug
					return 'R' + str;
				if (type.Equals("E")) // Eilzug
					return 'R' + str;
				if (type.Equals("S")) // ~Innsbruck
					return 'R' + str;
				if (type.Equals("WFB")) // Westfalenbahn
					return 'R' + str;
				if ("Westfalenbahn".Equals(type)) // Westfalenbahn
					return 'R' + name;
				if (type.Equals("NWB")) // NordWestBahn
					return 'R' + str;
				if (type.Equals("NordWestBahn"))
					return 'R' + str;
				if (type.Equals("ME")) // Metronom
					return 'R' + str;
				if (type.Equals("ERB")) // eurobahn
					return 'R' + str;
				if (type.Equals("CAN")) // cantus
					return 'R' + str;
				if (type.Equals("HEX")) // Veolia Verkehr Sachsen-Anhalt
					return 'R' + str;
				if (type.Equals("EB")) // Erfurter Bahn
					return 'R' + str;
				if (type.Equals("EBx")) // Erfurter Bahn Express
					return 'R' + str;
				if (type.Equals("MRB")) // Mittelrheinbahn
					return 'R' + str;
				if (type.Equals("ABR")) // ABELLIO Rail NRW
					return 'R' + str;
				if (type.Equals("NEB")) // Niederbarnimer Eisenbahn
					return 'R' + str;
				if (type.Equals("OE")) // Ostdeutsche Eisenbahn
					return 'R' + str;
				if (P_LINE_OE.Match(type).Success)
					return 'R' + str;
				if (type.Equals("MR")) // Märkische Regiobahn
					return 'R' + str;
				if (type.Equals("OLA")) // Ostseeland Verkehr
					return 'R' + str;
				if (type.Equals("UBB")) // Usedomer Bäderbahn
					return 'R' + str;
				if (type.Equals("EVB")) // Elbe-Weser
					return 'R' + str;
				if (type.Equals("PEG")) // Prignitzer Eisenbahngesellschaft
					return 'R' + str;
				if (type.Equals("RTB")) // Rurtalbahn
					return 'R' + str;
				if (type.Equals("STB")) // Süd-Thüringen-Bahn
					return 'R' + str;
				if (type.Equals("HTB")) // Hellertalbahn
					return 'R' + str;
				if (type.Equals("VBG")) // Vogtlandbahn
					return 'R' + str;
				if (type.Equals("VB")) // Vogtlandbahn
					return 'R' + str;
				if (P_LINE_VB.Match(type).Success)
					return 'R' + str;
				if (type.Equals("VX")) // Vogtland Express
					return 'R' + str;
				if (type.Equals("CB")) // City-Bahn Chemnitz
					return 'R' + str;
				if (type.Equals("VEC")) // VECTUS Verkehrsgesellschaft
					return 'R' + str;
				if (type.Equals("HzL")) // Hohenzollerische Landesbahn
					return 'R' + str;
				if (type.Equals("OSB")) // Ortenau-S-Bahn
					return 'R' + str;
				if (type.Equals("SBB")) // SBB
					return 'R' + str;
				if (type.Equals("MBB")) // Mecklenburgische Bäderbahn Molli
					return 'R' + str;
				if (type.Equals("OS")) // Regionalbahn
					return 'R' + str;
				if (type.Equals("SP"))
					return 'R' + str;
				if (type.Equals("Dab")) // Daadetalbahn
					return 'R' + str;
				if (type.Equals("FEG")) // Freiberger Eisenbahngesellschaft
					return 'R' + str;
				if (type.Equals("ARR")) // ARRIVA
					return 'R' + str;
				if (type.Equals("HSB")) // Harzer Schmalspurbahn
					return 'R' + str;
				if (type.Equals("SBE")) // Sächsisch-Böhmische Eisenbahngesellschaft
					return 'R' + str;
				if (type.Equals("ALX")) // Arriva-Länderbahn-Express
					return 'R' + str;
				if (type.Equals("EX")) // ALX verwandelt sich
					return 'R' + str;
				if (type.Equals("MEr")) // metronom regional
					return 'R' + str;
				if (type.Equals("AKN")) // AKN Eisenbahn
					return 'R' + str;
				if (type.Equals("ZUG")) // Regionalbahn
					return 'R' + str;
				if (type.Equals("SOE")) // Sächsisch-Oberlausitzer Eisenbahngesellschaft
					return 'R' + str;
				if (type.Equals("VIA")) // VIAS
					return 'R' + str;
				if (type.Equals("BRB")) // Bayerische Regiobahn
					return 'R' + str;
				if (type.Equals("BLB")) // Berchtesgadener Land Bahn
					return 'R' + str;
				if (type.Equals("HLB")) // Hessische Landesbahn
					return 'R' + str;
				if (type.Equals("NOB")) // NordOstseeBahn
					return 'R' + str;
				if (type.Equals("WEG")) // Wieslauftalbahn
					return 'R' + str;
				if (type.Equals("NBE")) // Nordbahn Eisenbahngesellschaft
					return 'R' + str;
				if (type.Equals("VEN")) // Rhenus Veniro
					return 'R' + str;
				if (type.Equals("DPN")) // Nahreisezug
					return 'R' + str;
				if (type.Equals("SHB")) // Schleswig-Holstein-Bahn
					return 'R' + str;
				if (type.Equals("RBG")) // Regental Bahnbetriebs GmbH
					return 'R' + str;
				if (type.Equals("BOB")) // Bayerische Oberlandbahn
					return 'R' + str;
				if (type.Equals("SWE")) // Südwestdeutsche Verkehrs AG
					return 'R' + str;
				if (type.Equals("VE")) // Vetter
					return 'R' + str;
				if (type.Equals("SDG")) // Sächsische Dampfeisenbahngesellschaft
					return 'R' + str;
				if (type.Equals("PRE")) // Pressnitztalbahn
					return 'R' + str;
				if (type.Equals("VEB")) // Vulkan-Eifel-Bahn
					return 'R' + str;
				if (type.Equals("neg")) // Norddeutsche Eisenbahn Gesellschaft
					return 'R' + str;
				if (type.Equals("AVG")) // Felsenland-Express
					return 'R' + str;
				if (type.Equals("ABG")) // Anhaltische Bahngesellschaft
					return 'R' + str;
				if (type.Equals("LGB")) // Lößnitzgrundbahn
					return 'R' + str;
				if (type.Equals("LEO")) // Chiemgauer Lokalbahn
					return 'R' + str;
				if (type.Equals("WTB")) // Weißeritztalbahn
					return 'R' + str;
				if (type.Equals("P")) // Kasbachtalbahn, Wanderbahn im Regental, Rhön-Zügle
					return 'R' + str;
				if (type.Equals("ÖBA")) // Eisenbahn-Betriebsgesellschaft Ochsenhausen
					return 'R' + str;
				if (type.Equals("MBS")) // Montafonerbahn
					return 'R' + str;
				if (type.Equals("EGP")) // EGP - die Städtebahn GmbH
					return 'R' + str;
				if (type.Equals("SBS")) // Städtebahn Sachsen, EGP - die Städtebahn GmbH
					return 'R' + str;
				if (type.Equals("SES")) // Städtebahn Sachsen Express, EGP - die Städtebahn GmbH
					return 'R' + str;
				if (type.Equals("SB")) // Städtebahn Sachsen
					return 'R' + str;
				if (type.Equals("agi")) // agilis
					return 'R' + str;
				if (type.Equals("ag")) // agilis
					return 'R' + str;
				if (type.Equals("as")) // agilis-Schnellzug
					return 'R' + str;
				if (type.Equals("agilis")) // agilis
					return 'R' + str;
				if (type.Equals("agilis-Schnellzug")) // agilis-Schnellzug
					return 'R' + str;
				if (type.Equals("TLX")) // Trilex (Vogtlandbahn)
					return 'R' + str;
				if (type.Equals("DBG")) // Döllnitzbahn
					return 'R' + str;
				if (type.Equals("MSB")) // Mainschleifenbahn
					return 'R' + str;
				if (type.Equals("BE")) // Grensland-Express, Niederlande
					return 'R' + str;
				if (type.Equals("MEL")) // Museums-Eisenbahn Losheim
					return 'R' + str;
				if (type.Equals("Abellio-Zug")) // Abellio
					return 'R' + str;
				if ("erx".Equals(type)) // erixx
					return 'R' + str;
				if ("SWEG-Zug".Equals(type)) // Südwestdeutschen Verkehrs-Aktiengesellschaft, evtl. S-Bahn?
					return 'R' + str;
				if (type.Equals("KBS")) // Kursbuchstrecke
					return 'R' + str;
				if (type.Equals("Zug"))
					return 'R' + str;
				if (type.Equals("ÖBB"))
					return 'R' + str;
				if (type.Equals("CAT")) // City Airport Train Wien
					return 'R' + str;
				if (type.Equals("DZ")) // Dampfzug, STV
					return 'R' + str;
				if (type.Equals("CD"))
					return 'R' + str;
				if (type.Equals("PR"))
					return 'R' + str;
				if (type.Equals("KD")) // Koleje Dolnośląskie (Niederschlesische Eisenbahn)
					return 'R' + str;
				if (type.Equals("VIAMO"))
					return 'R' + str;
				if (type.Equals("SE")) // Southeastern, GB
					return 'R' + str;
				if (type.Equals("SW")) // South West Trains, GB
					return 'R' + str;
				if (type.Equals("SN")) // Southern, GB
					return 'R' + str;
				if (type.Equals("NT")) // Northern Rail, GB
					return 'R' + str;
				if (type.Equals("CH")) // Chiltern Railways, GB
					return 'R' + str;
				if (type.Equals("EA")) // National Express East Anglia, GB
					return 'R' + str;
				if (type.Equals("FC")) // First Capital Connect, GB
					return 'R' + str;
				if (type.Equals("GW")) // First Great Western, GB
					return 'R' + str;
				if (type.Equals("XC")) // Cross Country, GB, evtl. auch highspeed?
					return 'R' + str;
				if (type.Equals("HC")) // Heathrow Connect, GB
					return 'R' + str;
				if (type.Equals("HX")) // Heathrow Express, GB
					return 'R' + str;
				if (type.Equals("GX")) // Gatwick Express, GB
					return 'R' + str;
				if (type.Equals("C2C")) // c2c, GB
					return 'R' + str;
				if (type.Equals("LM")) // London Midland, GB
					return 'R' + str;
				if (type.Equals("EM")) // East Midlands Trains, GB
					return 'R' + str;
				if (type.Equals("VT")) // Virgin Trains, GB, evtl. auch highspeed?
					return 'R' + str;
				if (type.Equals("SR")) // ScotRail, GB, evtl. auch long-distance?
					return 'R' + str;
				if (type.Equals("AW")) // Arriva Trains Wales, GB
					return 'R' + str;
				if (type.Equals("WS")) // Wrexham & Shropshire, GB
					return 'R' + str;
				if (type.Equals("TP")) // First TransPennine Express, GB, evtl. auch long-distance?
					return 'R' + str;
				if (type.Equals("GC")) // Grand Central, GB
					return 'R' + str;
				if (type.Equals("IL")) // Island Line, GB
					return 'R' + str;
				if ("FCC".Equals(type)) // First Capital Connect, GB
					return 'R' + str;
				if ("LE".Equals(type)) // Greater Anglia, GB
					return 'R' + str;
				if (type.Equals("BR")) // ??, GB
					return 'R' + str;
				if (type.Equals("OO")) // ??, GB
					return 'R' + str;
				if (type.Equals("XX")) // ??, GB
					return 'R' + str;
				if (type.Equals("XZ")) // ??, GB
					return 'R' + str;
				if (type.Equals("DB-Zug")) // VRR
					return 'R' + name;
				if (type.Equals("DB"))
					return 'R' + name;
				if (type.Equals("Regionalexpress")) // VRR
					return 'R' + name;
				if ("CAPITOL".Equals(name)) // San Francisco
					return 'R' + name;
				if ("Train".Equals(trainName) || "Train".Equals(type)) // San Francisco
					return "R" + name;
				if ("Regional Train :".Equals(longName))
					return "R";
				if ("Regional Train".Equals(trainName)) // Melbourne
					return "R" + name;
				if ("Regional".Equals(type)) // Melbourne
					return "R" + name;
				if (type.Equals("ATB")) // Autoschleuse Tauernbahn
					return 'R' + name;
				if ("Chiemsee-Bahn".Equals(type))
					return 'R' + name;
				if ("Regionalzug".Equals(trainName)) // Liechtenstein
					return 'R' + name;
				if ("RegionalExpress".Equals(trainName)) // Liechtenstein
					return 'R' + name;
				if ("Ostdeutsche".Equals(type)) // Bayern
					return 'R' + type;
				if ("Südwestdeutsche".Equals(type)) // Bayern
					return 'R' + type;
				if ("Mitteldeutsche".Equals(type)) // Bayern
					return 'R' + type;
				if ("Norddeutsche".Equals(type)) // Bayern
					return 'R' + type;
				if ("Hellertalbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Veolia".Equals(type)) // Bayern
					return 'R' + type;
				if ("vectus".Equals(type)) // Bayern
					return 'R' + type;
				if ("Hessische".Equals(type)) // Bayern
					return 'R' + type;
				if ("Niederbarnimer".Equals(type)) // Bayern
					return 'R' + type;
				if ("Rurtalbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Rhenus".Equals(type)) // Bayern
					return 'R' + type;
				if ("Mittelrheinbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Hohenzollerische".Equals(type)) // Bayern
					return 'R' + type;
				if ("Städtebahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Ortenau-S-Bahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Daadetalbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Mainschleifenbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Nordbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Harzer".Equals(type)) // Bayern
					return 'R' + type;
				if ("cantus".Equals(type)) // Bayern
					return 'R' + type;
				if ("DPF".Equals(type)) // Bayern, Vogtland-Express
					return 'R' + type;
				if ("Freiberger".Equals(type)) // Bayern
					return 'R' + type;
				if ("metronom".Equals(type)) // Bayern
					return 'R' + type;
				if ("Prignitzer".Equals(type)) // Bayern
					return 'R' + type;
				if ("Sächsisch-Oberlausitzer".Equals(type)) // Bayern
					return 'R' + type;
				if ("Ostseeland".Equals(type)) // Bayern
					return 'R' + type;
				if ("NordOstseeBahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("ELBE-WESER".Equals(type)) // Bayern
					return 'R' + type;
				if ("TRILEX".Equals(type)) // Bayern
					return 'R' + type;
				if ("Schleswig-Holstein-Bahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Vetter".Equals(type)) // Bayern
					return 'R' + type;
				if ("Dessau-Wörlitzer".Equals(type)) // Bayern
					return 'R' + type;
				if ("NATURPARK-EXPRESS".Equals(type)) // Bayern
					return 'R' + type;
				if ("Usedomer".Equals(type)) // Bayern
					return 'R' + type;
				if ("Märkische".Equals(type)) // Bayern
					return 'R' + type;
				if ("Vulkan-Eifel-Bahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Kandertalbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("RAD-WANDER-SHUTTLE".Equals(type)) // Bayern, Hohenzollerische Landesbahn
					return 'R' + type;
				if ("RADEXPRESS".Equals(type)) // Bayern, RADEXPRESS EYACHTÄLER
					return 'R' + type;
				if ("Dampfzug".Equals(type)) // Bayern
					return 'R' + type;
				if ("Wutachtalbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Grensland-Express".Equals(type)) // Bayern
					return 'R' + type;
				if ("Mecklenburgische".Equals(type)) // Bayern
					return 'R' + type;
				if ("Bentheimer".Equals(type)) // Bayern
					return 'R' + type;
				if ("Pressnitztalbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Regental".Equals(type)) // Bayern
					return 'R' + type;
				if ("Döllnitzbahn".Equals(type)) // Bayern
					return 'R' + type;
				if ("Schneeberg".Equals(type)) // VOR
					return 'R' + type;
				if ("FLZ".Equals(type)) // Stainzer Flascherlzug
					return 'R' + type;
				if ("FTB".Equals(type)) // Feistritztalbahn
					return 'R' + type;
				if ("DWE".Equals(type)) // Dessau-Wörlitzer Eisenbahn
					return 'R' + type;
				if ("KTB".Equals(type)) // Kandertalbahn
					return 'R' + type;
				if ("UEF".Equals(type)) // Ulmer Eisenbahnfreunde
					return 'R' + type;
				if ("CBC".Equals(type)) // City-Bahn Chemnitz
					return 'R' + type;
				if ("Regionalzug".Equals(type))
					return 'R' + type;
				if ("RR".Equals(type)) // RR 371 Horehronec / RR 404 Vltava / RR 922 Josef Skupa
					return 'R' + type;
				if ("ZAB1/766".Equals(type))
					return "R" + name;
				if ("ZAB2/768".Equals(type))
					return "R" + name;

				if ("BSB".Equals(type)) // Breisgau-S-Bahn
					return 'S' + str;
				if ("BSB-Zug".Equals(type)) // Breisgau-S-Bahn
					return 'S' + str;
				if ("Breisgau-S-Bahn".Equals(type)) // Bayern
					return 'S' + type;
				if ("RSB".Equals(type)) // Schnellbahn Wien
					return 'S' + type;
				if ("RER".Equals(type)) // Réseau Express Régional, Frankreich
					return 'S' + str;
				if ("LO".Equals(type)) // London Overground, GB
					return 'S' + str;
				if ("A".Equals(name) || "B".Equals(name) || "C".Equals(name)) // SES
					return 'S' + str;
				Match m = P_LINE_S.Match(name);
				if (m.Success)
					return 'S' + m.Groups[1].Value;

				if (P_LINE_U.Match(type).Success)
					return 'U' + str;
				if ("Underground".Equals(type)) // London Underground, GB
					return 'U' + str;
				if ("Millbrae / Richmond".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Richmond / Millbrae".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Fremont / RIchmond".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Richmond / Fremont".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Pittsburg Bay Point / SFO".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("SFO / Pittsburg Bay Point".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Dublin Pleasanton / Daly City".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Daly City / Dublin Pleasanton".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Fremont / Daly City".Equals(name)) // San Francisco, BART
					return 'U' + name;
				if ("Daly City / Fremont".Equals(name)) // San Francisco, BART
					return 'U' + name;

				if (type.Equals("RT")) // RegioTram
					return 'T' + str;
				if (type.Equals("STR")) // Nordhausen
					return 'T' + str;
				if ("California Cable Car".Equals(name)) // San Francisco
					return 'T' + name;
				if ("Muni".Equals(type)) // San Francisco
					return 'T' + name;
				if ("Cable".Equals(type)) // San Francisco
					return 'T' + name;
				if ("Muni Rail".Equals(trainName)) // San Francisco
					return 'T' + name;
				if ("Cable Car".Equals(trainName)) // San Francisco
					return 'T' + name;

				if ("BUS".Equals(type) || "Bus".Equals(type))
					return 'B' + str;
				if (P_LINE_SEV.Match(type).Success)
					return 'B' + str;
				if ("Bex".Equals(type)) // Bayern Express
					return 'B' + str;
				if ("Ersatzverkehr".Equals(type)) // Rhein-Ruhr
					return 'B' + str;
				if ("Bus replacement".Equals(trainName)) // Transport Line
					return 'B' + str;

				if ("HBL".Equals(type)) // Hamburg Hafenfähre
					return 'F' + str;

				if ("AST".Equals(type)) // Anruf-Sammel-Taxi
					return 'P' + str;

				if (type.Length == 0)
					return "?";
				//if (P_LINE_NUMBER.Match(type).Success)
				//    return "?" + webClient.firstNotEmpty(symbol, name);
				//if (P_LINE_Y.Match(name).Success)
				//    return "?" + webClient.firstNotEmpty(symbol, name);
				if ("Sonderverkehr Red Bull".Equals(name))
					return "?" + name;

				throw new Exception("cannot normalize mot='" + mot + "' symbol='" + symbol + "' name='" + name + "' long='" +
				                    longName
				                    + "' trainType='" + trainType + "' trainNum='" + trainNum + "' trainName='" + trainName +
				                    "' type='" + type + "' str='" + str
				                    + "'");
			}

			if (t == 1)
			{
				Match m = P_LINE_S.Match(name);
				if (m.Success)
					return 'S' + m.Groups[1].Value;
				else
					return 'S' + name;
			}

			if (t == 2)
				return 'U' + name;

			if (t == 3 || t == 4)
				return 'T' + name;

			if (t == 5 || t == 6 || t == 7 || t == 10)
			{
				if (name.Equals("Schienenersatzverkehr"))
					return "BSEV";
				else
					return 'B' + name;
			}

			if (t == 8)
				return 'C' + name;

			if (t == 9)
				return 'F' + name;

			//if (t == 11 || t == -1)
			//    return '?' + webClient.firstNotEmpty(symbol, name);

			throw new Exception("cannot normalize mot='" + mot + "' symbol='" + symbol + "' name='" + name + "' long='" +
			                    longName
			                    + "' trainType='" + trainType + "' trainNum='" + trainNum + "' trainName='" + trainName + "'");
		}

		public override DepartureMonitorRequest QueryDepartures(int stationId, int maxDepartures, bool equivs)
		{
			var parameters = new StringBuilder();
			AppendCommonRequestParams(parameters, "XML");
			parameters.Append("&type_dm=stop");
			parameters.Append("&name_dm=").Append(stationId);
			parameters.Append("&useRealtime=1");
			parameters.Append("&mode=direct");
			parameters.Append("&ptOptionsActive=1");
			parameters.Append("&deleteAssignedStops_dm=").Append(equivs ? '0' : '1');
			parameters.Append("&mergeDep=1"); // merge departures
			if (maxDepartures > 0)
				parameters.Append("&limit=").Append(maxDepartures);

			var uri = new StringBuilder(departureMonitorEndpoint);
			if (!httpPost)
				uri.Append(parameters);

			string resultString = WebClient.Scrape(uri.ToString(), null, requestUrlEncoding, null, 3);
			var itdRequest = Request.Deserialize(resultString);
			return itdRequest.DepartureMonitorRequest;
		}

		private StationDepartures FindStationDepartures(IEnumerable<StationDepartures> stationDepartures, int id)
		{
			return stationDepartures.FirstOrDefault(stationDeparture => stationDeparture.Location.Id == id);
		}

		protected string NormalizeLocationName(string name)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			return P_STATION_NAME_WHITESPACE.Replace(name, " ");
		}

		protected static double LatLonToDouble(int value)
		{
			return (double) value/1000000;
		}

		protected string xsltTripRequestParameters(Location from, Location via, Location to, DateTime date, bool dep,
		                                           int numConnections, List<Product> products, WalkSpeed walkSpeed,
		                                           Accessibility accessibility,
		                                           HashSet<Option> options)
		{
			string DATE_FORMAT = "yyyyMMdd";
			string TIME_FORMAT = "HHmm";

			var uri = new StringBuilder();
			AppendCommonRequestParams(uri, "XML");

			uri.Append("&sessionID=0");
			uri.Append("&requestID=0");
			uri.Append("&language=de");

			AppendCommonXsltTripRequest2Params(uri);

			AppendLocation(uri, from, "origin");
			AppendLocation(uri, to, "destination");
			if (via != null)
				AppendLocation(uri, via, "via");

			uri.Append("&itdDate=").Append(date.ToString(DATE_FORMAT));
			uri.Append("&itdTime=").Append(date.ToString(TIME_FORMAT));
			uri.Append("&itdTripDateTimeDepArr=").Append(dep ? "dep" : "arr");

			uri.Append("&calcNumberOfTrips=").Append(numConnections);

			uri.Append("&ptOptionsActive=1"); // enable public transport options
			uri.Append("&itOptionsActive=1"); // enable individual transport options
			//uri.Append("&changeSpeed=").Append(WALKSPEED_MAP.get(walkSpeed));

			if (accessibility == Accessibility.BARRIER_FREE)
				uri.Append("&imparedOptionsActive=1").Append("&wheelchair=on").Append("&noSolidStairs=on");
			else if (accessibility == Accessibility.LIMITED)
				uri.Append("&imparedOptionsActive=1")
				   .Append("&wheelchair=on")
				   .Append("&lowPlatformVhcl=on")
				   .Append("&noSolidStairs=on");

			if (products != null)
			{
				uri.Append("&includedMeans=checkbox");

				bool hasI = false;
				foreach (Product p in products)
				{
					if (p == Product.HIGH_SPEED_TRAIN || p == Product.REGIONAL_TRAIN)
					{
						uri.Append("&inclMOT_0=on");
						if (p == Product.HIGH_SPEED_TRAIN)
							hasI = true;
					}

					if (p == Product.SUBURBAN_TRAIN)
						uri.Append("&inclMOT_1=on");

					if (p == Product.SUBWAY)
						uri.Append("&inclMOT_2=on");

					if (p == Product.TRAM)
						uri.Append("&inclMOT_3=on&inclMOT_4=on");

					if (p == Product.BUS)
						uri.Append("&inclMOT_5=on&inclMOT_6=on&inclMOT_7=on");

					if (p == Product.ON_DEMAND)
						uri.Append("&inclMOT_10=on");

					if (p == Product.FERRY)
						uri.Append("&inclMOT_9=on");

					if (p == Product.CABLECAR)
						uri.Append("&inclMOT_8=on");
				}

				uri.Append("&inclMOT_11=on"); // TODO always show 'others', for now

				// workaround for highspeed trains: fails when you want highspeed, but not regional
				if (!hasI)
					uri.Append("&lineRestriction=403"); // means: all but ice
			}

			if (options != null && options.Contains(Option.BIKE))
				uri.Append("&bikeTakeAlong=1");

			uri.Append("&locationServerActive=1");
			uri.Append("&useRealtime=1");
			uri.Append("&useProxFootSearch=1"); // walk if it makes journeys quicker
			uri.Append("&nextDepsPerLeg=1"); // next departure in case previous was missed

			return uri.ToString();
		}

		private string CommandLink(string sessionId, string requestId)
		{
			var uri = new StringBuilder(tripEndpoint);

			uri.Append("?sessionID=").Append(sessionId);
			uri.Append("&requestID=").Append(requestId);
			AppendCommonXsltTripRequest2Params(uri);

			return uri.ToString();
		}

		private static void AppendCommonXsltTripRequest2Params(StringBuilder uri)
		{
			uri.Append("&coordListOutputFormat=string");
			uri.Append("&calcNumberOfTrips=4");
		}

		public override TripRequest QueryConnections(Location from, Location via, Location to, DateTime date, bool dep,
		                                             int numConnections, List<Product> products, WalkSpeed walkSpeed,
		                                             Accessibility accessibility,
		                                             HashSet<Option> options)
		{
			string parameters = xsltTripRequestParameters(from, via, to, date, dep, numConnections, products, walkSpeed,
			                                              accessibility, options);

			var uri = new StringBuilder(tripEndpoint);
			if (!httpPost)
				uri.Append(parameters);

			string resultString = WebClient.Scrape(uri.ToString(), httpPost ? parameters.Substring(1) : null,
			                                       requestUrlEncoding, null, 3);

			var itdRequest = Request.Deserialize(resultString);
			return itdRequest.TripRequest;
		}

		public override QueryConnectionsResult QueryMoreConnections(IQueryConnectionsContext contextObj, bool later,
		                                                            int numConnections)
		{
			var context = (InnerContext) contextObj;
			string commandUri = context.Context;
			var uri = new StringBuilder(commandUri);
			uri.Append("&command=").Append(later ? "tripNext" : "tripPrev");

			//InputStream is = null;
			//try
			//{
			//    is = new BufferedInputStream(webClient.scrapeInputStream(uri.tostring(), null, null, httpRefererTrip, "NSC_", 3));
			//    is.mark(512);

			//    return queryConnections(uri.tostring(), is);
			//}
			//catch (XmlPullParserException x)
			//{
			//    throw new ParserException(x);
			//}
			//catch (ProtocolException x) // must be html content
			//{
			//    is.reset();
			//    BufferedReader reader = new BufferedReader(new InputStreamReader(is));

			//    string line;
			//    while ((line = reader.readLine()) != null)
			//        if (P_SESSION_EXPIRED.matcher(line).find())
			//            throw new SessionExpiredException();

			//    throw x;
			//}
			//catch (RuntimeException x)
			//{
			//    throw new RuntimeException("uncategorized problem while processing " + uri, x);
			//}
			//finally
			//{
			//    if (is != null)
			//        is.close();
			//}

			return null;
		}

		private QueryConnectionsResult QueryConnections(string uri, Stream inStream)
		{
			// System.out.println(uri);

			//XmlPullParser pp = parserFactory.newPullParser();
			//pp.setInput(inStream, null);
			//ResultHeader header = enterItdRequest(pp);
			//Object context = header.context;

			//if (XmlPullUtil.test(pp, "itdLayoutParams"))
			//    XmlPullUtil.next(pp);

			//XmlPullUtil.require(pp, "itdTripRequest");
			//string requestId = XmlPullUtil.attr(pp, "requestID");
			//XmlPullUtil.enter(pp, "itdTripRequest");

			//if (XmlPullUtil.test(pp, "itdMessage"))
			//{
			//    int code = XmlPullUtil.intAttr(pp, "code");
			//    if (code == -4000) // no connection
			//        return new QueryConnectionsResult(header, QueryConnectionsResult.Status.NO_CONNECTIONS);
			//    XmlPullUtil.next(pp);
			//}
			//if (XmlPullUtil.test(pp, "itdPrintConfiguration"))
			//    XmlPullUtil.next(pp);
			//if (XmlPullUtil.test(pp, "itdAddress"))
			//    XmlPullUtil.next(pp);

			//// parse odv name elements
			//List<Location> ambiguousFrom = null, ambiguousTo = null, ambiguousVia = null;
			//Location from = null, via = null, to = null;

			//while (XmlPullUtil.test(pp, "itdOdv"))
			//{
			//    string usage = XmlPullUtil.attr(pp, "usage");
			//    XmlPullUtil.enter(pp, "itdOdv");

			//    string place = processItdOdvPlace(pp);

			//    if (!XmlPullUtil.test(pp, "itdOdvName"))
			//        throw new IllegalStateException("cannot find <itdOdvName /> inside " + usage);
			//    string nameState = XmlPullUtil.attr(pp, "state");
			//    XmlPullUtil.enter(pp, "itdOdvName");
			//    if (XmlPullUtil.test(pp, "itdMessage"))
			//        XmlPullUtil.next(pp);

			//    if ("list".Equals(nameState))
			//    {
			//        if ("origin".Equals(usage))
			//        {
			//            ambiguousFrom = new ArrayList<Location>();
			//            while (XmlPullUtil.test(pp, "odvNameElem"))
			//                ambiguousFrom.add(processOdvNameElem(pp, place));
			//        }
			//        else if ("via".Equals(usage))
			//        {
			//            ambiguousVia = new ArrayList<Location>();
			//            while (XmlPullUtil.test(pp, "odvNameElem"))
			//                ambiguousVia.add(processOdvNameElem(pp, place));
			//        }
			//        else if ("destination".Equals(usage))
			//        {
			//            ambiguousTo = new ArrayList<Location>();
			//            while (XmlPullUtil.test(pp, "odvNameElem"))
			//                ambiguousTo.add(processOdvNameElem(pp, place));
			//        }
			//        else
			//        {
			//            throw new IllegalStateException("unknown usage: " + usage);
			//        }
			//    }
			//    else if ("identified".Equals(nameState))
			//    {
			//        if (!XmlPullUtil.test(pp, "odvNameElem"))
			//            throw new IllegalStateException("cannot find <odvNameElem /> inside " + usage);

			//        if ("origin".Equals(usage))
			//            from = processOdvNameElem(pp, place);
			//        else if ("via".Equals(usage))
			//            via = processOdvNameElem(pp, place);
			//        else if ("destination".Equals(usage))
			//            to = processOdvNameElem(pp, place);
			//        else
			//            throw new IllegalStateException("unknown usage: " + usage);
			//    }
			//    else if ("notidentified".Equals(nameState))
			//    {
			//        if ("origin".Equals(usage))
			//            return new QueryConnectionsResult(header, QueryConnectionsResult.Status.UNKNOWN_FROM);
			//        else if ("via".Equals(usage))
			//            // return new QueryConnectionsResult(header, QueryConnectionsResult.Status.UNKNOWN_VIA);
			//            throw new UnsupportedOperationException();
			//        else if ("destination".Equals(usage))
			//            return new QueryConnectionsResult(header, QueryConnectionsResult.Status.UNKNOWN_TO);
			//        else
			//            throw new IllegalStateException("unknown usage: " + usage);
			//    }
			//    XmlPullUtil.exit(pp, "itdOdvName");
			//    XmlPullUtil.exit(pp, "itdOdv");
			//}

			//if (ambiguousFrom != null || ambiguousTo != null || ambiguousVia != null)
			//    return new QueryConnectionsResult(header, ambiguousFrom, ambiguousVia, ambiguousTo);

			//XmlPullUtil.enter(pp, "itdTripDateTime");
			//XmlPullUtil.enter(pp, "itdDateTime");
			//XmlPullUtil.require(pp, "itdDate");
			//if (!pp.isEmptyElementTag())
			//{
			//    XmlPullUtil.enter(pp, "itdDate");
			//    if (XmlPullUtil.test(pp, "itdMessage"))
			//    {
			//        string message = XmlPullUtil.nextText(pp, null, "itdMessage");

			//        if ("invalid date".Equals(message))
			//            return new QueryConnectionsResult(header, QueryConnectionsResult.Status.INVALID_DATE);
			//        else
			//            throw new IllegalStateException("unknown message: " + message);
			//    }
			//    XmlPullUtil.exit(pp, "itdDate");
			//}
			//else
			//{
			//    XmlPullUtil.next(pp);
			//}
			//XmlPullUtil.exit(pp, "itdDateTime");

			//Calendar time = new GregorianCalendar(timeZone());
			//List<Connection> connections = new ArrayList<Connection>();

			//if (XmlPullUtil.jumpToStartTag(pp, null, "itdRouteList"))
			//{
			//    XmlPullUtil.enter(pp, "itdRouteList");

			//    while (XmlPullUtil.test(pp, "itdRoute"))
			//    {
			//        string id = useRouteIndexAsConnectionId ? pp.getAttributeValue(null, "routeIndex") + "-"
			//                + pp.getAttributeValue(null, "routeTripIndex") : null;
			//        int numChanges = XmlPullUtil.intAttr(pp, "changes");
			//        XmlPullUtil.enter(pp, "itdRoute");

			//        while (XmlPullUtil.test(pp, "itdDateTime"))
			//            XmlPullUtil.next(pp);
			//        if (XmlPullUtil.test(pp, "itdMapItemList"))
			//            XmlPullUtil.next(pp);

			//        XmlPullUtil.enter(pp, "itdPartialRouteList");
			//        List<Connection.Part> parts = new LinkedList<Connection.Part>();
			//        Location firstDepartureLocation = null;
			//        Location lastArrivalLocation = null;

			//        bool cancelled = false;

			//        while (XmlPullUtil.test(pp, "itdPartialRoute"))
			//        {
			//            int distance = XmlPullUtil.optIntAttr(pp, "distance", 0);
			//            XmlPullUtil.enter(pp, "itdPartialRoute");

			//            XmlPullUtil.test(pp, "itdPoint");
			//            if (!"departure".Equals(pp.getAttributeValue(null, "usage")))
			//                throw new IllegalStateException();
			//            Location departureLocation = processItdPointAttributes(pp);
			//            if (firstDepartureLocation == null)
			//                firstDepartureLocation = departureLocation;
			//            string departurePosition;
			//            if (!suppressPositions)
			//                departurePosition = normalizePlatform(pp.getAttributeValue(null, "platform"), pp.getAttributeValue(null, "platformName"));
			//            else
			//                departurePosition = null;
			//            XmlPullUtil.enter(pp, "itdPoint");
			//            if (XmlPullUtil.test(pp, "itdMapItemList"))
			//                XmlPullUtil.next(pp);
			//            XmlPullUtil.require(pp, "itdDateTime");
			//            processItdDateTime(pp, time);
			//            Date departureTime = time.getTime();
			//            Date departureTargetTime;
			//            if (XmlPullUtil.test(pp, "itdDateTimeTarget"))
			//            {
			//                processItdDateTime(pp, time);
			//                departureTargetTime = time.getTime();
			//            }
			//            else
			//            {
			//                departureTargetTime = null;
			//            }
			//            XmlPullUtil.exit(pp, "itdPoint");

			//            XmlPullUtil.test(pp, "itdPoint");
			//            if (!"arrival".Equals(pp.getAttributeValue(null, "usage")))
			//                throw new IllegalStateException();
			//            Location arrivalLocation = processItdPointAttributes(pp);
			//            lastArrivalLocation = arrivalLocation;
			//            string arrivalPosition;
			//            if (!suppressPositions)
			//                arrivalPosition = normalizePlatform(pp.getAttributeValue(null, "platform"), pp.getAttributeValue(null, "platformName"));
			//            else
			//                arrivalPosition = null;
			//            XmlPullUtil.enter(pp, "itdPoint");
			//            if (XmlPullUtil.test(pp, "itdMapItemList"))
			//                XmlPullUtil.next(pp);
			//            XmlPullUtil.require(pp, "itdDateTime");
			//            processItdDateTime(pp, time);
			//            Date arrivalTime = time.getTime();
			//            Date arrivalTargetTime;
			//            if (XmlPullUtil.test(pp, "itdDateTimeTarget"))
			//            {
			//                processItdDateTime(pp, time);
			//                arrivalTargetTime = time.getTime();
			//            }
			//            else
			//            {
			//                arrivalTargetTime = null;
			//            }
			//            XmlPullUtil.exit(pp, "itdPoint");

			//            XmlPullUtil.test(pp, "itdMeansOfTransport");
			//            string productName = pp.getAttributeValue(null, "productName");
			//            if ("Fussweg".Equals(productName) || "Taxi".Equals(productName))
			//            {
			//                int min = (int) (arrivalTime.getTime() - departureTime.getTime()) / 1000 / 60;
			//                bool transfer = "Taxi".Equals(productName);

			//                XmlPullUtil.enter(pp, "itdMeansOfTransport");
			//                XmlPullUtil.exit(pp, "itdMeansOfTransport");

			//                if (XmlPullUtil.test(pp, "itdStopSeq"))
			//                    XmlPullUtil.next(pp);

			//                if (XmlPullUtil.test(pp, "itdFootPathInfo"))
			//                    XmlPullUtil.next(pp);

			//                List<Point> path = null;
			//                if (XmlPullUtil.test(pp, "itdPathCoordinates"))
			//                    path = processItdPathCoordinates(pp);

			//                if (parts.size() > 0 && parts.get(parts.size() - 1) instanceof Connection.Footway)
			//                {
			//                    Connection.Footway lastFootway = (Connection.Footway) parts.remove(parts.size() - 1);
			//                    if (path != null && lastFootway.path != null)
			//                        path.addAll(0, lastFootway.path);
			//                    parts.add(new Connection.Footway(lastFootway.min + min, distance, lastFootway.transfer || transfer,
			//                            lastFootway.departure, arrivalLocation, path));
			//                }
			//                else
			//                {
			//                    parts.add(new Connection.Footway(min, distance, transfer, departureLocation, arrivalLocation, path));
			//                }
			//            }
			//            else if ("gesicherter Anschluss".Equals(productName) || "nicht umsteigen".Equals(productName)) // type97
			//            {
			//                // ignore

			//                XmlPullUtil.enter(pp, "itdMeansOfTransport");
			//                XmlPullUtil.exit(pp, "itdMeansOfTransport");
			//            }
			//            else
			//            {
			//                string destinationName = normalizeLocationName(pp.getAttributeValue(null, "destination"));
			//                string destinationIdStr = pp.getAttributeValue(null, "destID");
			//                int destinationId = (destinationIdStr != null && destinationIdStr.length() > 0) ? Integer.parseInt(destinationIdStr)
			//                        : 0;
			//                Location destination = new Location(destinationId > 0 ? LocationType.STATION : LocationType.ANY,
			//                        destinationId > 0 ? destinationId : 0, null, destinationName);
			//                string lineLabel;
			//                string motSymbol = pp.getAttributeValue(null, "symbol");
			//                if ("AST".Equals(motSymbol))
			//                {
			//                    lineLabel = "BAST";
			//                }
			//                else
			//                {
			//                    string motType = pp.getAttributeValue(null, "motType");
			//                    string motShortName = pp.getAttributeValue(null, "shortname");
			//                    string motName = pp.getAttributeValue(null, "name");
			//                    string motTrainName = pp.getAttributeValue(null, "trainName");
			//                    string motTrainType = pp.getAttributeValue(null, "trainType");

			//                    lineLabel = parseLine(motType, motSymbol, motShortName, motName, motTrainType, motShortName, motTrainName);
			//                }
			//                XmlPullUtil.enter(pp, "itdMeansOfTransport");
			//                XmlPullUtil.require(pp, "motDivaParams");
			//                string lineId = XmlPullUtil.attr(pp, "network") + ':' + XmlPullUtil.attr(pp, "line") + ':'
			//                        + XmlPullUtil.attr(pp, "supplement") + ':' + XmlPullUtil.attr(pp, "direction") + ':'
			//                        + XmlPullUtil.attr(pp, "project");
			//                XmlPullUtil.exit(pp, "itdMeansOfTransport");

			//                Integer departureDelay;
			//                Integer arrivalDelay;
			//                if (XmlPullUtil.test(pp, "itdRBLControlled"))
			//                {
			//                    departureDelay = XmlPullUtil.optIntAttr(pp, "delayMinutes", 0);
			//                    arrivalDelay = XmlPullUtil.optIntAttr(pp, "delayMinutesArr", 0);

			//                    cancelled |= (departureDelay == -9999 || arrivalDelay == -9999);

			//                    XmlPullUtil.next(pp);
			//                }
			//                else
			//                {
			//                    departureDelay = null;
			//                    arrivalDelay = null;
			//                }

			//                bool lowFloorVehicle = false;
			//                string message = null;
			//                if (XmlPullUtil.test(pp, "itdInfoTextList"))
			//                {
			//                    if (!pp.isEmptyElementTag())
			//                    {
			//                        XmlPullUtil.enter(pp, "itdInfoTextList");
			//                        while (XmlPullUtil.test(pp, "infoTextListElem"))
			//                        {
			//                            XmlPullUtil.enter(pp, "infoTextListElem");
			//                            string text = pp.getText();
			//                            if ("Niederflurwagen soweit verfügbar".Equals(text)) // KVV
			//                                lowFloorVehicle = true;
			//                            else if (text != null && text.toLowerCase().contains("ruf")) // RufBus, RufTaxi
			//                                message = text;
			//                            XmlPullUtil.exit(pp, "infoTextListElem");
			//                        }
			//                        XmlPullUtil.exit(pp, "itdInfoTextList");
			//                    }
			//                    else
			//                    {
			//                        XmlPullUtil.next(pp);
			//                    }
			//                }

			//                if (XmlPullUtil.test(pp, "itdFootPathInfo"))
			//                    XmlPullUtil.next(pp);
			//                if (XmlPullUtil.test(pp, "infoLink"))
			//                    XmlPullUtil.next(pp);

			//                List<Stop> intermediateStops = null;
			//                if (XmlPullUtil.test(pp, "itdStopSeq"))
			//                {
			//                    XmlPullUtil.enter(pp, "itdStopSeq");
			//                    intermediateStops = new LinkedList<Stop>();
			//                    while (XmlPullUtil.test(pp, "itdPoint"))
			//                    {
			//                        Location stopLocation = processItdPointAttributes(pp);

			//                        string stopPosition;
			//                        if (!suppressPositions)
			//                            stopPosition = normalizePlatform(pp.getAttributeValue(null, "platform"),
			//                                    pp.getAttributeValue(null, "platformName"));
			//                        else
			//                            stopPosition = null;

			//                        XmlPullUtil.enter(pp, "itdPoint");
			//                        XmlPullUtil.require(pp, "itdDateTime");

			//                        Date plannedStopArrivalTime;
			//                        Date predictedStopArrivalTime;
			//                        if (processItdDateTime(pp, time))
			//                        {
			//                            plannedStopArrivalTime = time.getTime();
			//                            if (arrivalDelay != null)
			//                            {
			//                                time.add(Calendar.MINUTE, arrivalDelay);
			//                                predictedStopArrivalTime = time.getTime();
			//                            }
			//                            else
			//                            {
			//                                predictedStopArrivalTime = null;
			//                            }
			//                        }
			//                        else
			//                        {
			//                            plannedStopArrivalTime = null;
			//                            predictedStopArrivalTime = null;
			//                        }

			//                        Date plannedStopDepartureTime;
			//                        Date predictedStopDepartureTime;
			//                        if (XmlPullUtil.test(pp, "itdDateTime") && processItdDateTime(pp, time))
			//                        {
			//                            plannedStopDepartureTime = time.getTime();
			//                            if (departureDelay != null)
			//                            {
			//                                time.add(Calendar.MINUTE, departureDelay);
			//                                predictedStopDepartureTime = time.getTime();
			//                            }
			//                            else
			//                            {
			//                                predictedStopDepartureTime = null;
			//                            }
			//                        }
			//                        else
			//                        {
			//                            plannedStopDepartureTime = null;
			//                            predictedStopDepartureTime = null;
			//                        }

			//                        Stop stop = new Stop(stopLocation, plannedStopArrivalTime, predictedStopArrivalTime, stopPosition, null,
			//                                plannedStopDepartureTime, predictedStopDepartureTime, stopPosition, null);

			//                        intermediateStops.add(stop);

			//                        XmlPullUtil.exit(pp, "itdPoint");
			//                    }
			//                    XmlPullUtil.exit(pp, "itdStopSeq");

			//                    // remove first and last, because they are not intermediate
			//                    int size = intermediateStops.size();
			//                    if (size >= 2)
			//                    {
			//                        if (intermediateStops.get(size - 1).location.id != arrivalLocation.id)
			//                            throw new IllegalStateException();
			//                        intermediateStops.remove(size - 1);

			//                        if (intermediateStops.get(0).location.id != departureLocation.id)
			//                            throw new IllegalStateException();
			//                        intermediateStops.remove(0);
			//                    }
			//                }

			//                List<Point> path = null;
			//                if (XmlPullUtil.test(pp, "itdPathCoordinates"))
			//                    path = processItdPathCoordinates(pp);

			//                bool wheelChairAccess = false;
			//                if (XmlPullUtil.test(pp, "genAttrList"))
			//                {
			//                    XmlPullUtil.enter(pp, "genAttrList");
			//                    while (XmlPullUtil.test(pp, "genAttrElem"))
			//                    {
			//                        XmlPullUtil.enter(pp, "genAttrElem");
			//                        XmlPullUtil.enter(pp, "name");
			//                        string name = pp.getText();
			//                        XmlPullUtil.exit(pp, "name");
			//                        XmlPullUtil.enter(pp, "value");
			//                        string value = pp.getText();
			//                        XmlPullUtil.exit(pp, "value");
			//                        XmlPullUtil.exit(pp, "genAttrElem");

			//                        // System.out.println("genAttrElem: name='" + name + "' value='" + value + "'");

			//                        if ("PlanWheelChairAccess".Equals(name) && "1".Equals(value))
			//                            wheelChairAccess = true;
			//                    }
			//                    XmlPullUtil.exit(pp, "genAttrList");
			//                }

			//                if (XmlPullUtil.test(pp, "nextDeps"))
			//                {
			//                    XmlPullUtil.enter(pp, "nextDeps");
			//                    while (XmlPullUtil.test(pp, "itdDateTime"))
			//                    {
			//                        processItdDateTime(pp, time);
			//                        /* Date nextDepartureTime = */time.getTime();
			//                    }
			//                    XmlPullUtil.exit(pp, "nextDeps");
			//                }

			//                Set<Line.Attr> lineAttrs = new HashSet<Line.Attr>();
			//                if (wheelChairAccess || lowFloorVehicle)
			//                    lineAttrs.add(Line.Attr.WHEEL_CHAIR_ACCESS);
			//                Line line = new Line(lineId, lineLabel, lineStyle(lineLabel), lineAttrs);

			//                Stop departure = new Stop(departureLocation, true, departureTargetTime != null ? departureTargetTime : departureTime,
			//                        departureTargetTime != null ? departureTime : null, departurePosition, null);
			//                Stop arrival = new Stop(arrivalLocation, false, arrivalTargetTime != null ? arrivalTargetTime : arrivalTime,
			//                        arrivalTime != null ? arrivalTime : null, arrivalPosition, null);

			//                parts.add(new Connection.Trip(line, destination, departure, arrival, intermediateStops, path, message));
			//            }

			//            XmlPullUtil.exit(pp, "itdPartialRoute");
			//        }

			//        XmlPullUtil.exit(pp, "itdPartialRouteList");

			//        List<Fare> fares = new ArrayList<Fare>(2);
			//        if (XmlPullUtil.test(pp, "itdFare"))
			//        {
			//            if (!pp.isEmptyElementTag())
			//            {
			//                XmlPullUtil.enter(pp, "itdFare");
			//                if (XmlPullUtil.test(pp, "itdSingleTicket"))
			//                {
			//                    string net = XmlPullUtil.attr(pp, "net");
			//                    Currency currency = parseCurrency(XmlPullUtil.attr(pp, "currency"));
			//                    string fareAdult = XmlPullUtil.attr(pp, "fareAdult");
			//                    string fareChild = XmlPullUtil.attr(pp, "fareChild");
			//                    string unitName = XmlPullUtil.attr(pp, "unitName");
			//                    string unitsAdult = XmlPullUtil.attr(pp, "unitsAdult");
			//                    string unitsChild = XmlPullUtil.attr(pp, "unitsChild");
			//                    string levelAdult = pp.getAttributeValue(null, "levelAdult");
			//                    bool hasLevelAdult = levelAdult != null && levelAdult.length() > 0;
			//                    string levelChild = pp.getAttributeValue(null, "levelChild");
			//                    bool hasLevelChild = levelChild != null && levelChild.length() > 0;
			//                    if (fareAdult != null && fareAdult.length() > 0)
			//                        fares.add(new Fare(net, Type.ADULT, currency, Float.parseFloat(fareAdult), hasLevelAdult ? null : unitName,
			//                                hasLevelAdult ? levelAdult : unitsAdult));
			//                    if (fareChild != null && fareChild.length() > 0)
			//                        fares.add(new Fare(net, Type.CHILD, currency, Float.parseFloat(fareChild), hasLevelChild ? null : unitName,
			//                                hasLevelChild ? levelChild : unitsChild));

			//                    if (!pp.isEmptyElementTag())
			//                    {
			//                        XmlPullUtil.enter(pp, "itdSingleTicket");
			//                        if (XmlPullUtil.test(pp, "itdGenericTicketList"))
			//                        {
			//                            XmlPullUtil.enter(pp, "itdGenericTicketList");
			//                            while (XmlPullUtil.test(pp, "itdGenericTicketGroup"))
			//                            {
			//                                Fare fare = processItdGenericTicketGroup(pp, net, currency);
			//                                if (fare != null)
			//                                    fares.add(fare);
			//                            }
			//                            XmlPullUtil.exit(pp, "itdGenericTicketList");
			//                        }
			//                        XmlPullUtil.exit(pp, "itdSingleTicket");
			//                    }
			//                    else
			//                    {
			//                        XmlPullUtil.next(pp);
			//                    }
			//                }
			//                XmlPullUtil.exit(pp, "itdFare");
			//            }
			//            else
			//            {
			//                XmlPullUtil.next(pp);
			//            }
			//        }

			//        XmlPullUtil.exit(pp, "itdRoute");

			//        Connection connection = new Connection(id, firstDepartureLocation, lastArrivalLocation, parts, fares.isEmpty() ? null : fares,
			//                null, numChanges);

			//        if (!cancelled)
			//            connections.add(connection);
			//    }

			//    XmlPullUtil.exit(pp, "itdRouteList");

			//    return new QueryConnectionsResult(header, uri, from, via, to, new Context(commandLink((string) context, requestId)), connections);
			//}
			//else
			//{
			//    return new QueryConnectionsResult(header, QueryConnectionsResult.Status.NO_CONNECTIONS);
			//}

			return null;
		}


		private void AppendLocation(StringBuilder uri, Location location, string paramSuffix)
		{
			if (canAcceptPoiId && location.Type == LocationType.POI && location.HasId())
			{
				uri.Append("&type_").Append(paramSuffix).Append("=poiID");
				uri.Append("&name_").Append(paramSuffix).Append("=").Append(location.Id);
			}
			else if ((location.Type == LocationType.POI || location.Type == LocationType.ADDRESS) && location.HasLocation())
			{
				uri.Append("&type_").Append(paramSuffix).Append("=coord");
				uri.Append("&name_").Append(paramSuffix).Append("=")
				   .Append(string.Format("{0}:{1}", location.Lon/1E6, location.Lat/1E6)).Append(":WGS84");
			}
			else
			{
				uri.Append("&type_").Append(paramSuffix).Append("=").Append(LocationTypeValue(location));
				uri.Append("&name_")
				   .Append(paramSuffix)
				   .Append("=")
				   .Append(WebClient.UrlEncode(LocationValue(location), requestUrlEncoding));
			}
		}

		protected static string LocationTypeValue(Location location)
		{
			LocationType type = location.Type;
			if (type == LocationType.STATION)
				return "stop";
			if (type == LocationType.ADDRESS)
				return "any"; // strange, matches with anyObjFilter
			if (type == LocationType.POI)
				return "poi";
			if (type == LocationType.ANY)
				return "any";
			throw new Exception(type.ToString());
		}

		protected static string LocationValue(Location location)
		{
			if ((location.Type == LocationType.STATION || location.Type == LocationType.POI) && location.HasId())
				return location.Id.ToString(CultureInfo.InvariantCulture);

			return location.Name;
		}

		private class InnerContext : IQueryConnectionsContext
		{
			private InnerContext(string context)
			{
				Context = context;
			}

			public string Context { get; private set; }

			public bool CanQueryLater()
			{
				return Context != null;
			}

			public bool CanQueryEarlier()
			{
				return false; // TODO enable earlier querying
			}

			public override string ToString()
			{
				return GetType().Name + "[" + Context + "]";
			}
		}
	}
}