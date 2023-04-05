using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace LazuliLibrary.API;

public class ApiHelper : IApiHelper
{
	private readonly IConfiguration _config;

	public ApiHelper(IConfiguration config)
	{
		_config = config;
		InitializeClient();
	}

	public HttpClient? ApiClient { get; private set; }

	private void InitializeClient()
	{
		// gets api url from appsettings.json
		var api = _config.GetSection("Api")["url"];

		if (api is null) throw new ArgumentNullException(nameof(api));

		ApiClient = new HttpClient();
		ApiClient.BaseAddress = new Uri(api);
		ApiClient.DefaultRequestHeaders.Accept.Clear();
		ApiClient.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json")
		);
	}

	public void LoggOffUser()
	{
		ApiClient?.DefaultRequestHeaders.Clear();
	}

	public static void ApiHelperValidator(IApiHelper apiHelper)
	{
		if (apiHelper is null) throw new NullReferenceException("ApiHelper instance cannot be null");

		if (apiHelper.ApiClient is null) throw new NullReferenceException("ApiClient instance cannot be null");
	}
}