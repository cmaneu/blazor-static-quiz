using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorQuiz.FrontEnd
{
    public class ClientAppSettings
    {
        private HttpClient _httpClient;
        private Dictionary<string, object> _allValues = new Dictionary<string, object>();


        public string ApiBaseUrl
        {
            get => _allValues[nameof(ApiBaseUrl)] as string;
            set => _allValues[nameof(ApiBaseUrl)] = value;
        }
        
        public string EnvironmentName
        {
            get => _allValues[nameof(EnvironmentName)] as string;
            set => _allValues[nameof(EnvironmentName)] = value;
        }

        public ClientAppSettings()
        {
            _httpClient = new HttpClient();
        }

        public ClientAppSettings(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoadAsync()
        {
            ClientAppSettings appSettings = null;
            try
            {
                appSettings = await _httpClient.GetFromJsonAsync<ClientAppSettings>("appsettings.json");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (appSettings == null)
            {
                appSettings = await _httpClient.GetFromJsonAsync<ClientAppSettings>("appsettings.local.json");
            }

            _allValues = appSettings._allValues;
        }
    }
}
