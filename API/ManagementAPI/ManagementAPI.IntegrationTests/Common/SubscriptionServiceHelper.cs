using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.IntegrationTests.Common
{
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Service.DataTransferObjects;

    public static class SubscriptionServiceHelper
    {
        public static async Task CreateEventStoreServer(SqlConnection connection, Guid eventStoreServerId, String connectionString, String name, CancellationToken cancellationToken)
        {
            SqlCommand eventStoreServerInsert = connection.CreateCommand();
            eventStoreServerInsert.CommandText = $"insert into EventStoreServers(EventStoreServerId, Name, ConnectionString) select '{eventStoreServerId}', '{name}', '{connectionString}'";
            await eventStoreServerInsert.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }

        public static async Task CreateSubscriptionConfiguration(SqlConnection connection, Guid eventStoreServerId, Guid subscriptionId, String groupName,
                                                           String streamName, String endpointUrl, CancellationToken cancellationToken)
        {
            SqlCommand subscriptionInsert = connection.CreateCommand();
            subscriptionInsert.CommandText =
                $"insert into SubscriptionConfigurations(SubscriptionId, EventStoreServerId, StreamName, GroupName, EndPointUri) select '{subscriptionId}', '{eventStoreServerId}', '{streamName}', '{groupName}', '{endpointUrl}'";
            await subscriptionInsert.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
