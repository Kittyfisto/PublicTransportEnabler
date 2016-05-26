using FluentAssertions;
using NUnit.Framework;
using PublicTransportEnabler.Web;

namespace PublicTransportEnabler.Test
{
	[TestFixture]
	public sealed class WebClientTest
	{
		private WebClient _client;

		[SetUp]
		public void Setup()
		{
			_client = new WebClient();
		}

		[Test]
		public void TestUrlEncode1()
		{
			_client.UrlEncode("foobar").Should().Be("foobar");
			_client.UrlEncode("hello world").Should().Be("hello+world");
		}

		[Test]
		public void TestUrlEncode2()
		{
			_client.UrlEncode("foobar", "blub").Should().Be("foobar");
			_client.UrlEncode("hello world", "blub").Should().Be("hello+world");
		}
	}
}