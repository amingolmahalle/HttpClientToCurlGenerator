using System.Text;

namespace HttpClientToCurl.Sample.InConsole;

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

            /* Generate a curl command and print it in the console for debugging or testing.
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
        Console.WriteLine("* Generate Curl By HttpClient:");

        // config is optional
        Client.GenerateCurlInConsole(httpRequestMessage, config =>
        {
            // Customize console configuration if needed
            config.TurnOn = true; // Enable generating curl command to the console
            config.NeedAddDefaultHeaders = true; // Specify if default headers should be included
            config.EnableCodeBeautification = true;
            config.EnableCompression = false;
        });

        Console.WriteLine();
    }

    private static void GenerateCurlByHttpRequestMessage(HttpRequestMessage httpRequestMessage)
    {
        Console.WriteLine("* Generate Curl By HttpRequestMessage:");

        // config is optional
        httpRequestMessage.GenerateCurlInConsole(new Uri(ApiUrl), config =>
        {
            // Customize console configuration if needed
            config.TurnOn = true; // Enable generating curl command to the console
            config.NeedAddDefaultHeaders = true; // Specify if default headers should be included
            config.EnableCodeBeautification = true;
            config.EnableCompression = false;
        });

        Console.WriteLine();
    }

    #endregion << Private Methods >>
}
