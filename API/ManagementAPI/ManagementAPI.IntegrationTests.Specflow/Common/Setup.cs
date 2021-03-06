﻿using System;
using System.Data;
using System.IO;
using System.Threading;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using MySql.Data.MySqlClient;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Specflow.Common
{
    [Binding]
    public class Setup
    {
        public static IContainerService DatabaseServerContainer;
        //public static String DbConnectionString;
        private static String DbConnectionStringWithNoDatabase;
        public static INetworkService DatabaseServerNetwork;

        [BeforeTestRun]
        protected static void GlobalSetup()
        {
            // Setup a network for the DB Server
            DatabaseServerNetwork = new Builder().UseNetwork($"testnetwork{Guid.NewGuid()}").Build();

            // Start the Database Server here
            DbConnectionStringWithNoDatabase = StartMySqlContainerWithOpenConnection();            
        }

        public static String GetConnectionString(String databaseName)
        {
            return $"{DbConnectionStringWithNoDatabase} database={databaseName};";
        }

        [AfterTestRun]
        protected static void GlobalTearDown()
        {
            // Kill the DB Server 
            DatabaseServerContainer?.Stop();

            // Kill the Network 
            DatabaseServerNetwork?.Stop();
        }

        private static String StartMySqlContainerWithOpenConnection()
        {
            String containerName = $"testdatabase{Guid.NewGuid():N}";
            DatabaseServerContainer = new Ductus.FluentDocker.Builders.Builder()
                .UseContainer()
                .WithName(containerName)
                .UseImage("golfhandicapping/subscriptionservicemysqldb")
                .WithEnvironment("MYSQL_ROOT_PASSWORD=Pa55word", "MYSQL_ROOT_HOST=%")
                .ExposePort(3306)
                .UseNetwork(DatabaseServerNetwork)
                .Build()
                .Start()
                .WaitForPort("3306/tcp", 30000);

            var mysqlEndpoint = DatabaseServerContainer.ToHostExposedEndpoint("3306/tcp");

            // Try opening a connection
            Int32 maxRetries = 10;
            Int32 counter = 1;

            String server = "127.0.0.1";
            String database = "SubscriptionServiceConfiguration";
            String user = "root";
            String password = "Pa55word";
            String port = mysqlEndpoint.Port.ToString();
            String sslM = "none";

            String connectionString = $"server={server};port={port};user id={user}; password={password}; database={database}; SslMode={sslM}";

            MySqlConnection connection = new MySqlConnection(connectionString);

            using (StreamWriter sw = new StreamWriter("C:\\Temp\\testlog.log", true))
            {
                while (counter <= maxRetries)
                {
                    try
                    {
                        sw.WriteLine($"Attempt {counter}");
                        sw.WriteLine(DateTime.Now);

                        connection.Open();

                        MySqlCommand command = connection.CreateCommand();
                        command.CommandText = "SELECT * FROM EndPoints";
                        command.ExecuteNonQuery();

                        sw.WriteLine("Connection Opened");

                        connection.Close();

                        break;
                    }
                    catch (MySqlException ex)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        sw.WriteLine(ex);
                        Thread.Sleep(20000);
                    }
                    finally
                    {
                        counter++;
                    }
                }
            }

            return $"server={containerName};user id={user}; password={password};";
        }
    }
}