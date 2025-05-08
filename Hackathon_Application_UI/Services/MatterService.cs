using System;

using System.Collections.Generic;

using System.Net.Http;

using System.Text;

using System.Text.Json;

using System.Threading.Tasks;
using global::Hackathon_Application_UI.Interface;
using global::Hackathon_Application_UI.Models;
using Microsoft.Extensions.Configuration;


namespace Hackathon_Application_UI.Services

{

    public class MatterService : IMatterService

    {

        private readonly HttpClient _httpClient;

        private readonly string _apiBaseUrl;

        public MatterService(HttpClient httpClient, IConfiguration configuration)

        {

            _httpClient = httpClient;

            _apiBaseUrl = configuration["ApiSettings:GatewayUrl"];

        }

        public async Task<IEnumerable<Matter>> GetAllMattersAsync()

        {

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/matter");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<Matter>>(content, new JsonSerializerOptions
            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<Matter> GetMatterByIdAsync(int matterId)

        {

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/matter/{matterId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)

                return null;

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Matter>(content, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<Matter> CreateMatterAsync(Matter matter)

        {

            var content = new StringContent(

                JsonSerializer.Serialize(matter),

                Encoding.UTF8,

                "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/matter", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Matter>(responseContent, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<Matter> UpdateMatterAsync(Matter matter)

        {

            var content = new StringContent(

                JsonSerializer.Serialize(matter),

                Encoding.UTF8,

                "application/json");

            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/api/matter/{matter.MatterId}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)

                return null;

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Matter>(responseContent, new JsonSerializerOptions

            {

                PropertyNameCaseInsensitive = true

            });

        }

        public async Task<bool> DeleteMatterAsync(int matterId)

        {

            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/matter/{matterId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)

                return false;

            response.EnsureSuccessStatusCode();

            return true;

        }

    }

}


 