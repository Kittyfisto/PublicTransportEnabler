namespace PublicTransportEnabler.Contract
{
	public interface IQueryConnectionsContext
	{
		bool CanQueryLater();
		bool CanQueryEarlier();
	}
}