using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PublicTransportEnabler.DataModel;
using PublicTransportEnabler.Enum;
using PublicTransportEnabler.Model;
using Location = PublicTransportEnabler.Model.Location;

namespace PublicTransportEnabler.Provider
{
	public class VrrProvider : AbstractEfaProvider
	{
		private static string API_BASE = "http://app.vrr.de/standard/";
		private static readonly Dictionary<String, Style> LINES = new Dictionary<String, Style>();

		static VrrProvider()
		{
			// Busse Bonn
			LINES.Add("B63", new Style(Style.ParseColor("#0065ae"), Style.WHITE));
			LINES.Add("B16", new Style(Style.ParseColor("#0065ae"), Style.WHITE));
			LINES.Add("B66", new Style(Style.ParseColor("#0065ae"), Style.WHITE));
			LINES.Add("B67", new Style(Style.ParseColor("#0065ae"), Style.WHITE));
			LINES.Add("B68", new Style(Style.ParseColor("#0065ae"), Style.WHITE));
			LINES.Add("B18", new Style(Style.ParseColor("#0065ae"), Style.WHITE));
			LINES.Add("B61", new Style(Style.ParseColor("#e4000b"), Style.WHITE));
			LINES.Add("B62", new Style(Style.ParseColor("#e4000b"), Style.WHITE));
			LINES.Add("B65", new Style(Style.ParseColor("#e4000b"), Style.WHITE));
			LINES.Add("BSB55", new Style(Style.ParseColor("#00919e"), Style.WHITE));
			LINES.Add("BSB60", new Style(Style.ParseColor("#8f9867"), Style.WHITE));
			LINES.Add("BSB69", new Style(Style.ParseColor("#db5f1f"), Style.WHITE));
			LINES.Add("B529", new Style(Style.ParseColor("#2e2383"), Style.WHITE));
			LINES.Add("B537", new Style(Style.ParseColor("#2e2383"), Style.WHITE));
			LINES.Add("B541", new Style(Style.ParseColor("#2e2383"), Style.WHITE));
			LINES.Add("B550", new Style(Style.ParseColor("#2e2383"), Style.WHITE));
			LINES.Add("B163", new Style(Style.ParseColor("#2e2383"), Style.WHITE));
			LINES.Add("B551", new Style(Style.ParseColor("#2e2383"), Style.WHITE));
			LINES.Add("B600", new Style(Style.ParseColor("#817db7"), Style.WHITE));
			LINES.Add("B601", new Style(Style.ParseColor("#831b82"), Style.WHITE));
			LINES.Add("B602", new Style(Style.ParseColor("#dd6ba6"), Style.WHITE));
			LINES.Add("B603", new Style(Style.ParseColor("#e6007d"), Style.WHITE));
			LINES.Add("B604", new Style(Style.ParseColor("#009f5d"), Style.WHITE));
			LINES.Add("B605", new Style(Style.ParseColor("#007b3b"), Style.WHITE));
			LINES.Add("B606", new Style(Style.ParseColor("#9cbf11"), Style.WHITE));
			LINES.Add("B607", new Style(Style.ParseColor("#60ad2a"), Style.WHITE));
			LINES.Add("B608", new Style(Style.ParseColor("#f8a600"), Style.WHITE));
			LINES.Add("B609", new Style(Style.ParseColor("#ef7100"), Style.WHITE));
			LINES.Add("B610", new Style(Style.ParseColor("#3ec1f1"), Style.WHITE));
			LINES.Add("B611", new Style(Style.ParseColor("#0099db"), Style.WHITE));
			LINES.Add("B612", new Style(Style.ParseColor("#ce9d53"), Style.WHITE));
			LINES.Add("B613", new Style(Style.ParseColor("#7b3600"), Style.WHITE));
			LINES.Add("B614", new Style(Style.ParseColor("#806839"), Style.WHITE));
			LINES.Add("B615", new Style(Style.ParseColor("#532700"), Style.WHITE));
			LINES.Add("B630", new Style(Style.ParseColor("#c41950"), Style.WHITE));
			LINES.Add("B631", new Style(Style.ParseColor("#9b1c44"), Style.WHITE));
			LINES.Add("B633", new Style(Style.ParseColor("#88cdc7"), Style.WHITE));
			LINES.Add("B635", new Style(Style.ParseColor("#cec800"), Style.WHITE));
			LINES.Add("B636", new Style(Style.ParseColor("#af0223"), Style.WHITE));
			LINES.Add("B637", new Style(Style.ParseColor("#e3572a"), Style.WHITE));
			LINES.Add("B638", new Style(Style.ParseColor("#af5836"), Style.WHITE));
			LINES.Add("B640", new Style(Style.ParseColor("#004f81"), Style.WHITE));
			LINES.Add("BT650", new Style(Style.ParseColor("#54baa2"), Style.WHITE));
			LINES.Add("BT651", new Style(Style.ParseColor("#005738"), Style.WHITE));
			LINES.Add("BT680", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B800", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B812", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B843", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B845", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B852", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B855", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B856", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
			LINES.Add("B857", new Style(Style.ParseColor("#4e6578"), Style.WHITE));
		}

		public VrrProvider(IWebClient webClient)
			: base(API_BASE, webClient)
		{
			SetNeedsSpEncId(true);
		}

		public VrrProvider(string apiBase, string departureMonitorEndpoint, string tripEndpoint, string stopFinderEndpoint,
		                   string coordEndpoint, IWebClient webClient)
			: base(apiBase, departureMonitorEndpoint, tripEndpoint, stopFinderEndpoint, coordEndpoint, webClient)
		{
		}


		public override NetworkId Id()
		{
			return NetworkId.VRR;
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
			Style style = LINES[line];
			if (style != null)
				return style;

			return base.LineStyle(line);
		}
	}
}