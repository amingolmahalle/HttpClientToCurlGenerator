using System.Text;

namespace HttpClientToCurl.Sample.InFile;

public static class ApiCaller
{
    public static async Task MakeApiCall()
    {
        string apiUrl = "https://jsonplaceholder.typicode.com/posts";

        // Create an instance of HttpClient
        HttpClient client = new();

        try
        {
            // Create a sample JSON payload
            string jsonPayload =
                "{\"title\":\"New Post\",\"body\":\"This is the body of the new post\",\"userId\":1}";

            // Create HttpRequestMessage with the POST verb
            HttpRequestMessage request = new(HttpMethod.Post, apiUrl);

            // Set up the request headers
            request.Headers.Add("Authorization", "Bearer YourAccessToken"); // Add any necessary headers

            // Set the request content with the JSON payload
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Generate a curl command and write it to a file for debugging or testing.
            // This command can be imported into Postman for checking and comparing against all the requirements.
            // config is optional
            client.GenerateCurlInFile(request, config =>
            {
                // Customize file configuration if needed
                config.TurnOn = true; // Enable generating curl command to file
                config.Filename = "curl_commands.txt"; // Specify the file name
                config.Path = "C:\\Path\\To\\Directory"; // Specify the directory path
                config.NeedAddDefaultHeaders = true; // Specify if default headers should be included
            });

            // Send the request
            HttpResponseMessage response = await client.SendAsync(request);

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
}
