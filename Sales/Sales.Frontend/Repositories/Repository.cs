using System.Text;
using System.Text.Json;

namespace Sales.Frontend.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jsonSerializerOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };


        public Repository(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<T>(responseHttp);
                return new HttpResponseWrapper<T>(response, false, responseHttp);

            }
            return new HttpResponseWrapper<T>(default, true, responseHttp);

        }

       

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson,Encoding.UTF8, "application/json");
            var responseHtpp = await _httpClient.PostAsync(url, messageContent);
            
            return new HttpResponseWrapper<object>(default, !responseHtpp.IsSuccessStatusCode, responseHtpp);

        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHtpp = await _httpClient.PostAsync(url, messageContent);

            if (responseHtpp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<TResponse>(responseHtpp);
                return new HttpResponseWrapper<TResponse>(response, false, responseHtpp);

            }

            return new HttpResponseWrapper<TResponse>(default, true, responseHtpp);
        }

        private async Task<T> UnserializeAnswerAsync<T>(HttpResponseMessage responseHtpp)
        {
            var response = await responseHtpp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, _jsonSerializerOptions)!;

        }
    }
}
