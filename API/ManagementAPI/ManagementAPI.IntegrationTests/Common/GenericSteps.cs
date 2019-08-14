using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using ManagementAPI.IntegrationTests.DataTransferObjects;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Common
{
    using System.Net;
    using Database;
    using GolfClub;
    using MySql.Data.MySqlClient;
    using Service.Client;

    [Binding]
    [Scope(Tag = "base")]
    public class GenericSteps
    {
        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        public GenericSteps(ScenarioContext scenarioContext,
                            TestingContext testingContext)
        {
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }

        [BeforeScenario()]
        public async Task StartSystem()
        {
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            this.TestingContext.DockerHelper = new DockerHelper();
            await this.TestingContext.DockerHelper.StartContainersForScenarioRun(scenarioName).ConfigureAwait(false);
        }

        [AfterScenario()]
        public async Task StopSystem()
        {
            await this.TestingContext.DockerHelper.StopContainersForScenarioRun().ConfigureAwait(false);
        }
    }
}
/*
protected async Task<HttpResponseMessage> MakeHttpGet(String requestUri, String bearerToken = "")
{
    HttpResponseMessage result = null;

    using (HttpClient client = new HttpClient())
    {
        if (!String.IsNullOrEmpty(bearerToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);   
        }

        result = await client.GetAsync(requestUri, CancellationToken.None).ConfigureAwait(false);
    }

    return result;
}

protected async Task<HttpResponseMessage> MakeHttpPost<T>(String requestUri, T requestObject, String bearerToken = "", String mediaType = "application/json")
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

protected async Task<HttpResponseMessage> MakeHttpPut<T>(String requestUri, T requestObject, String bearerToken = "")
{
    HttpResponseMessage result = null;

    using (HttpClient client = new HttpClient())
    {
        if (!String.IsNullOrEmpty(bearerToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);   
        }

        String requestSerialised = requestObject == null ? String.Empty : JsonConvert.SerializeObject(requestObject);
        StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");
        
        result = await client.PutAsync(requestUri, httpContent, CancellationToken.None).ConfigureAwait(false);   
    }

    return result;
}

protected async Task<T> GetResponseObject<T>(String contextKey)
{
    T result = default(T);

    HttpResponseMessage httpResponse = this.ScenarioContext.Get<HttpResponseMessage>(contextKey);

    result = await GetResponseObject<T>(httpResponse).ConfigureAwait(false);

    return result;
}

protected async Task<T> GetResponseObject<T>(HttpResponseMessage responseMessage)
{
    T result = default(T);

    result = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));

    return result;
}

protected async Task<String> GetToken(TokenType tokenType, String clientId, String clientSecret, String userName = "", String password = "")
{
    StringBuilder queryString = new StringBuilder();
    if (tokenType == TokenType.Client)
    {
        queryString.Append("grant_type=client_credentials");
        queryString.Append($"&client_id={clientId}");
        queryString.Append($"&client_secret={clientSecret}");                
    }
    else if (tokenType == TokenType.Password)
    {
        queryString.Append("grant_type=password");
        queryString.Append($"&client_id={clientId}");
        queryString.Append($"&client_secret={clientSecret}");
        queryString.Append($"&username={userName}");

        queryString.Append($"&password={password}");
    }

    String requestUri = $"http://127.0.0.1:{this.SecurityServicePort}/connect/token";

    HttpResponseMessage httpResponse = await MakeHttpPost(requestUri, queryString.ToString(), mediaType:"application/x-www-form-urlencoded").ConfigureAwait(false);

    TokenResponse token = await GetResponseObject<TokenResponse>(httpResponse).ConfigureAwait(false);

    return token.AccessToken;
}
}
}*/
