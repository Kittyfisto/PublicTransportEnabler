using System;
using System.Collections.Generic;
using System.Text;
using PublicTransportEnabler.Contract;
using PublicTransportEnabler.Enum;

namespace PublicTransportEnabler.Model
{
	public class QueryConnectionsResult
	{
		public QueryConnectionsResult(ResultHeader header, String queryUri, Location from, Location via, Location to,
		                              IQueryConnectionsContext context, List<Connection> connections,
		                              List<Location> ambiguousFrom,
		                              List<Location> ambiguousVia,
		                              List<Location> ambiguousTo, Status status)
		{
			Header = header;
			Status = status;
			QueryUri = queryUri;
			From = from;
			Via = via;
			To = to;
			Context = context;
			Connections = connections;

			AmbiguousFrom = ambiguousFrom;
			AmbiguousVia = ambiguousVia;
			AmbiguousTo = ambiguousTo;
		}

		public QueryConnectionsResult(ResultHeader header, String queryUri, Location from, Location via, Location to,
		                              IQueryConnectionsContext context, List<Connection> connections)
			: this(header, queryUri, from, via, to, context, connections, null, null, null, Status.OK)
		{
		}

		public QueryConnectionsResult(ResultHeader header, List<Location> ambiguousFrom, List<Location> ambiguousVia,
		                              List<Location> ambiguousTo)
			: this(
				header, null, null, null, null, null, null, ambiguousFrom, ambiguousVia, ambiguousTo, Status.AMBIGUOUS)
		{
		}

		public QueryConnectionsResult(ResultHeader header, Status status)
			: this(header, null, null, null, null, null, null, null, null, null, status)
		{
		}

		public ResultHeader Header { get; private set; }
		public Status Status { get; private set; }

		public List<Location> AmbiguousFrom { get; private set; }
		public List<Location> AmbiguousVia { get; private set; }
		public List<Location> AmbiguousTo { get; private set; }

		public string QueryUri { get; private set; }
		public Location From { get; private set; }
		public Location Via { get; private set; }
		public Location To { get; private set; }
		public IQueryConnectionsContext Context { get; private set; }
		public List<Connection> Connections { get; private set; }


		public override string ToString()
		{
			var builder = new StringBuilder(GetType().Name);
			builder.Append("[").Append(Status).Append(": ");

			if (Connections != null)
				builder.Append(Connections.Count).Append(" connections " + Connections + ", ");
			if (AmbiguousFrom != null)
				builder.Append(AmbiguousFrom.Count).Append(" ambiguous from, ");
			if (AmbiguousVia != null)
				builder.Append(AmbiguousVia.Count).Append(" ambiguous via, ");
			if (AmbiguousTo != null)
				builder.Append(AmbiguousTo.Count).Append(" ambiguous to, ");

			builder.Append("]");
			return builder.ToString();
		}
	}
}