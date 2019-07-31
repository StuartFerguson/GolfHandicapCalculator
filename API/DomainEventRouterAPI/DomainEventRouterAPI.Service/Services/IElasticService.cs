using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.Services
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using System.Threading;
    using Elasticsearch.Net;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Nest;
    using Shared.General;

    public interface IElasticService
    {
        #region Methods

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IndexName> GetIndex<T>(String index,
                                    CancellationToken cancellationToken);

        /// <summary>
        /// Posts the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="documentId">The document identifier.</param>
        void PostModel<T>(T model, 
                          IndexName indexName,
                          String documentId) where T : class;

        /// <summary>
        /// Upserts the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="documentId">The document identifier.</param>
        void UpsertModel<T>(T model,
                            IndexName indexName,
                            String documentId) where T : class;

        #endregion
    }

    public class ElasticService : IElasticService
    {
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

        public ElasticService()
        {
            Uri uri = new Uri(ConfigurationReader.GetValue("ElasticServer"));
            SingleNodeConnectionPool pool = new SingleNodeConnectionPool(uri);

            String elasticUserName = ConfigurationReader.GetValue("ElasticUserName");
            String elasticPassword = ConfigurationReader.GetValue("ElasticPassword");

            ConnectionSettings settings = new ConnectionSettings(pool)
                                          .DisableDirectStreaming().PrettyJson().OnRequestCompleted(callDetails =>
                                          {
                                              if (callDetails.RequestBodyInBytes != null)
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

                                              if (callDetails.ResponseBodyInBytes != null)
                                              {
                                                  Console.WriteLine($"Status: {callDetails.HttpStatusCode}\n" +
                                                                    $"{Encoding.UTF8.GetString(callDetails.ResponseBodyInBytes)}\n" +
                                                                    $"{new String('-', 30)}\n");
                                              }
                                              else
                                              {
                                                  Console.WriteLine($"Status: {callDetails.HttpStatusCode}\n" +
                                                                    $"{new String('-', 30)}\n");
                                              }
                                          });
            ;

            if (String.IsNullOrEmpty(elasticUserName) == false && String.IsNullOrEmpty(elasticPassword) == false)
            {
                // We are talking to a secured elastic instance
                settings = settings.BasicAuthentication(elasticUserName, elasticPassword);
            }

            this.ElasticClient = new ElasticClient(settings);
        }

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
                catch (Exception e)
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

        private async Task<IndexName> CreateIndex<T>(String indexName,
                                                     CancellationToken cancellationToken)
        {
            IndexExistsRequest request = new IndexExistsRequest(Indices.Parse(indexName));
            IExistsResponse indexExistsResponse = await this.ElasticClient.IndexExistsAsync(request, cancellationToken);

            if (indexExistsResponse.Exists == false)
            {
                await this.ElasticClient.CreateIndexAsync(indexName, null, cancellationToken);
            }

            GetIndexRequest getIndexRequest = new GetIndexRequest(Indices.Parse(indexName));

            IGetIndexResponse getIndexResponse = await this.ElasticClient.GetIndexAsync(getIndexRequest, cancellationToken);

            return getIndexResponse.Indices.Single().Key;
        }

        public void PostModel<T>(T model,
                                 IndexName indexName,
                                 String documentId) where T : class
        {
            CreateRequest<T> createRequest = new CreateRequest<T>(model, indexName, null, new Id(documentId));

            ICreateResponse createResponse = this.ElasticClient.Create(createRequest);

            if (!createResponse.ApiCall.Success && (HttpStatusCode)createResponse.ApiCall.HttpStatusCode != HttpStatusCode.Conflict)
            {
                throw new Exception($"Error occured when creating document, {createResponse.OriginalException}");
            }
        }

        public void UpsertModel<T>(T model,
                                   IndexName indexName,
                                   String documentId) where T : class
        {
            IUpdateResponse<T> response = this.ElasticClient.Update<T>(documentId, u => u.Index(indexName).Doc(model).DocAsUpsert());

            if (!response.IsValid)
            {
                throw new
                    Exception($"Error occured when creating/updating document id [{documentId}] of type [{typeof(T).Name}] in index [{indexName.Name}], {response.OriginalException}");
            }
        }
    }
}
