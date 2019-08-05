namespace DomainEventRouterAPI.Service.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Elasticsearch.Net;
    using Nest;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DomainEventRouterAPI.Service.Services.IElasticService" />
    public class ElasticService : IElasticService
    {
        #region Fields

        /// <summary>
        /// The elastic client
        /// </summary>
        private readonly ElasticClient ElasticClient;

        /// <summary>
        /// The index lock
        /// </summary>
        private static readonly Object IndexLock = new Object();

        /// <summary>
        /// The index name dictionary
        /// </summary>
        private readonly ConcurrentDictionary<String, IndexName> IndexNameDictionary = new ConcurrentDictionary<String, IndexName>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticService"/> class.
        /// </summary>
        public ElasticService()
        {
            Uri uri = new Uri(ConfigurationReader.GetValue("ElasticServer"));
            SingleNodeConnectionPool pool = new SingleNodeConnectionPool(uri);

            String elasticUserName = ConfigurationReader.GetValue("ElasticUserName");
            String elasticPassword = ConfigurationReader.GetValue("ElasticPassword");

            ConnectionSettings settings = new ConnectionSettings(pool).DisableDirectStreaming().PrettyJson().OnRequestCompleted(callDetails =>
                                                                                                                                {
                                                                                                                                    if (callDetails.RequestBodyInBytes !=
                                                                                                                                        null)
                                                                                                                                    {
                                                                                                                                        Console
                                                                                                                                            .WriteLine($"{callDetails.HttpMethod} {callDetails.Uri} \n" +
                                                                                                                                                       $"{Encoding.UTF8.GetString(callDetails.RequestBodyInBytes)}");
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Console
                                                                                                                                            .WriteLine($"{callDetails.HttpMethod} {callDetails.Uri}");
                                                                                                                                    }

                                                                                                                                    Console.WriteLine();

                                                                                                                                    if (callDetails.ResponseBodyInBytes !=
                                                                                                                                        null)
                                                                                                                                    {
                                                                                                                                        Console
                                                                                                                                            .WriteLine($"Status: {callDetails.HttpStatusCode}\n" +
                                                                                                                                                       $"{Encoding.UTF8.GetString(callDetails.ResponseBodyInBytes)}\n" +
                                                                                                                                                       $"{new String('-', 30)}\n");
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        Console
                                                                                                                                            .WriteLine($"Status: {callDetails.HttpStatusCode}\n" +
                                                                                                                                                       $"{new String('-', 30)}\n");
                                                                                                                                    }
                                                                                                                                });
            ;

            if (string.IsNullOrEmpty(elasticUserName) == false && string.IsNullOrEmpty(elasticPassword) == false)
            {
                // We are talking to a secured elastic instance
                settings = settings.BasicAuthentication(elasticUserName, elasticPassword);
            }

            this.ElasticClient = new ElasticClient(settings);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<IndexName> GetIndex<T>(String index,
                                                 CancellationToken cancellationToken)
        {
            Boolean lockTaken = false;

            if (this.IndexNameDictionary.ContainsKey(index) == false)
            {
                try
                {
                    Monitor.TryEnter(ElasticService.IndexLock, ref lockTaken);

                    if (lockTaken)
                    {
                        //Check if we have cached this
                        if (this.IndexNameDictionary.ContainsKey(index) == false)
                        {
                            IndexName i = this.CreateIndex<T>(index, cancellationToken).Result;

                            this.IndexNameDictionary.TryAdd(index, i);
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.LogError(e);
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(ElasticService.IndexLock);
                    }
                }
            }

            this.IndexNameDictionary.TryGetValue(index, out IndexName indexName);

            return indexName;
        }

        /// <summary>
        /// Posts the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Exception">Error occured when creating document, {createResponse.OriginalException}</exception>
        public async Task PostModel<T>(T model,
                                       String indexName,
                                       String documentId,
                                       CancellationToken cancellationToken) where T : class
        {
            IndexName index = await this.GetIndex<T>(indexName, cancellationToken);

            CreateRequest<T> createRequest = new CreateRequest<T>(model, index, null, new Id(documentId));

            ICreateResponse createResponse = this.ElasticClient.Create(createRequest);

            if (!createResponse.ApiCall.Success && (HttpStatusCode)createResponse.ApiCall.HttpStatusCode != HttpStatusCode.Conflict)
            {
                throw new Exception($"Error occured when creating document, {createResponse.OriginalException}");
            }
        }

        /// <summary>
        /// Upserts the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Exception">Error occured when creating/updating document id [{documentId}] of type [{typeof(T).Name}] in index [{indexName}], {response.OriginalException}</exception>
        public async Task UpsertModel<T>(T model,
                                         String indexName,
                                         String documentId,
                                         CancellationToken cancellationToken) where T : class
        {
            IUpdateResponse<T> response = this.ElasticClient.Update<T>(documentId, u => u.Index(indexName).Doc(model).DocAsUpsert());

            if (!response.IsValid)
            {
                throw new
                    Exception($"Error occured when creating/updating document id [{documentId}] of type [{typeof(T).Name}] in index [{indexName}], {response.OriginalException}");
            }
        }

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task<IndexName> CreateIndex<T>(String indexName,
                                                     CancellationToken cancellationToken)
        {
            indexName = indexName.ToLower();
            IndexExistsRequest request = new IndexExistsRequest(Indices.Parse(indexName));
            IExistsResponse indexExistsResponse = await this.ElasticClient.IndexExistsAsync(request, cancellationToken);

            if (indexExistsResponse.Exists == false)
            {
                var createIndexResponse = await this.ElasticClient.CreateIndexAsync(indexName, null, cancellationToken);
            }

            GetIndexRequest getIndexRequest = new GetIndexRequest(Indices.Parse(indexName));

            IGetIndexResponse getIndexResponse = await this.ElasticClient.GetIndexAsync(getIndexRequest, cancellationToken);

            return getIndexResponse.Indices.Single().Key;
        }

        #endregion
    }
}