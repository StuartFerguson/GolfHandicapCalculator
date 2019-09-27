using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.IntegrationTests.Common
{
    using System.Data.SqlClient;
    using Service.DataTransferObjects;

    public static class SubscriptionServiceHelper
    {
        //public static void CreateSubscriptionStream(MySqlConnection connection, Guid subscriptionStreamId, String streamName)
        //{
        //    MySqlCommand streamInsert = connection.CreateCommand();
        //    streamInsert.CommandText = $"insert into SubscriptionStream(Id, StreamName, SubscriptionType) select '{subscriptionStreamId}', '{streamName}', 0";
        //    streamInsert.ExecuteNonQuery();
        //}

        //public static void CreateEndpoint(MySqlConnection connection, Guid endpointId, String endpointName, String endpointUrl)
        //{
        //    MySqlCommand endpointInsert = connection.CreateCommand();
        //    endpointInsert.CommandText = $"insert into EndPoints(EndpointId, name, url) select '{endpointId}', '{endpointName}', '{endpointUrl}'";
        //    endpointInsert.ExecuteNonQuery();
        //}

        //public static void CreateSubscriptionGroup(MySqlConnection connection, Guid subscriptionGroupId, Guid endpointId, String subscriptionGroup, Guid subscriptionStreamId)
        //{
        //    MySqlCommand groupInsert = connection.CreateCommand();
        //    groupInsert.CommandText =
        //        $"insert into SubscriptionGroups(Id, BufferSize, EndpointId, Name, StreamPosition, SubscriptionStreamId) select '{subscriptionGroupId}', 10, '{endpointId}', '{subscriptionGroup}', null, '{subscriptionStreamId}'";
        //    groupInsert.ExecuteNonQuery();
        //}

        //public static void CreateSubscriptionService(MySqlConnection connection, Guid subscriberServiceId, String subscriberServiceName)
        //{
        //    MySqlCommand subscriptionServiceInsert = connection.CreateCommand();
        //    subscriptionServiceInsert.CommandText =
        //        $"insert into SubscriptionServices(SubscriptionServiceId, Description) select '{subscriberServiceId}', '{subscriberServiceName}'";
        //    subscriptionServiceInsert.ExecuteNonQuery();
        //}

        //public static void AddSubscriptionGroupToSubscriberService(MySqlConnection connection, Guid subscriptionServiceGroupId, Guid subscriptionGroupId, Guid subscriberServiceId)
        //{
        //    MySqlCommand subscriptonServiceGroupInsert = connection.CreateCommand();
        //    subscriptonServiceGroupInsert.CommandText =
        //        $"insert into SubscriptionServiceGroups(SubscriptionServiceGroupId, SubscriptionGroupId, SubscriptionServiceId) select '{subscriptionServiceGroupId}', '{subscriptionGroupId}', '{subscriberServiceId}' ";
        //    subscriptonServiceGroupInsert.ExecuteNonQuery();
        //}

        public static void CreateEventStoreServer(SqlConnection connection, Guid eventStoreServerId, String connectionString, String name)
        {
            SqlCommand eventStoreServerInsert = connection.CreateCommand();
            eventStoreServerInsert.CommandText = $"insert into EventStoreServers(EventStoreServerId, Name, ConnectionString) select '{eventStoreServerId}', '{name}', '{connectionString}'";
            eventStoreServerInsert.ExecuteNonQuery();
        }

        public static void CreateSubscriptionConfiguration(SqlConnection connection, Guid eventStoreServerId, Guid subscriptionId, String groupName,
                                                           String streamName, String endpointUrl)
        {
            SqlCommand subscriptionInsert = connection.CreateCommand();
            subscriptionInsert.CommandText =
                $"insert into SubscriptionConfigurations(SubscriptionId, EventStoreServerId, StreamName, GroupName, EndPointUri) select '{subscriptionId}', '{eventStoreServerId}', '{streamName}', '{groupName}', '{endpointUrl}'";
            subscriptionInsert.ExecuteNonQuery();
        }
    }
}
