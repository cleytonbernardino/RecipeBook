using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test
{
    public class RecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public RecipeBookClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

        protected async Task<HttpResponseMessage> DoPost(string method, object request, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AddBearerToken(token);
            return await _httpClient.PostAsJsonAsync(method, request);
        }

        protected async Task<HttpResponseMessage> DoGet(string method, string token, string culture = "en")
        {
            ChangeRequestCulture(culture);
            AddBearerToken(token);
            return await _httpClient.GetAsync(method);
        }

        protected async Task<HttpResponseMessage> DoPut(string method, object request, string token, string culture = "en")
        {
            ChangeRequestCulture(culture);
            AddBearerToken(token);
            return await _httpClient.PutAsJsonAsync(method, request);
        }

        protected async Task<HttpResponseMessage> DoDelete(string method, string token, string culture = "en")
        {
            ChangeRequestCulture(culture);
            AddBearerToken(token);
            return await _httpClient.DeleteAsync(method);
        }

        protected static async Task<JsonElement> GetJsonElementAsync(HttpResponseMessage response)
        {
            await using Stream responseBody = await response.Content.ReadAsStreamAsync();
            JsonDocument responseData = await JsonDocument.ParseAsync(responseBody);
            return responseData.RootElement;
        }

        /// <summary>
        /// This method returns all errors in the response, it looks for the 'errors' key within the json.
        /// </summary>
        protected static async Task<JsonElement.ArrayEnumerator> GetErrorList(HttpResponseMessage response)
        {
            JsonElement jsonElement = await GetJsonElementAsync(response);
            return jsonElement.GetProperty("errors").EnumerateArray();
        }

        private void ChangeRequestCulture(string culture)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }

        private void AddBearerToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
