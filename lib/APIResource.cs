using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Marcin5.lib
{
    /// <summary>
    /// Inteface básica de um recurso de API
    /// </summary>
    public interface IApiResources : IDisposable
    {
        string BaseURI { get; set; }
        Task<T> GetAsync<T>();
        Task<T> GetAsync<T>(string id);
        Task<T> GetAsync<T>(string id, string apiToken);
        Task<T> GetAsync<T>(string id, string partOfUrl, string apiToken);

        Task<T> PostAsync<T>(object data);
        Task<T> PostAsync<T>(object data, string partOfUrl);
        Task<T> PostAsync<T>(object data, string partOfUrl, string apiUserToken);

        Task<T> PutAsync<T>(string id, object data);

        Task<T> DeleteAsync<T>(string id);
    }

    /// <summary>
    /// Implementação da inteface básica de acesso a recursos básicos da API da IUGU
    /// </summary>
    public class APIResource : IApiResources
    {
        private readonly IHttpClientWrapper _client;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly string _version;
        private readonly string _endpoint;
        private readonly string _apiVersion;
        private readonly string _apiKey;
        private string _baseURI;

        public string BaseURI
        {
            get { return _baseURI; }
            set { _baseURI = _endpoint + "/" + _apiVersion + value; }
        }

        /// <summary>
        /// Construtor customizado que permite total controle sobre as configurações do client
        /// </summary>
        private APIResource(IHttpClientWrapper customClient, JsonSerializerSettings customJsonSerializerSettings = null)
        {
            _client = customClient;
            _jsonSettings = customJsonSerializerSettings ??
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            _version = "1.0.5";
            _apiVersion = "v1";
            _endpoint = "https://api.iugu.com";
            _apiKey = ConfigurationManager.AppSettings["iuguApiKey"];

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new MissingFieldException(
                    "API Key not configured. Verify id the iuguApiKey key with your token is in the .config file");
            }

            _baseURI = _endpoint + "/" + _apiVersion;
        }

        /// <summary>
        /// Construtor default que usa as configurações padrão do httpClient e do JsonSerializer
        /// </summary>
        public APIResource() : this(new StandardHttpClient(),
            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore})
        {
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<T> GetAsync<T>()
        {
            var response = await SendRequestAsync(HttpMethod.Get, BaseURI).ConfigureAwait(false);
            return await ProcessResponse<T>(response).ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(string id)
        {
            var response = await GetAsync<T>(id, null, null).ConfigureAwait(false);
            return response;
        }

        public async Task<T> GetAsync<T>(string id, string apiToken)
        {
            var response = await GetAsync<T>(id, null, apiToken).ConfigureAwait(false);
            return response;
        }

        public async Task<T> GetAsync<T>(string id, string partOfUrl, string apiToken)
        {
            var completeUrl = GetCompleteUrl(partOfUrl, id);
            var response = await SendRequestAsync(HttpMethod.Get, completeUrl, null, apiToken).ConfigureAwait(false);
            return await ProcessResponse<T>(response).ConfigureAwait(false);
        }

        public async Task<T> PostAsync<T>(object data)
        {
            var response = await PostAsync<T>(data, null, null).ConfigureAwait(false);
            return response;
        }

        public async Task<T> PostAsync<T>(object data, string partOfUrl)
        {
            var response = await PostAsync<T>(data, partOfUrl, null).ConfigureAwait(false);
            return response;
        }

        public async Task<T> PostAsync<T>(object data, string partOfUrl, string customApiToken)
        {
            var completeUrl = GetCompleteUrl(partOfUrl, null);
            var response =
                await SendRequestAsync(HttpMethod.Post, completeUrl, data, customApiToken).ConfigureAwait(false);
            return await ProcessResponse<T>(response).ConfigureAwait(false);
        }

        public async Task<T> PutAsync<T>(string id, object data)
        {
            return await PutAsync<T>(data, id, null).ConfigureAwait(false);
        }

        public async Task<T> PutAsync<T>(object data, string partOfUrl)
        {
            return await PutAsync<T>(data, partOfUrl, null).ConfigureAwait(false);
        }

        public async Task<T> PutAsync<T>(object data, string partOfUrl, string customApiToken)
        {
            var completeUrl = GetCompleteUrl(partOfUrl, null);
            var response =
                await SendRequestAsync(HttpMethod.Put, completeUrl, data, customApiToken).ConfigureAwait(false);
            return await ProcessResponse<T>(response).ConfigureAwait(false);
        }

        public async Task<T> DeleteAsync<T>(string id)
        {
            return await DeleteAsync<T>(id, null).ConfigureAwait(false);
        }

        public async Task<T> DeleteAsync<T>(string id, string customApiToken)
        {
            var response =
                await SendRequestAsync(HttpMethod.Delete, $"{BaseURI}/{id}", null, customApiToken).ConfigureAwait(false);
            return await ProcessResponse<T>(response).ConfigureAwait(false);
        }

        private async Task<T> ProcessResponse<T>(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return
                    await Task.FromResult(JsonConvert.DeserializeObject<T>(data, _jsonSettings)).ConfigureAwait(false);
            }
            else
            {
                var responseError = await Task.FromResult(JsonConvert.SerializeObject(new
                {
                    StatusCode = response.StatusCode,
                    ReasonPhase = response.ReasonPhrase,
                    Message = data
                })).ConfigureAwait(false);

                //TODO: Should it really throw an exception? The Status Code describes what the problem is
                throw new Exception(responseError);
            }
        }

        private async Task<T> ProcessListResponse<T>(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return
                    await Task.FromResult(JsonConvert.DeserializeObject<T>(data, _jsonSettings)).ConfigureAwait(false);
            }
            else
            {
                var responseError = await Task.FromResult(JsonConvert.SerializeObject(new
                {
                    StatusCode = response.StatusCode,
                    ReasonPhase = response.ReasonPhrase,
                    Message = data
                })).ConfigureAwait(false);

                //TODO: Should it really throw an exception? The Status Code describes what the problem is
                throw new Exception(responseError);
            }
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, object data = null, string apiToken = null)
        {
            using (var requestMessage = new HttpRequestMessage(method, url))
            {
                SetAutorizationHeader(apiToken, requestMessage);

                await SetContent(data, requestMessage);

                var response = await _client.SendAsync(requestMessage).ConfigureAwait(false);
                return response;
            }
        }

        private async Task SetContent(object data, HttpRequestMessage requestMessage)
        {
            if (data != null)
            {
                var content = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(data, _jsonSettings)).ConfigureAwait(false);
                requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }
        }

        private void SetAutorizationHeader(string apiToken, HttpRequestMessage requestMessage)
        {
            var token = !string.IsNullOrEmpty(apiToken) ? apiToken : _apiKey;
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(token)));
        }

        private string GetCompleteUrl(string partOfUrl, string id)
        {
            var url = string.IsNullOrEmpty(partOfUrl) ? $"{BaseURI}/{id}" : $"{BaseURI}/{partOfUrl}/{id}";
            if (url.Last().Equals('/'))
            {
                url = url.Remove(url.Length - 1);
            }
            return url;
        }

        private string GetCompleteListUrl()
        {
            var url = BaseURI;
            if (url.Last().Equals('/'))
            {
                url = url.Remove(url.Length - 1);
            }
            return url;
        }
    }
}