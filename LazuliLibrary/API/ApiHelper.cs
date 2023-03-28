using System.Net.Http.Headers;

namespace LazuliLibrary.API;

public class ApiHelper
{
    private HttpClient? _apiClient;

    public HttpClient? ApiClient
    {
        get { return _apiClient; }
    }

    public ApiHelper()
    {
        InitializeClient();
    }

    private void InitializeClient()
    {
        // TODO get api from appsettings.json
        string api = "https://jsonplaceholder.typicode.com/";

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
