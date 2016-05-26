using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransportEnabler.Contract;
using PublicTransportEnabler.Enum;
using PublicTransportEnabler.Model;
using PublicTransportEnabler.Provider;
using PublicTransportEnabler.Web;

namespace PublicTransportEnabler.Test.Provider
{
	[TestFixture]
	public sealed class MvvProviderTest
	{
		private IWebClient _client;

		[SetUp]
		public void Setup()
		{
			_client = new WebClient();
		}

		[Test]
		public void TestQueryNearbyStations()
		{
			var provider = new MvvProvider(_client);
			var ostbahnhof = Location.FromWgs84(LocationType.STATION, 0, 48.127619, 11.604669);
			var response = provider.QueryNearbyStations(ostbahnhof, 1000, 10);

			response.Should().NotBeNull();
			var coordInfo = response.CoordInfo;
			coordInfo.Should().NotBeNull();
			var items = coordInfo.CoordInfoItemList;
			items.Should().NotBeNull();
			items.Count.Should().Be(10);
			var item = items[0];
			item.Name.Should().StartWith("Ostbahnhof");
			item.Id.Should().Be(1000005);
		}

		[Test]
		public void TestQueryDepartures()
		{
			var provider = new MvvProvider(_client);
			var departures = provider.QueryDepartures(1000005, 10, true);
			departures.Should().NotBeNull();
			var list = departures.DepartureList;
			list.Should().NotBeNull();
			list.Length.Should().Be(10, "Because we requested 10 departures");
			
		}
	}
}