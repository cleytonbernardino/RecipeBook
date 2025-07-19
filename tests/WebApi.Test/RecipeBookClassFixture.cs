using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test;

public class RecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public RecipeBookClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoPostFormData(string method, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        MultipartFormDataContent multipartContent = new();

        var requestProperties = request.GetType().GetProperties().ToList();
        foreach (var property in requestProperties)
        {
            var propertyValue = property.GetValue(request);

            if (string.IsNullOrWhiteSpace(propertyValue?.ToString()))
                continue;

            if (propertyValue is IList list)
                AddListToMultpartContent(multipartContent, property.Name, list);
            else
                multipartContent.Add(new StringContent(propertyValue.ToString()!), property.Name);
        }

        return await _httpClient.PostAsync(method, multipartContent);
    }

    protected async Task<HttpResponseMessage> DoGet(string method, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _httpClient.GetAsync(method);
    }

    protected async Task<HttpResponseMessage> DoPut(string method, object request, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _httpClient.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string method, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
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

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrEmpty(token))
            return;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void AddListToMultpartContent(MultipartFormDataContent multipartFormDataContent, string propertyName, IList list)
    {
        var itemType = list.GetType().GetGenericArguments().Single();

        if (itemType.IsClass && itemType != typeof(string))
        {
            int index = 0;
            foreach (var item in list)
            {
                var classPropertyInfo = item.GetType().GetProperties().ToList();

                foreach (var property in classPropertyInfo)
                {
                    var value = property.GetValue(item, null);
                    multipartFormDataContent.Add(new StringContent(value!.ToString()!), $"{propertyName}[{index}][{property.Name}]");
                }
                index++;
            }
        } 
        else
        {
            foreach (var item in list)
            {
                multipartFormDataContent.Add(new StringContent(item.ToString()!), propertyName);
            }
        }
    }
}
