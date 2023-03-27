using LazuliLibrary.Models;
using System.Net.Http.Headers;

namespace LazuliLibrary.API;

public class ApiHelper
{
    private HttpClient? _apiClient;
    private readonly LoggedInUserModel _loggedInUser;

    public HttpClient? ApiClient
    {
        get { return _apiClient; }
    }

    public ApiHelper(LoggedInUserModel loggedInUser)
    {
        InitializeClient();
        _loggedInUser = loggedInUser;
    }

    private void InitializeClient()
    {
        // TODO get api from appsettings.json
        string api = "https://jsonplaceholder.typicode.com/users";

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
}
