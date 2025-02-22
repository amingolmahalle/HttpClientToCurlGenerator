# :1st_place_medal: HttpClientToCurl :1st_place_medal:
An extension for generating the Curl script of HttpClient in .NET

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

**1- Put it in a string variable:**

```cs
string curlScript = httpClientInstance.GenerateCurlInString(httpRequestMessage);
```

**2- Show to the IDE console:**

```cs
httpClientInstance.GenerateCurlInConsole(httpRequestMessage);
```
- **Notice**: when the curl script was written in the console, maybe your **IDE console** applies **WordWrap** automatically. 
you should **remove enters** from the script.
- **Notice**: You can set specific configurations for your result in the optional **second parameter**.

**3- Write in a file:**

```cs
httpClientInstance.GenerateCurlInFile(httpRequestMessage);
```

- **Notice**: You can set specific configurations for your result in the optional **second parameter**.

:rocket: :rocket: :rocket:
___
*Additionally*, there is another overload option for adding everything as plain:
```cs
httpClientInstance.GenerateCurlInString(yourHttpMethod, "yourRequestUri", "yourHeader", "yourContent");
```
:fire:
___

**Read more about this extension:**

[English Article](https://www.c-sharpcorner.com/article/how-to-generate-curl-script-of-the-httpclient-in-net/)

[Persian Article](https://vrgl.ir/FE5s4)


Please let me know if you have any feedback and your solution to improve the code also if you find a problem. 
Also, I would be extremely happy if you could contribute to the implementation and improvement of the project.

[Gmail Address](mailto:amin.golmahalle@gmail.com)

## **Give a Star!** :star:

If you like this project, learn something, or are using it in your applications, please give it a star. Thanks!

## **How to use HttpClientToCurl Extensions**:

### **Post Method** sample code (it will be written in the **console**):
```cs
string requestBody = @"{""name"":""amin"",""requestId"":""10001000"",""amount"":10000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

 // Call PostAsync => await client.PostAsync(requestUri, httpRequestMessage.Content);
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
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");
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

// Call PostAsync => await client.PostAsync(requestUri, httpRequestMessage.Content);
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
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

// Call PostAsync => await client.PostAsync(requestUri, httpRequestMessage.Content);
```

### **Get Method** sample code (it will be written in the **console**):

```cs
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

### **Put Method** sample code (it will be written in the **console**):

```cs
string requestBody = @"{""name"":""jadi"",""requestId"":""10001003"",""amount"":30000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

// Call PutAsync => await client.PutAsync(requestUri, httpRequestMessage.Content);
```

### **Patch Method** sample code (it will be written in the **console**):

```cs
string requestBody = @"{""name"":""hamed"",""requestId"":""10001005"",""amount"":50000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

// Call PatchAsync => await client.PatchAsync(requestUri, httpRequestMessage.Content);
```

### **Delete Method** sample code (it will be written in the **console**):

```cs
int id = 12;
string requestUri = $"api/test/{id}";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

### **Post Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **yourProjectPath/bin/Debug/netx.0**

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "PostMethodResult" ;
string requestBody = @"{""name"":""sara"",""requestId"":""10001001"",""amount"":20000}";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

// Call PostAsync => await client.PostAsync(requestUri, httpRequestMessage.Content);
```

### **Get Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **yourProjectPath/bin/Debug/netx.0**

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "GetMethodResult";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

### **Put Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **yourProjectPath/bin/Debug/netx.0**

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "PutMethodResult" ;
string requestBody = @"{ ""name"" : ""reza"",""requestId"" : ""10001004"",""amount"":40000 }";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

// Call PutAsync => await client.PutAsync(requestUri, httpRequestMessage.Content);
```

### **Patch Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **yourProjectPath/bin/Debug/netx.0**

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "PatchMethodResult" ;
string requestBody = @"{ ""name"" : ""zara"",""requestId"" : ""10001006"",""amount"":60000 }";
string requestUri = "api/test";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

// Call PatchAsync => await client.PatchAsync(requestUri, httpRequestMessage.Content);
```

### **Delete Method** sample code (it will be written in the **file**):

If the path variable is null or empty, then the file is created in the **yourProjectPath/bin/Debug/netx.0**

If the filename variable is null or empty, then the current date will be set for it with this format: **yyyyMMdd**

```cs
string path = string.Empty;
string filename = "DeleteMethodResult";
int id = 12;
string requestUri = $"api/test/{id}";
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {Guid.NewGuid()}");

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

[More Samples](https://github.com/amingolmahalle/HttpClientToCurlGenerator/tree/master/tests/HttpClientToCurlGeneratorTest/FunctionalTest)

I hope you enjoy this extension in your projects.

### All Thanks to Our Contributors:
<a href="https://github.com/amingolmahalle/HttpClientToCurlGenerator/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=amingolmahalle/HttpClientToCurlGenerator" />
</a>
