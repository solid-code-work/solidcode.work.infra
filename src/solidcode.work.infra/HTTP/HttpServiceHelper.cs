using System.Net.Http.Json;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra;

public class HttpServiceHelper
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpServiceHelper(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TResult<T>> GetAsync<T>(string endpoint) where T : class
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CommonHttpClient");
            var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                return TResultFactory.Ok(data!, "GET request successful.");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new TResult<T>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Message = $"HTTP {response.StatusCode}: {errorMessage}",
                };
            }
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
        where TResponse : class
        where TRequest : class
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CommonHttpClient");
            var response = await client.PostAsJsonAsync(endpoint, payload);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TResponse>();
                return TResultFactory.Ok(data!, "POST request successful.");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new TResult<TResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Message = $"HTTP {response.StatusCode}: {errorMessage}",
                };
            }
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<TResponse>(ex.Message);
        }
    }

    // Helper to map HTTP status codes to MessageErrorType
    private static MessageErrorType MapErrorType(System.Net.HttpStatusCode statusCode) =>
statusCode switch
{
    System.Net.HttpStatusCode.BadRequest => MessageErrorType.Validation,
    System.Net.HttpStatusCode.Unauthorized => MessageErrorType.Unauthorized,
    System.Net.HttpStatusCode.NotFound => MessageErrorType.NotFound,
    System.Net.HttpStatusCode.Conflict => MessageErrorType.Conflict,
    System.Net.HttpStatusCode.InternalServerError => MessageErrorType.Exception,
    System.Net.HttpStatusCode.NoContent => MessageErrorType.EmptyResult,
    _ => MessageErrorType.Exception
};

}
