using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
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
		public void TestQueryNearbyStationsAsync1()
		{
			var provider = new MvvProvider(_client);
			var ostbahnhof = Location.FromWgs84(LocationType.STATION, 0, 48.127619, 11.604669);
			var response = provider.QueryNearbyStationsAsync(ostbahnhof, 1000, 10);

			response.Should().NotBeNull();
			var coordInfo = response.Result.CoordInfo;
			coordInfo.Should().NotBeNull();
			var items = coordInfo.CoordInfoItemList;
			items.Should().NotBeNull();
			items.Count.Should().Be(10);
			var item = items[0];
			item.Name.Should().StartWith("Ostbahnhof");
			item.Id.Should().Be(1000005);
		}

		[Test]
		public void TestQueryNearbyStationsAsync2()
		{
			var provider = new MvvProvider(_client);
			var ostbahnhof = Location.FromWgs84(LocationType.STATION, 0, 48.099198, 11.787178);
			var response = provider.QueryNearbyStationsAsync(ostbahnhof, 1000, 10);

			response.Should().NotBeNull();
			var coordInfo = response.Result.CoordInfo;
			coordInfo.Should().NotBeNull();
			var items = coordInfo.CoordInfoItemList;
			items.Should().NotBeNull();
			items.Count.Should().Be(10);
			var item = items[0];
			item.Name.Should().StartWith("Baldham");
			item.Id.Should().Be(1004010);
		}

		[Test]
		public void TestQueryConnectionsAsync1()
		{
			var provider = new MvvProvider(_client);
			var connections = provider.QueryConnectionsAsync(Location.FromId(LocationType.STATION, 1000005),
			                                            null,
			                                            Location.FromId(LocationType.STATION, 1004010),
			                                            DateTime.Now,
			                                            true,
			                                            10,
			                                            new List<Product>
				                                            {
					                                            Product.REGIONAL_TRAIN,
																Product.SUBURBAN_TRAIN,
																Product.SUBWAY,
																Product.TRAM
				                                            },
			                                            WalkSpeed.NORMAL,
			                                            Accessibility.NEUTRAL,
			                                            null);

			connections.Should().NotBeNull();
		}

		[Test]
		public void TestQueryDeparturesAsync()
		{
			var provider = new MvvProvider(_client);
			var departures = provider.QueryDeparturesAsync(1000005, 10, true);
			departures.Should().NotBeNull();
			var list = departures.Result.DepartureList;
			list.Should().NotBeNull();
			list.Length.Should().Be(10, "Because we requested 10 departures");
			
		}
	}
}