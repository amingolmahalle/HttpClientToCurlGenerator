using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientToCurl.Sample.InGeneral.Controllers;

[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{
    private readonly HttpClient _httpClient;
    public MyController(HttpClient httpClient) => _httpClient = httpClient;

    [HttpGet]
    public async Task Send()
    {
        string apiUrl = "https://jsonplaceholder.typicode.com/posts";

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

            // Log the curl command for debugging or testing.
            // This generates a curl command that can be imported into Postman.
            // Use it to check and compare against all the requirements.

            // Send the request
            HttpResponseMessage response = await _httpClient.SendAsync(request);

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
