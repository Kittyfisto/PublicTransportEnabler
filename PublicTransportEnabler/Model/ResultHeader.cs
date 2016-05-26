using System;

namespace PublicTransportEnabler.Model
{
	public class ResultHeader
	{
		public ResultHeader(string serverProduct)
		{
			ServerProduct = serverProduct;
			ServerVersion = null;
			ServerTime = DateTime.Now;
			Context = null;
		}

		public ResultHeader(string serverProduct, string serverVersion, DateTime serverTime, Object context)
		{
			ServerProduct = serverProduct;
			ServerVersion = serverVersion;
			ServerTime = serverTime;
			Context = context;
		}

		public string ServerProduct { get; private set; }
		public string ServerVersion { get; private set; }
		public DateTime ServerTime { get; private set; }
		public Object Context { get; private set; }
	}
}