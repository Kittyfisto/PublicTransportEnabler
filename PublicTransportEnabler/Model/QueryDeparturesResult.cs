using System.Collections.Generic;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class QueryDeparturesResult
	{
		public QueryDeparturesResult(ResultHeader header) : this(header, Status.OK)
		{
		}

		public QueryDeparturesResult(ResultHeader header, Status status)
		{
			StationDepartures = new List<StationDepartures>();
			Header = header;
			Status = status;
		}

		public ResultHeader Header { get; private set; }
		public Status Status { get; private set; }
		public List<StationDepartures> StationDepartures { get; private set; }
	}
}