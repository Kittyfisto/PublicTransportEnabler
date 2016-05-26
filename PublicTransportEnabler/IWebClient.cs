using System.Threading.Tasks;

namespace PublicTransportEnabler
{
	public interface IWebClient
	{
		void ResetState();
		Task<string> Scrape(string url);
		Task<string> Scrape(string url, string postRequest, string encoding, string sessionCookieName);
		Task<string> Scrape(string urlStr, string postRequest, string encoding, string sessionCookieName, int tries);

		Task<string> Scrape(string urlStr, string postRequest, string encoding, string sessionCookieName, string httpReferrer,
		              int tries);

		string ResolveEntities(string str);
		string UrlEncode(string str);
		string UrlEncode(string str, string encoding);
		string UrlDecode(string str, string encoding);
	}
}