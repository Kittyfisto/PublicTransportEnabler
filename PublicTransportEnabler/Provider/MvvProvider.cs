using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PublicTransportEnabler.DataModel;
using PublicTransportEnabler.Enum;
using PublicTransportEnabler.Model;
using Location = PublicTransportEnabler.Model.Location;

namespace PublicTransportEnabler.Provider
{
	public class MvvProvider : AbstractEfaProvider
	{
		private static string API_BASE = "http://efa.mvv-muenchen.de/mobile/";
		private static readonly Dictionary<string, Style> Lines = new Dictionary<string, Style>();

		static MvvProvider()
		{
			Lines.Add("SS1", new Style(Style.ParseColor("#00ccff"), Style.WHITE));
			Lines.Add("SS2", new Style(Style.ParseColor("#66cc00"), Style.WHITE));
			Lines.Add("SS3", new Style(Style.ParseColor("#880099"), Style.WHITE));
			Lines.Add("SS4", new Style(Style.ParseColor("#ff0033"), Style.WHITE));
			Lines.Add("SS6", new Style(Style.ParseColor("#00aa66"), Style.WHITE));
			Lines.Add("SS7", new Style(Style.ParseColor("#993333"), Style.WHITE));
			Lines.Add("SS8", new Style(Style.BLACK, Style.ParseColor("#ffcc00")));
			Lines.Add("SS20", new Style(Style.BLACK, Style.ParseColor("#ffaaaa")));
			Lines.Add("SS27", new Style(Style.ParseColor("#ffaaaa"), Style.WHITE));
			Lines.Add("SA", new Style(Style.ParseColor("#231f20"), Style.WHITE));

			Lines.Add("T12", new Style(Style.ParseColor("#883388"), Style.WHITE));
			Lines.Add("T15", new Style(Style.ParseColor("#3366CC"), Style.WHITE));
			Lines.Add("T16", new Style(Style.ParseColor("#CC8833"), Style.WHITE));
			Lines.Add("T17", new Style(Style.ParseColor("#993333"), Style.WHITE));
			Lines.Add("T18", new Style(Style.ParseColor("#66bb33"), Style.WHITE));
			Lines.Add("T19", new Style(Style.ParseColor("#cc0000"), Style.WHITE));
			Lines.Add("T20", new Style(Style.ParseColor("#00bbee"), Style.WHITE));
			Lines.Add("T21", new Style(Style.ParseColor("#33aa99"), Style.WHITE));
			Lines.Add("T23", new Style(Style.ParseColor("#fff000"), Style.WHITE));
			Lines.Add("T25", new Style(Style.ParseColor("#ff9999"), Style.WHITE));
			Lines.Add("T27", new Style(Style.ParseColor("#ff6600"), Style.WHITE));
			Lines.Add("TN17", new Style(Style.ParseColor("#999999"), Style.ParseColor("#ffff00")));
			Lines.Add("TN19", new Style(Style.ParseColor("#999999"), Style.ParseColor("#ffff00")));
			Lines.Add("TN20", new Style(Style.ParseColor("#999999"), Style.ParseColor("#ffff00")));
			Lines.Add("TN27", new Style(Style.ParseColor("#999999"), Style.ParseColor("#ffff00")));

			Lines.Add("UU1", new Style(Style.ParseColor("#227700"), Style.WHITE));
			Lines.Add("UU2", new Style(Style.ParseColor("#bb0000"), Style.WHITE));
			Lines.Add("UU2E", new Style(Style.ParseColor("#bb0000"), Style.WHITE));
			Lines.Add("UU3", new Style(Style.ParseColor("#ee8800"), Style.WHITE));
			Lines.Add("UU4", new Style(Style.ParseColor("#00ccaa"), Style.WHITE));
			Lines.Add("UU5", new Style(Style.ParseColor("#bb7700"), Style.WHITE));
			Lines.Add("UU6", new Style(Style.ParseColor("#0000cc"), Style.WHITE));
			//LINES.Add("UU7", new Style(Style.ParseColor("#227700"), Style.ParseColor("#bb0000"), Style.WHITE, 0));
		}

		public MvvProvider(IWebClient webClient)
			: base(API_BASE, webClient)
		{
			SetNeedsSpEncId(true);
		}

		public override NetworkId Id()
		{
			return NetworkId.MVV;
		}

		public override bool HasCapabilities(IEnumerable<Capability> capabilities)
		{
			return
				capabilities.Any(
					capability =>
					capability == Capability.AUTOCOMPLETE_ONE_LINE || capability == Capability.DEPARTURES ||
					capability == Capability.CONNECTIONS);
		}


		public override Task<StopFinderRequest> AutocompleteStationsAsync(string constraint)
		{
			//return this.JsonStopfinderRequest(new Location(LocationType.ANY, 0, null, constraint));
			return XmlStopfinderRequest(new Location(LocationType.ANY, 0, null, constraint));
		}


		public override Style LineStyle(string line)
		{
			Style style = Lines[line];
			if (style != null)
				return style;

			return base.LineStyle(line);
		}
	}
}