using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoreDeo {
    public class HttpHelper : IDisposable {
        private readonly HttpClient _httpClient;

        public HttpHelper(string baseUrl) {
            _httpClient = new HttpClient { BaseAddress = new Uri (baseUrl) };
        }

        public Task<string> GetAsync(string token, string requestUri) {
            return SendAsync (token, HttpMethod.Get, requestUri, new StringContent (string.Empty));
        }

        public Task<HttpResponseMessage> PostAsync(string request, HttpContent content) {
            return _httpClient.PostAsync (request, content);
        }

        public Task<string> PostAsync(string token, string requestUri, string key, string value) {
            //var content = new KeyValuePair<string, string> (key, value);
            return PostCollectionAsync (token, requestUri, new Dictionary<string, string> { { key, value } });
        }

        public Task<string> PostCollectionAsync(string token, string requestUri, IDictionary<string, string> content) {
            var httpContent = new FormUrlEncodedContent (content); // smallest enumerable?
            return SendAsync (token, HttpMethod.Post, requestUri, httpContent);
        }

        public async Task<string> SendAsync(string token, HttpMethod method, string requestUri, HttpContent content) {
            using (var httpRequest = new HttpRequestMessage (method, requestUri) {
                Headers = { { "auth", token } },
                Content = content
            }) {
                var httpResponse = await _httpClient.SendAsync (httpRequest).ConfigureAwait (false);
                httpResponse.EnsureSuccessStatusCode ();
                var response = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);
                return response;
            }
        }

        public void Dispose() {
            _httpClient.Dispose ();
        }
    }
}