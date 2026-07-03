using System.Text;
using HttpClientToCurl.Extensions;

namespace HttpClientToCurl.Sample.InString;

public static class ApiCaller
{
    // Create an instance of HttpClient
    private static readonly HttpClient Client = new();
    private const string ApiUrl = "https://jsonplaceholder.typicode.com/posts";

    public static async Task MakeApiCall()
    {
        try
        {
            // Create a sample JSON payload
            string requestBody = /*lang=json,strict*/ @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

            // Create HttpRequestMessage with the POST verb
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, ApiUrl);

            // Set up the request headers
            httpRequestMessage.Headers.Add("Authorization", "Bearer YourAccessToken"); // Add any necessary headers

            // Set the request content with the JSON payload
            httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            /* Generate a curl command and set it to a string variable for debugging or testing.
               This command can be imported into Postman for checking and comparing against all the requirements. */

            // *** First Scenario ***
            GenerateCurlByHttpClient(httpRequestMessage);
            // *** Second Scenario ***
            GenerateCurlByHttpRequestMessage(httpRequestMessage);

            // Send the request
            HttpResponseMessage response = await Client.SendAsync(httpRequestMessage);

            // Check if the request was successful (status code 200-299)
            if (response.IsSuccessStatusCode)
            {
                // Read and print the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();
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

    #region << Private Methods >>

    private static void GenerateCurlByHttpClient(HttpRequestMessage httpRequestMessage)
    {
        // config is optional
        string curlResult = Client.GenerateCurlInString(httpRequestMessage, config =>
        {
            // Customize string variable configuration if needed
            config.TurnOn = true; // Enable generating curl command to string variable
            config.NeedAddDefaultHeaders = true; // Specify if default headers should be included
        });

        Console.WriteLine("* Generate Curl By HttpClient:");
        Console.WriteLine(curlResult);
        Console.WriteLine();
    }

    private static void GenerateCurlByHttpRequestMessage(HttpRequestMessage httpRequestMessage)
    {
        // config is optional
        string curlResult = httpRequestMessage.GenerateCurlInString(new Uri(ApiUrl), config =>
        {
            // Customize string variable configuration if needed
            config.TurnOn = true; // Enable generating curl command to string variable
            config.NeedAddDefaultHeaders = true; // Specify if default headers should be included
        });

        Console.WriteLine("* Generate Curl By HttpRequestMessage:");
        Console.WriteLine(curlResult);
        Console.WriteLine();
    }

    #endregion << Private Methods >>
}
