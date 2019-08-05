namespace DomainEventRouterAPI.Service.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Nest;

    /// <summary>
    /// 
    /// </summary>
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
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task PostModel<T>(T model,
                          String indexName,
                          String documentId,
                          CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Upserts the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpsertModel<T>(T model,
                            String indexName,
                            String documentId,
                            CancellationToken cancellationToken) where T : class;

        #endregion
    }
}