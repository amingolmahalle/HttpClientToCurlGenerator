# :1st_place_medal: HttpClientToCurlGenerator :1st_place_medal:
An extension for generating Curl script of HttpClient.

This extension will help you to see whatever is set in **HttpClient** in the form of a curl script.

And you can check if that is the correct data for sending to an external service or not. and also if you have an error, you can check the script and find your problem and fix that. so easily.

Also, it is new way and also a fast way to create or update a collection Postman, when you haven't got a **postman collection** for your desired external service.

It's easy to use. just you should install the package on your project from the below address and use sample codes for how to call and work with extensions.

[Nuget Package Address](https://www.nuget.org/packages/HttpClientToCurl/)

You have **2 ways** to see script results:

**1- Console**
(e.g. **httpClient.GenerateCurlInConsole(httpRequestMessage, requestUri, null);** )

:warning: **Notice**: when the curl script was written in the console, maybe your **IDE console** applies **WordWrap** automatically. you should **remove enters** from the script.

**2- File**
(e.g. **httpClient.GenerateCurlInFile(httpRequestMessage, requestUri, null);** )

(Parameter of **configs** for both of them are optional.)

I will be happy say me if you have any feedback and your solution to improve the code and also if you find a problem. 
also, I will be extremely happy if you contribute to the implementation and improvement of the project.

[Gmail Address](mailto:amin.golmahalle@gmail.com)

## **Give a Star!** :star:

If you like this project, learn something or you are using it in your applications, please give it a star. Thanks!

## **How to use HttpClientToCurlGenerator**:

### Sample code for **Post Method** (it will be written in the console):
```
        string requestBody = @"{ ""name"" : ""amin"",""requestId"" : ""10001000"",""amount"":10000 }";
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
                configs.EnableCodeBeautification = false;
            });

        // Call PostAsync =>  await client.PostAsync(requestUri, httpRequest.Content);
```

### Sample code for **Post Method** (it will be written in the file):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**
```
        string path = string.Empty;
        string filename = "PostMethodResult" ;
        string requestBody = @"{ ""name"" : ""sara"",""requestId"" : ""10001001"",""amount"":20000 }";
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

### Sample code for **Get Method** (it will be written in the console):
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
                configs.EnableCodeBeautification = false;
            });

        // Call GetAsync =>  await client.GetAsync(requestUri);
```

### Sample code for **Get Method** (it will be written in the file):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**
```
        string path = string.Empty;
        string filename = "GetMethodResult";
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

### Sample code for **Put Method** (it will be written in the console):
```
        string requestBody = @"{ ""name"" : ""jadi"",""requestId"" : ""10001003"",""amount"":30000 }";
        string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
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
                configs.EnableCodeBeautification = false;
            });

        // Call PutAsync =>  await client.PutAsync(requestUri, httpRequest.Content);
```

### Sample code for **Put Method** (it will be written in the file):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**
```
        string path = string.Empty;
        string filename = "PutMethodResult" ;
        string requestBody = @"{ ""name"" : ""reza"",""requestId"" : ""10001004"",""amount"":40000 }";
        string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
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

        // Call PutAsync =>  await client.PutAsync(requestUri, httpRequest.Content);
```

### Sample code for **Patch Method** (it will be written in the console):
```
        string requestBody = @"{ ""name"" : ""hamed"",""requestId"" : ""10001005"",""amount"":50000 }";
        string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
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
                configs.EnableCodeBeautification = false;
            });

        // Call PatchAsync =>  await client.PatchAsync(requestUri, httpRequest.Content);
```

### Sample code for **Patch Method** (it will be written in the file):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**
```
        string path = string.Empty;
        string filename = "PatchMethodResult" ;
        string requestBody = @"{ ""name"" : ""zara"",""requestId"" : ""10001006"",""amount"":60000 }";
        string requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
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

        // Call PatchAsync =>  await client.PatchAsync(requestUri, httpRequest.Content);
```

### Sample code for **Delete Method** (it will be written in the console):
```
        int id = 12;
        string requestUri = $"api/test/{id}";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
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
                configs.EnableCodeBeautification = false;
            });

        // Call DeleteAsync =>  await client.DeleteAsync(requestUri);
```

### Sample code for **Delete Method** (it will be written in the file):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**
```
        string path = string.Empty;
        string filename = "DeleteMethodResult";
        int id = 12;
        string requestUri = $"api/test/{id}";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
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

        // Call DeleteAsync =>  await client.DeleteAsync(requestUri);
```

I Hope Enjoying this extension in your projects.
