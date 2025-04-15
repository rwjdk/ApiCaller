using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using ApiCaller.AuthorizationTypes;
using ApiCaller.Exceptions;

namespace ApiCaller;

public class ApiClient(HttpClient httpClient, ApiClientOptions? options = null)
{
    public async Task<T> GetAsync<T>(RequestUrl requestUrl, CancellationToken cancellationToken = default)
    {
        return await GetAsync<T>(requestUrl.ToString(), cancellationToken);
    }

    public async Task<T> GetAsync<T>(string requestUrl, CancellationToken cancellationToken = default)
    {
        var json = await GetAsync(requestUrl, cancellationToken);
        try
        {
            return JsonSerializer.Deserialize<T>(json)!;
        }
        catch (Exception e)
        {
            throw new ApiClientDeserializationException(e.ToString());
        }
    }

    public async Task<string> GetAsync(RequestUrl requestUrl, CancellationToken cancellationToken = default)
    {
        return await GetAsync(requestUrl.ToString(), cancellationToken);
    }

    public async Task<string> GetAsync(string requestUrl, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await GetHttpClient().GetAsync(BuildUrl(requestUrl), cancellationToken);
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return responseString;
        }

        throw new ApiClientException(responseString, response.StatusCode);
    }

    public async Task<T> PostAsync<T>(RequestUrl requestUrl, string body, CancellationToken cancellationToken = default)
    {
        return await PostAsync<T>(requestUrl.ToString(), body, cancellationToken);
    }

    public async Task<T> PostAsync<T>(string requestUrl, string body, CancellationToken cancellationToken = default)
    {
        var json = await PostAsync(requestUrl, body, cancellationToken);
        try
        {
            return JsonSerializer.Deserialize<T>(json)!;
        }
        catch (Exception e)
        {
            throw new ApiClientDeserializationException(e.ToString());
        }
    }

    public async Task<string> PostAsync(RequestUrl requestUrl, string body, CancellationToken cancellationToken = default)
    {
        return await PostAsync(requestUrl.ToString(), body, cancellationToken);
    }

    public async Task<string> PostAsync(string requestUrl, string body, CancellationToken cancellationToken = default)
    {
        Uri requestUri = BuildUrl(requestUrl);
        HttpResponseMessage response = await GetHttpClient().PostAsync(requestUri, new StringContent(body, Encoding.UTF8, "application/json"), cancellationToken);
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return responseString;
        }

        throw new ApiClientException(responseString, response.StatusCode);
    }

    public async Task<T> PutAsync<T>(RequestUrl requestUrl, string body, CancellationToken cancellationToken = default)
    {
        return await PutAsync<T>(requestUrl.ToString(), body, cancellationToken);
    }

    public async Task<T> PutAsync<T>(string requestUrl, string body, CancellationToken cancellationToken = default)
    {
        var json = await PutAsync(requestUrl, body, cancellationToken);
        try
        {
            return JsonSerializer.Deserialize<T>(json)!;
        }
        catch (Exception e)
        {
            throw new ApiClientDeserializationException(e.ToString());
        }
    }

    public async Task<string> PutAsync(RequestUrl requestUrl, string body, CancellationToken cancellationToken = default)
    {
        return await PutAsync(requestUrl.ToString(), body, cancellationToken);
    }

    public async Task<string> PutAsync(string requestUrl, string body, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await GetHttpClient().PutAsync(BuildUrl(requestUrl), new StringContent(body, Encoding.UTF8, "application/json"), cancellationToken);
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return responseString;
        }

        throw new ApiClientException(responseString, response.StatusCode);
    }

    private HttpClient GetHttpClient()
    {
        if (options?.Authorization is ApiClientBearerTokenAuthorization bearerTokenAuthorization)
        {
            string token = bearerTokenAuthorization.Token;
            if (!token.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                token = "Bearer " + token;
            }

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
        }

        return httpClient;
    }

    private Uri BuildUrl(string requestUrl)
    {
        string url = options?.BaseUrl ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(url) && !url.EndsWith('/'))
        {
            url += "/";
        }

        if (requestUrl.StartsWith('/'))
        {
            requestUrl = requestUrl[1..];
        }

        url += requestUrl;

        var builder = new UriBuilder(new Uri(url));
        if (options?.Authorization is ApiClientQueryAuthorization queryAuthorization)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(builder.Query);
            foreach (var param in queryAuthorization.QueryParams)
            {
                queryString.Add(param.Key, param.Value);
            }

            builder.Query = queryString.ToString();
        }

        return builder.Uri;
    }

    public async Task DeleteAsync(RequestUrl requestUrl, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(requestUrl.ToString(), cancellationToken);
    }

    public async Task DeleteAsync(string requestUrl, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await GetHttpClient().DeleteAsync(BuildUrl(requestUrl), cancellationToken);
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        throw new ApiClientException(responseString, response.StatusCode);
    }
}