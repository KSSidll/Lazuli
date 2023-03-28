using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net.Http.Headers;

namespace LazuliLibrary.API;

public class ApiHelper : IApiHelper
{
    private HttpClient? _apiClient;
    private readonly IConfiguration _config;

    public HttpClient? ApiClient
    {
        get { return _apiClient; }
    }

    public ApiHelper(IConfiguration config)
    {
        _config = config;
        InitializeClient();     
    }

    private void InitializeClient()
    {
        // gets api url from appsettings.json
        string? api = _config?.GetSection("Api")["url"];

        if (api is null)
        {
            throw new ArgumentNullException(nameof(api));
        }

        _apiClient = new HttpClient();
        _apiClient.BaseAddress = new Uri(api);
        _apiClient.DefaultRequestHeaders.Accept.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );
    }

    public void LoggOffUser()
    {
        _apiClient?.DefaultRequestHeaders.Clear();
    }

    public static void ApiHelperValidator(ApiHelper apiHelper)
    {
        if (apiHelper is null)
        {
            throw new NullReferenceException("ApiHelper instance cannot be null");
        }

        if (apiHelper.ApiClient is null)
        {
            throw new NullReferenceException("ApiClient instance cannot be null");
        }
    }
}
