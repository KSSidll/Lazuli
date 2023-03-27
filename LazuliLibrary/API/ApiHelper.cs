using LazuliLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LazuliLibrary.API
{
    public class ApiHelper
    {
		private HttpClient? _apiClient;
        private readonly LoggedInUserModel _loggedInUser;

        public HttpClient? ApiClient
		{
			get 
			{ 
				return _apiClient; 
			}
		}

		public ApiHelper(LoggedInUserModel loggedInUser)
		{
            InitializeClient();
            _loggedInUser = loggedInUser;
        }

        private void InitializeClient()
        {
            string? api = ConfigurationManager.AppSettings["api"];

            if (String.IsNullOrWhiteSpace(api))
            {
                throw new Exception("Could not find api path in appsettings");
            }

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void LoggOffUser()
        {
            _apiClient?.DefaultRequestHeaders.Clear();
        }

    }
}
