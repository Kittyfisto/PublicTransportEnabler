namespace PublicTransportEnabler.Contract
{
	public interface IWebClient
	{
		void ResetState();
		string Scrape(string url);
		string Scrape(string url, string postRequest, string encoding, string sessionCookieName);
		string Scrape(string urlStr, string postRequest, string encoding, string sessionCookieName, int tries);

		string Scrape(string urlStr, string postRequest, string encoding, string sessionCookieName, string httpReferrer,
		              int tries);

		string ResolveEntities(string str);
		string UrlEncode(string str);
		string UrlEncode(string str, string encoding);
		string UrlDecode(string str, string encoding);
	}
}