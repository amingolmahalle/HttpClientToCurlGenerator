# HttpClientToCurlGenerator
An extension for generating Curl script of HttpClient

[Nuget Package Address](https://www.nuget.org/packages/HttpClientToCurl/1.0.0)

Sample code for **Post Method** (it will be written in the console):
```
 string requestBody = @"""{ ""name"" : ""amin"",""requestId"" : ""10001001"",""amount"":10000 }""";
        string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", Guid.NewGuid().ToString());

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        httpClient.GenerateCurlInConsole(
            httpRequestMessage,
            requestUri,
            configs =>
            {
                configs.TurnOn = true;
                configs.NeedAddDefaultHeaders = true;
            });

        // Call PostAsync =>  await client.PostAsync(requestUri, httpRequest.Content);
```

Sample code for **Get Method** (it will be written in the console):
```
 string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", Guid.NewGuid().ToString());

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        httpClient.GenerateCurlInConsole(
            httpRequestMessage,
            requestUri,
            configs =>
            {
                configs.TurnOn = true;
                configs.NeedAddDefaultHeaders = true;
            });

        // Call GetAsync =>  await client.GetAsync(requestUri);
```

Sample Code for **Post Method** (it will be written in the file):
```
 string path ="";
 string filename = "PostMethodResult" 
 string requestBody = @"""{ ""name"" : ""sara"",""requestId"" : ""10001002"",""amount"":90000 }""";
        string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", Guid.NewGuid().ToString());

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        httpClient.GenerateCurlInFile(
            httpRequestMessage,
            requestUri,
            configs =>
            {
                configs.Filename = filename;
                configs.Path = path;
                configs.TurnOn = true;
                configs.NeedAddDefaultHeaders = true;
            });

        // Call PostAsync =>  await client.PostAsync(requestUri, httpRequest.Content);
```

Sample Code for **Get Method** (it will be written in the file):
```
  string path ="";
  string filename = "GetMethodResult" 
  string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", Guid.NewGuid().ToString());

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        httpClient.GenerateCurlInFile(
            httpRequestMessage,
            requestUri,
            configs =>
            {
                configs.Filename = filename;
                configs.Path = path;
                configs.TurnOn = true;
                configs.NeedAddDefaultHeaders = true;
            });

        // Call GetAsync =>  await client.GetAsync(requestUri);
```
