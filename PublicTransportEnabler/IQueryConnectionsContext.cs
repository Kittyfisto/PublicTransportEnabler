namespace PublicTransportEnabler
{
	public interface IQueryConnectionsContext
	{
		bool CanQueryLater();
		bool CanQueryEarlier();
	}
}