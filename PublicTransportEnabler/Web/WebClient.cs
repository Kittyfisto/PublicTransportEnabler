using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PublicTransportEnabler.Web
{
	public class WebClient : IWebClient
	{
		private readonly Regex _pEntity = new Regex("&(?:#(x[\\da-f]+|\\d+)|(amp|quot|apos|szlig|nbsp));");
		private readonly HttpClient _client;
		public string Platform = "[\\wÄÖÜäöüßáàâéèêíìîóòôúùû\\. -/&#;]+?";

		private const string ScrapeAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
		private const string ScrapeDefaultEncoding = "ISO-8859-1";
		private const string ScrapeUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0";

		public WebClient()
		{
			_client = new HttpClient();

			_client.DefaultRequestHeaders.Add("User-Agent", ScrapeUserAgent);
			_client.DefaultRequestHeaders.Add("Accept", ScrapeAccept);
			//_client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
			_client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
		}

		public void ResetState()
		{}

		public Task<string> Scrape(string url)
		{
			return Scrape(url, null, null, null);
		}

		public Task<string> Scrape(string url, string postRequest, string encoding, string sessionCookieName)
		{
			return Scrape(url, postRequest, encoding, sessionCookieName, 3);
		}

		public Task<string> Scrape(string urlStr, string postRequest, string encoding, string sessionCookieName, int tries)
		{
			return Scrape(urlStr, postRequest, encoding, sessionCookieName, null, 3);
		}

		public Task<string> Scrape(string urlStr, string postRequest, string encoding, string sessionCookieName, string httpReferrer,
		                     int tries)
		{
			if (encoding == null)
				encoding = ScrapeDefaultEncoding;

			Encoding transportEncoding = Encoding.GetEncoding(encoding);

			/*if (!string.IsNullOrWhiteSpace(httpReferrer))
				_client.DefaultRequestHeaders.Add(HttpRequestHeader.Referer, httpReferrer);

			if (sessionCookieName != null && stateCookie != null)
				_client.Headers.Add(HttpRequestHeader.Cookie, stateCookie);*/


			Task<HttpResponseMessage> task;

			if (postRequest != null)
			{
				byte[] postRequestBytes = transportEncoding.GetBytes(postRequest);
				var content = new ByteArrayContent(postRequestBytes);

				content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				content.Headers.Add("Content-Length", postRequestBytes.Length.ToString(CultureInfo.InvariantCulture));

				task = _client.PostAsync(urlStr, content);
			}
			else
			{
				task = _client.GetAsync(urlStr);
			}

			return task.ContinueWith(t =>
				{
					HttpResponseMessage result = t.Result;
					var msg = result.Content.ReadAsStringAsync().Result;
					return msg;
				});
		}

		public string ResolveEntities(string str)
		{
			if (str == null)
				return null;

			Match matcher = _pEntity.Match(str);
			var builder = new StringBuilder(str.Length);
			//int pos = 0;
			//while (matcher.Success)
			//{
			//     char c;
			//     string code = matcher.Groups[1].Value;
			//    if (code != null)
			//    {
			//        if (code[0] == 'x')
			//            c = (char) Integer.valueOf(code.substring(1), 16).intValue();
			//        else
			//            c = (char) Integer.parseInt(code);
			//    }
			//    else
			//    {
			//         string namedEntity = matcher.group(2);
			//        if (namedEntity.equals("amp"))
			//            c = '&';
			//        else if (namedEntity.equals("quot"))
			//            c = '"';
			//        else if (namedEntity.equals("apos"))
			//            c = '\'';
			//        else if (namedEntity.equals("szlig"))
			//            c = '\u00df';
			//        else if (namedEntity.equals("nbsp"))
			//            c = ' ';
			//        else
			//            throw new IllegalStateException("unknown entity: " + namedEntity);
			//    }
			//    builder.append(str.subSequence(pos, matcher.start()));
			//    builder.append(c);
			//    pos = matcher.end();
			//}
			//builder.append(str.subSequence(pos, str.length()));
			return builder.ToString();
		}

		public string UrlEncode(string str)
		{
			var value = WebUtility.UrlEncode(str);
			return value;
		}

		public string UrlEncode(string str, string encoding)
		{
			var value = WebUtility.UrlEncode(str);
			return value;
			//return System.Web.HttpUtility.UrlEncode(str, Encoding.GetEncoding(encoding));
		}


		public string UrlDecode(string str, string encoding)
		{
			//return System.Web.HttpUtility.UrlDecode(str, Encoding.GetEncoding(encoding));
			return null;
		}
	}
}