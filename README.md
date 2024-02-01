# :1st_place_medal: HttpClientToCurlGenerator :1st_place_medal:
An extension for generating the Curl script of HttpClient.

[![license](https://img.shields.io/github/license/amingolmahalle/HttpClientToCurlGenerator)](https://github.com/amingolmahalle/HttpClientToCurlGeneratore/blob/master/LICENSE) [![forks](https://img.shields.io/github/forks/amingolmahalle/HttpClientToCurlGenerator)]() [![stars](https://img.shields.io/github/stars/amingolmahalle/HttpClientToCurlGenerator)](https://github.com/amingolmahalle/HttpClientToCurlGenerator)  
![example workflow](https://github.com/amingolmahalle/HttpClientToCurlGenerator/actions/workflows/dotnet.yml/badge.svg)

This extension will help you to see whatever is set in **HttpClient** in the form of a curl script.

And you can check if that is the correct data for sending to an external service or not. also if you have an error, you can check the script find your problem, and fix that. so easily.

Also, it is the new way and fast way to create or update a collection of Postman, when you haven't got a **postman collection** for your desired external service.

It's easy to use. just you should install the package on your project from the below address and use sample codes for how to call and work with extensions.


**For adding a package to your project from Nuget use this command**

```
dotnet add package HttpClientToCurl 
```

[Nuget Package Address](https://www.nuget.org/packages/HttpClientToCurl/)



You have **3 ways** to see script result:

**1: Put it in a string variable:**

```cs
string curlScript = httpClient.GenerateCurlInString(httpRequestMessage);
```

**2: Show to the IDE console:**

```cs
httpClient.GenerateCurlInConsole(httpRequestMessage);
```
- **Notice**: when the curl script was written in the console, maybe your **IDE console** applies **WordWrap** automatically. 
you should **remove enters** from the script.
- **Notice**: The 'config' Parameter is optional.

**3: Write in a file:**

```cs
httpClient.GenerateCurlInFile(httpRequestMessage);
```

- **Notice**: The 'config' Parameter is optional.

**Read more about this extension:**

[English Article]([https://medium.com/@amin.golmahalle/how-to-generate-curl-script-of-the-httpclient-in-net-c539da7c6588](https://www.c-sharpcorner.com/article/how-to-generate-curl-script-of-the-httpclient-in-net/))

[Persian Article](https://vrgl.ir/FPx11)


Please let me know if you have any feedback and your solution to improve the code and also if you find a problem. 
also, I will be extremely happy if you contribute to the implementation and improvement of the project.

[Gmail Address](mailto:amin.golmahalle@gmail.com)

## **Give a Star!** :star:

If you like this project, learn something, or are using it in your applications, please give it a star. Thanks!

## **How to use HttpClientToCurlGenerator Extensions**:

**You can see more samples in the FunctionalTest Directory.**

### **Post Method** sample code (it will be written in the **console**):
```cs
string requestBody = @"{""name"":""amin"",""requestId"":""10001000"",""amount"":10000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInConsole(
    httpRequestMessage,
    config =>
    {
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
        config.EnableCodeBeautification = false;
    });

 // Call PostAsync => await client.PostAsync(requestUri, httpRequest.Content);
```

### **Post Method** sample code for FormUrlEncodedContent (it will be written in the **console**):
```cs
string requestBody = @"{""name"":""justin"",""requestId"":10001026,""amount"":26000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
    httpRequestMessage.Content = new FormUrlEncodedContent(new[]
{
    new KeyValuePair<string, string>("session", "703438f3-16ad-4ba5-b923-8f72cd0f2db9"),
    new KeyValuePair<string, string>("payload", requestBody),
});
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInConsole(
    httpRequestMessage,
    config =>
    {
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
        config.EnableCodeBeautification = false;
    });

// Call PostAsync => await client.PostAsync(requestUri, httpRequest.Content);
```

### **Post Method** sample code for XML (it will be written in the **console**):

```cs
string requestBody = @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
    <Order>
    <Id>12</Id>
    <name>Jason</name>
    <requestId>10001024</requestId>
    <amount>240000</amount>
    </Order>";

var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/test");
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInConsole(
    httpRequestMessage,
    config: config =>
    {
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
        config.EnableCodeBeautification = false;
    });

// Call PostAsync => await client.PostAsync(requestUri, httpRequest.Content);
```

### **Post Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "PostMethodResult" ;
string requestBody = @"{""name"":""sara"",""requestId"":""10001001"",""amount"":20000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInFile(
    httpRequestMessage,
    config =>
    {
        config.Filename = filename;
        config.Path = path;
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
    });

// Call PostAsync => await client.PostAsync(requestUri, httpRequest.Content);
```

### **Get Method** sample code (it will be written in the **console**):

```cs
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInConsole(
    httpRequestMessage,
    config =>
    {
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
        config.EnableCodeBeautification = false;
    });

// Call GetAsync => await client.GetAsync(requestUri);
```

### **Get Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "GetMethodResult";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInFile(
    httpRequestMessage,
    config =>
    {
        config.Filename = filename;
        config.Path = path;
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
    });

// Call GetAsync => await client.GetAsync(requestUri);
```

### **Put Method** sample code (it will be written in the **console**):

```cs
string requestBody = @"{""name"":""jadi"",""requestId"":""10001003"",""amount"":30000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInConsole(
    httpRequestMessage,
    config =>
    {
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
        config.EnableCodeBeautification = false;
    });

// Call PutAsync => await client.PutAsync(requestUri, httpRequest.Content);
```

### **Put Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "PutMethodResult" ;
string requestBody = @"{ ""name"" : ""reza"",""requestId"" : ""10001004"",""amount"":40000 }";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInFile(
    httpRequestMessage,
    config =>
    {
        config.Filename = filename;
        config.Path = path;
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
    });

// Call PutAsync => await client.PutAsync(requestUri, httpRequest.Content);
```

### **Patch Method** sample code (it will be written in the **console**):

```cs
string requestBody = @"{""name"":""hamed"",""requestId"":""10001005"",""amount"":50000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInConsole(
    httpRequestMessage,
    config =>
    {
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
        config.EnableCodeBeautification = false;
    });

// Call PatchAsync => await client.PatchAsync(requestUri, httpRequest.Content);
```

### **Patch Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "PatchMethodResult" ;
string requestBody = @"{ ""name"" : ""zara"",""requestId"" : ""10001006"",""amount"":60000 }";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInFile(
    httpRequestMessage,
    config =>
    {
        config.Filename = filename;
        config.Path = path;
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
    });

// Call PatchAsync => await client.PatchAsync(requestUri, httpRequest.Content);
```

### **Delete Method** sample code (it will be written in the **console**):

```cs
int id = 12;
string requestUri = $"api/test/{id}";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInConsole(
    httpRequestMessage,
    config =>
    {
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
        config.EnableCodeBeautification = false;
    });

// Call DeleteAsync => await client.DeleteAsync(requestUri);
```

### **Delete Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **root project**.

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "DeleteMethodResult";
int id = 12;
string requestUri = $"api/test/{id}";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid().ToString()}");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

httpClient.GenerateCurlInFile(
    httpRequestMessage,
    config =>
    {
        config.Filename = filename;
        config.Path = path;
        config.TurnOn = true;
        config.NeedAddDefaultHeaders = true;
    });

// Call DeleteAsync => await client.DeleteAsync(requestUri);
```

**You can see more samples in the Functional Tests Directory.**

I hope you enjoy this extension in your projects.
