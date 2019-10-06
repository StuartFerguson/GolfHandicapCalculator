using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.IntegrationTests.Common
{
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Database;
    using DataTransferObjects;
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using Newtonsoft.Json;
    using Service.Client;
    using Service.Client.v2;
    using TechTalk.SpecFlow;
    using IGolfClubClient = Service.Client.v2.IGolfClubClient;
    using TokenType = Gherkin.TokenType;

    public class DockerHelper
    {
        protected ScenarioContext ScenarioContext;
        protected IContainerService ManagementAPIContainer;
        protected IContainerService SubscriptionServiceContainer;
        protected IContainerService EventStoreContainer;
        protected IContainerService SecurityServiceContainer;
        protected IContainerService MessagingServiceContainer;

        protected INetworkService TestNetwork;

        protected Int32 ManagementApiPort;
        protected Int32 EventStorePort;
        protected Int32 SecurityServicePort;
        protected Int32 MessagingServicePort;
        protected Guid SubscriberServiceId;

        private String ManagementAPIContainerName;
        private String EventStoreContainerName;
        private String SecurityServiceContainerName;
        private String SubscriptionServiceContainerName;
        private String MessagingServiceContainerName;

        private String EventStoreConnectionString;
        private String SecurityServiceAddress;
        private String AuthorityAddress;
        private String SubscriptionServiceConnectionString;
        private String ManagementAPIReadModelConnectionString;
        private String ManagementAPISeedingType;
        private String MessagingServiceAddress;

        public IPlayerClient PlayerClient;

        public IGolfClubAdministratorClient GolfClubAdministratorClient;

        public IGolfClubClient GolfClubClient;

        public ITournamentClient TournamentClient;

        public IReportingClient ReportingClient;

        public HttpClient HttpClient;

        private void SetupSecurityServiceContainer(String testFolder)
        {
            // Security Service Container
            this.SecurityServiceContainer = new Ductus.FluentDocker.Builders.Builder()
                .UseContainer()
                .WithName(this.SecurityServiceContainerName)
                .WithEnvironment("SeedingType=IntegrationTest", "ASPNETCORE_ENVIRONMENT=IntegrationTest",
                    $"ServiceOptions:PublicOrigin=http://{this.SecurityServiceContainerName}:5001",
                    $"ServiceOptions:IssuerUrl=http://{this.SecurityServiceContainerName}:5001",
                    this.MessagingServiceAddress)
                .WithCredential("https://www.docker.com", "stuartferguson", "Sc0tland")
                .UseImage("stuartferguson/securityserviceservice")
                .ExposePort(5001)
                .UseNetwork(this.TestNetwork)
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                .Build()
                .Start()
                .WaitForPort("5001/tcp", 30000);
        }

        private void SetupManagementAPIContainer(String testFolder)
        {
            // Management API Container
            this.ManagementAPIContainer = new Builder()
                .UseContainer()
                .WithName(this.ManagementAPIContainerName)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT=IntegrationTest",
                    this.EventStoreConnectionString,
                    this.ManagementAPIReadModelConnectionString,
                    this.SecurityServiceAddress,
                    this.ManagementAPISeedingType,
                    this.AuthorityAddress,
                    "EventStoreSettings:START_PROJECTIONS=true",
                    "EventStoreSettings:ContinuousProjectionsFolder=/app/projections/continuous")
                .UseImage("managementapiservice")
                .ExposePort(5000)
                .UseNetwork(new List<INetworkService> { this.TestNetwork, Setup.DatabaseServerNetwork }.ToArray())
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                .Build()
                .Start().WaitForPort("5000/tcp", 30000);
        }

        private void SetupMessagingService(String testFolder)
        {
            this.MessagingServiceContainer = new Builder()
                                             .UseContainer()
                                             .WithName(this.MessagingServiceContainerName)
                                             .WithEnvironment("ASPNETCORE_ENVIRONMENT=IntegrationTest")
                                             .WithCredential("https://www.docker.com", "stuartferguson", "Sc0tland")
                                             .UseImage("stuartferguson/messagingservice")
                                             .ExposePort(5002)
                                             .UseNetwork(this.TestNetwork)
                                             .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                                             .Build()
                                             .Start()
                                             .WaitForPort("5002/tcp", 30000);
        }

        private void SetupEventStoreContainer(String testFolder)
        {
            // Event Store Container
            this.EventStoreContainer = new Builder()
                .UseContainer()
                .UseImage("eventstore/eventstore:release-5.0.0")
                .ExposePort(2113)
                .ExposePort(1113)
                .WithName(this.EventStoreContainerName)
                .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS=all", "EVENTSTORE_START_STANDARD_PROJECTIONS=true")
                .UseNetwork(this.TestNetwork)
                .Mount($"D:\\temp\\docker\\{testFolder}\\eventstore", "/var/log/eventstore", MountType.ReadWrite)
                .Build()
                .Start().WaitForPort("2113/tcp", 30000);
        }

        private void SetupSubscriptionServiceContainer(String testFolder)
        {
            // Subscriber Service Container
            this.SubscriptionServiceContainer = new Builder()
                .UseContainer()
                .WithName(this.SubscriptionServiceContainerName)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT=Development",
                    this.SubscriptionServiceConnectionString,
                    this.EventStoreConnectionString,
                    "EventStoreSettings:HttpPort=2113",
                    $"ServiceSettings:EventStoreServerId={this.SubscriberServiceId}")
                .WithCredential("https://docker.io", "stuartferguson", "Sc0tland")
                .UseImage("stuartferguson/subscriptionservice")
                .UseNetwork(new List<INetworkService> { this.TestNetwork, Setup.DatabaseServerNetwork }.ToArray())
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                .Build()
                .Start();
        }

        private void SetupTestNetwork()
        {
            // Build a network
            this.TestNetwork = new Builder().UseNetwork($"testnetwork{Guid.NewGuid()}").Build();
        }

        protected void SetupSubscriptionServiceConfig()
        {
            IPEndPoint sqlEndpoint = Setup.DatabaseServerContainer.ToHostExposedEndpoint("1433/tcp");

            String server = "127.0.0.1";
            String database = "SubscriptionServiceConfiguration";
            String user = "sa";
            String password = "thisisalongpassword123!";
            String port = sqlEndpoint.Port.ToString();

            String connectionString = $"server={server},{port};user id={user}; password={password}; database={database};";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            // Insert the subscription service
            SubscriptionServiceHelper.CreateEventStoreServer(connection, this.SubscriberServiceId, "Test Service", "Test Service");

            // Create the Subscription Configurations
            SubscriptionServiceHelper.CreateSubscriptionConfiguration(connection,
                                                                      this.SubscriberServiceId,
                                                                      Guid.NewGuid(),
                                                                      "Golf Club Events",
                                                                      "$ce-GolfClubAggregate",
                                                                      $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/GolfClub");

            SubscriptionServiceHelper.CreateSubscriptionConfiguration(connection,
                                                                      this.SubscriberServiceId,
                                                                      Guid.NewGuid(),
                                                                      "Golf Club Membership Events",
                                                                      "$ce-GolfClubMembershipAggregate",
                                                                      $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/GolfClubMembership");

            SubscriptionServiceHelper.CreateSubscriptionConfiguration(connection,
                                                                      this.SubscriberServiceId,
                                                                      Guid.NewGuid(),
                                                                      "Golf Club Membership Events - Reporting",
                                                                      "$ce-GolfClubMembershipAggregate",
                                                                      $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/Reporting");

            SubscriptionServiceHelper.CreateSubscriptionConfiguration(connection,
                                                                      this.SubscriberServiceId,
                                                                      Guid.NewGuid(),
                                                                      "Handicap Calculator Process Events",
                                                                      "$ce-HandicapCalculationProcessAggregate",
                                                                      $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/HandicapCalculator");

            SubscriptionServiceHelper.CreateSubscriptionConfiguration(connection,
                                                                      this.SubscriberServiceId,
                                                                      Guid.NewGuid(),
                                                                      "Player Events",
                                                                      "$ce-PlayerAggregate",
                                                                      $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/Reporting");

            SubscriptionServiceHelper.CreateSubscriptionConfiguration(connection,
                                                                      this.SubscriberServiceId,
                                                                      Guid.NewGuid(),
                                                                      "Tournament Events",
                                                                      "$ce-TournamentAggregate",
                                                                      $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/Tournament");

            SubscriptionServiceHelper.CreateSubscriptionConfiguration(connection,
                                                                      this.SubscriberServiceId,
                                                                      Guid.NewGuid(),
                                                                      "Tournament Events - Reporting",
                                                                      "$ce-TournamentAggregate",
                                                                      $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/Reporting");

            connection.Close();
        }

        public Guid TestId;

        public async Task StartContainersForScenarioRun(String scenarioName)
        {
            String testFolder = scenarioName;

            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;
            // Setup the container names
            this.ManagementAPIContainerName = $"rest{testGuid:N}";
            this.EventStoreContainerName = $"eventstore{testGuid:N}";
            this.SecurityServiceContainerName = $"auth{testGuid:N}";
            this.SubscriberServiceId = testGuid;
            this.SubscriptionServiceContainerName = $"subService{testGuid:N}";
            this.MessagingServiceContainerName = $"messaging{testGuid:N}";

            this.EventStoreConnectionString =
                $"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{this.EventStoreContainerName}:1113;VerboseLogging=true;";
            this.SecurityServiceAddress = $"AppSettings:SecurityService=http://{this.SecurityServiceContainerName}:5001";
            this.AuthorityAddress = $"SecurityConfiguration:Authority=http://{this.SecurityServiceContainerName}:5001";
            this.SubscriptionServiceConnectionString =
                $"\"ConnectionStrings:SubscriptionServiceConfigurationContext={Setup.GetConnectionString("SubscriptionServiceConfiguration")}\"";
            this.ManagementAPIReadModelConnectionString = $"\"ConnectionStrings:ManagementAPIReadModel={Setup.GetConnectionString($"ManagementAPIReadModel{testGuid:N}")}\"";
            this.ManagementAPISeedingType = "SeedingType=IntegrationTest";
            this.MessagingServiceAddress = $"ServiceAddresses:MessagingService=http://{this.MessagingServiceContainerName}:5002";

            this.SetupTestNetwork();
            this.SetupSecurityServiceContainer(testFolder);
            this.SetupEventStoreContainer(testFolder);
            this.SetupSubscriptionServiceContainer(testFolder);
            this.SetupMessagingService(testFolder);
            this.SetupManagementAPIContainer(testFolder);

            // Cache the ports
            this.ManagementApiPort = this.ManagementAPIContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.EventStorePort = this.EventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
            this.SecurityServicePort = this.SecurityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
            this.MessagingServicePort = this.MessagingServiceContainer.ToHostExposedEndpoint("5002/tcp").Port;

            this.SetupSubscriptionServiceConfig();
            await this.SetupSecurityServiceConfig().ConfigureAwait(false);

            // Setup the base address resolver
            Func<String, String> baseAddressResolver = api => $"http://127.0.0.1:{this.ManagementApiPort}";

            HttpClient httpClient = new HttpClient();
            HttpClient httpClientv2 = new HttpClient();

            this.GolfClubAdministratorClient = new GolfClubAdministratorClient(baseAddressResolver, httpClientv2);
            this.GolfClubClient = new Service.Client.v2.GolfClubClient(baseAddressResolver, httpClientv2);
            this.PlayerClient = new PlayerClient(baseAddressResolver, httpClient);
            this.ReportingClient = new ReportingClient(baseAddressResolver, httpClient);
            this.TournamentClient = new TournamentClient(baseAddressResolver, httpClient);
            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(baseAddressResolver(String.Empty));
        }

        private async Task SetupSecurityServiceConfig()
        {
            String baseUri = $"http://127.0.0.1:{this.SecurityServicePort}";
            
            await SecurityServiceHelper.CreateSecurityRole(baseUri, RoleNames.Developer).ConfigureAwait(false);
            await SecurityServiceHelper.CreateSecurityRole(baseUri, RoleNames.TestDataGenerator).ConfigureAwait(false);
            await SecurityServiceHelper.CreateSecurityRole(baseUri, RoleNames.ClubAdministrator).ConfigureAwait(false);
            await SecurityServiceHelper.CreateSecurityRole(baseUri, RoleNames.MatchSecretary).ConfigureAwait(false);
            await SecurityServiceHelper.CreateSecurityRole(baseUri, RoleNames.Player).ConfigureAwait(false);
        }

        public async Task StopContainersForScenarioRun()
        {
            try
            {
                IPEndPoint sqlEndpoint = Setup.DatabaseServerContainer.ToHostExposedEndpoint("1433/tcp");

                String server = "127.0.0.1";
                String database = $"ManagementAPIReadModel{this.TestId:N}";
                String user = "sa";
                String password = "thisisalongpassword123!";
                String port = sqlEndpoint.Port.ToString();

                String connectionString = $"server={server},{port};user id={user}; password={password}; database={database};";

                ManagementAPI.Database.ManagementAPIReadModel context = new ManagementAPIReadModel(connectionString);
                context.Database.EnsureDeleted();

                if (this.ManagementAPIContainer != null)
                {
                    this.ManagementAPIContainer.StopOnDispose = true;
                    this.ManagementAPIContainer.RemoveOnDispose = true;
                    this.ManagementAPIContainer.Dispose();
                }

                if (this.MessagingServiceContainer != null)
                {
                    this.MessagingServiceContainer.StopOnDispose = true;
                    this.MessagingServiceContainer.RemoveOnDispose = true;
                    this.MessagingServiceContainer.Dispose();
                }

                if (this.EventStoreContainer != null)
                {
                    this.EventStoreContainer.StopOnDispose = true;
                    this.EventStoreContainer.RemoveOnDispose = true;
                    this.EventStoreContainer.Dispose();
                }

                if (this.SecurityServiceContainer != null)
                {
                    this.SecurityServiceContainer.StopOnDispose = true;
                    this.SecurityServiceContainer.RemoveOnDispose = true;
                    this.SecurityServiceContainer.Dispose();
                }

                if (this.SubscriptionServiceContainer != null)
                {
                    this.SubscriptionServiceContainer.StopOnDispose = true;
                    this.SubscriptionServiceContainer.RemoveOnDispose = true;
                    this.SubscriptionServiceContainer.Dispose();
                }

                if (this.TestNetwork != null)
                {
                    this.TestNetwork.Stop();
                    this.TestNetwork.Remove(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<String> GetToken(DataTransferObjects.TokenType tokenType, String clientId, String clientSecret, String userName=null, String password = null)
        {
            StringBuilder queryString = new StringBuilder();
            if (tokenType == DataTransferObjects.TokenType.Client)
            {
                queryString.Append("grant_type=client_credentials");
                queryString.Append($"&client_id={clientId}");
                queryString.Append($"&client_secret={clientSecret}");
            }
            else if (tokenType == DataTransferObjects.TokenType.Password)
            {
                queryString.Append("grant_type=password");
                queryString.Append($"&client_id={clientId}");
                queryString.Append($"&client_secret={clientSecret}");
                queryString.Append($"&username={userName}");

                queryString.Append($"&password={password}");
            }

            String requestUri = $"http://127.0.0.1:{this.SecurityServicePort}/connect/token";

            HttpResponseMessage httpResponse = await MakeHttpPost(requestUri, queryString.ToString(), mediaType: "application/x-www-form-urlencoded").ConfigureAwait(false);

            TokenResponse token = await GetResponseObject<TokenResponse>(httpResponse).ConfigureAwait(false);

            return token.AccessToken;
        }

        private async Task<HttpResponseMessage> MakeHttpPost<T>(String requestUri, T requestObject, String bearerToken = "", String mediaType = "application/json")
        {
            HttpResponseMessage result = null;
            StringContent httpContent = null;
            if (requestObject is String)
            {
                httpContent = new StringContent(requestObject.ToString(), Encoding.UTF8, mediaType);
            }
            else
            {
                String requestSerialised = JsonConvert.SerializeObject(requestObject);
                httpContent = new StringContent(requestSerialised, Encoding.UTF8, mediaType);
            }

            using (HttpClient client = new HttpClient())
            {
                if (!String.IsNullOrEmpty(bearerToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                result = await client.PostAsync(requestUri, httpContent, CancellationToken.None).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<T> GetResponseObject<T>(HttpResponseMessage responseMessage)
        {
            T result = default(T);

            result = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));

            return result;
        }
    }

    public class RoleNames
    {
        #region Others

        /// <summary>
        /// The club administrator
        /// </summary>
        public const String ClubAdministrator = "Club Administrator";

        /// <summary>
        /// The developer
        /// </summary>
        public const String Developer = "Developer";

        /// <summary>
        /// The match secretary
        /// </summary>
        public const String MatchSecretary = "Match Secretary";

        /// <summary>
        /// The player
        /// </summary>
        public const String Player = "Player";

        /// <summary>
        /// The test data generator
        /// </summary>
        public const String TestDataGenerator = "Test Data Generator";

        #endregion
    }
}
