using System.Net.Http.Json;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Helpers
{
    //In Program.cs of the consuming app:
    //services.AddCommonHttpClient();
    //
    // Example:
    //
    // private readonly HttpServiceHelper _http;

    // public MyService(HttpServiceHelper http)
    // {
    //     _http = http;
    // }

    // public async Task TestCalls()
    // {
    //     // GET example
    //     var getResult = await _http.GetAsync<List<UserDto>>("users");

    //     // POST example
    //     var postResult = await _http.PostAsync<CreateUserRequest, UserDto>("users", new CreateUserRequest { Name = "Alice" });
    // }
    //
    //
    public class HttpServiceHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpServiceHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResult<T>> GetAsync<T>(string endpoint) where T : class
        {
            var result = new TResult<T>();
            try
            {
                var client = _httpClientFactory.CreateClient("CommonHttpClient");
                var response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    result.Data = await response.Content.ReadFromJsonAsync<T>();
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = $"HTTP {response.StatusCode}: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<TResult<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
            where TResponse : class
            where TRequest : class
        {
            var result = new TResult<TResponse>();
            try
            {
                var client = _httpClientFactory.CreateClient("CommonHttpClient");
                var response = await client.PostAsJsonAsync(endpoint, payload);

                if (response.IsSuccessStatusCode)
                {
                    result.Data = await response.Content.ReadFromJsonAsync<TResponse>();
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = $"HTTP {response.StatusCode}: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
