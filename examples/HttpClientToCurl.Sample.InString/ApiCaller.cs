using System.Text;
using HttpClientToCurl.Extensions;

namespace HttpClientToCurl.Sample.InString;

public static class ApiCaller
{
    // Create an instance of HttpClient
    private static readonly HttpClient Client = new();
    private const string ApiUrl = "https://jsonplaceholder.typicode.com/posts";
    private const string AccessToken = "YourAccessToken";

    public static async Task MakeApiCall()
    {
        try
        {
            // Create a sample JSON payload
            var jsonPayload = "{\"title\":\"New Post\",\"body\":\"This is the body of the new post\",\"userId\":1}";

            // Generate a curl command as a string for debugging or testing.
            var response = await SendHttpRequest(HttpMethod.Post, ApiUrl, jsonPayload);

            // Check if the request was successful (status code 200-299)
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from the API:\n" + responseBody);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    private static async Task<HttpResponseMessage> SendHttpRequest(HttpMethod method, string url, string payload = null)
    {
        var request = new HttpRequestMessage(method, url);

        request.Headers.Add("Authorization", $"Bearer {AccessToken}");
        request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

        Console.WriteLine(GenerateCurl(request));

        return await Client.SendAsync(request);
    }

    private static string GenerateCurl(HttpRequestMessage request) => Client.GenerateCurlInString(request);
}
