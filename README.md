# 🥇 HttpClientToCurl
An extension for generating the **CURL script** from **`HttpClient`** and **`HttpRequestMessage`** in .NET.

[![license](https://img.shields.io/github/license/amingolmahalle/HttpClientToCurlGenerator)](https://github.com/amingolmahalle/HttpClientToCurlGenerator/blob/master/LICENSE)
[![stars](https://img.shields.io/github/stars/amingolmahalle/HttpClientToCurlGenerator)](https://github.com/amingolmahalle/HttpClientToCurlGenerator/stargazers)
[![NuGet Version](https://img.shields.io/nuget/v/HttpClientToCurl.svg)](https://www.nuget.org/packages/HttpClientToCurl/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HttpClientToCurl.svg?style=flat-square)](https://www.nuget.org/packages/HttpClientToCurl/)
![Build](https://github.com/amingolmahalle/HttpClientToCurlGenerator/actions/workflows/dotnet.yml/badge.svg)

---

## 📖 Overview
**`HttpClientToCurl`** is a lightweight **.NET extension library** that helps you visualize any HTTP request as a **CURL command**.

You can use its extension methods on both:
- **`HttpClient`** — to generate CURL directly when sending requests  
- **`HttpRequestMessage`** — to inspect or log CURL representations before sending  

This is useful for:
- Debugging and verifying request payloads or headers  
- Sharing API calls between teammates  
- Generating or updating Postman collections easily  

---
## ⚙️ Installation

```bash
dotnet add package HttpClientToCurl
```
Or visit the NuGet page here: <a href="https://www.nuget.org/packages/HttpClientToCurl" target="_blank">HttpClientToCurl</a>

## 📚 Documentation

For full examples, detailed usage, and advanced configuration options, please see the **Wiki**:

👉 [Open Wiki → More Details](https://github.com/amingolmahalle/HttpClientToCurlGenerator/wiki)

## 🚀 **Usage Example**

### ⚡ Quick Usage

#### 1️⃣ Global Registration

Enable cURL generation globally so that every `HttpClient` created through `IHttpClientFactory` automatically logs cURL commands before sending requests.

**Program.cs / Startup:**

```csharp
using HttpClientToCurl;

// Register global cURL generation mode
builder.Services.AddHttpClientToCurlInGeneralMode(builder.Configuration);

// Register a default HttpClient (now cURL-enabled)
builder.Services.AddHttpClient();
```

#### 2️⃣ Specific Registration

If you only want cURL generation for specific clients, you can enable it per-client easily using the built-in registration helpers.

**Program.cs / Startup:**

```csharp
using HttpClientToCurl;

// Register the cURL generator service once
builder.Services.AddHttpClientToCurl(builder.Configuration);

// Then register specific HttpClients with cURL logging enabled
builder.Services.AddHttpClient("my-client1", showCurl: true);
```

### ⚙️ Manual Usage

```csharp
using System.Text;
using HttpClientToCurl;

class Program
{
    static async Task Main()
    {
        var requestUri = "api/test";

        using var httpClientInstance = new HttpClient();
        var baseAddress = new Uri("http://localhost:1213/v1/");
        httpClientInstance.BaseAddress = baseAddress;
        string requestBody = /*lang=json,strict*/ @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";
        HttpRequestMessage httpRequestMessageInstance = new(HttpMethod.Post, requestUri);
        httpRequestMessageInstance.Headers.Add("Authorization", "Bearer YourAccessToken");
        httpRequestMessageInstance.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        // Using the HttpClient extension:
        httpClientInstance.GenerateCurlInConsole(httpRequestMessageInstance);

        // Or using the HttpRequestMessage extension:
        httpRequestMessageInstance.GenerateCurlInConsole(baseAddress);

        await httpClientInstance.SendAsync(httpRequestMessageInstance);
    }
}
```

## ✅ Output:

```bash
curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer YourAccessToken'
-H 'Content-Type: application/json; charset=utf-8' -d '{"name":"sara","requestId":10001001,"amount":20000}'
```

## 🧩 **Other Features**

Works with **GET**, **POST**, **PUT**, **PATCH**, and **DELETE**

Supports **JSON**, **XML**, and **FormUrlEncodedContent**

## **Output to:**

**Console** / **String** / **File**

## 📚 Articles

- [How to Generate cURL Script of the HttpClient in .NET](https://www.c-sharpcorner.com/article/how-to-generate-curl-script-of-the-httpclient-in-net/)
- [New Feature in HttpClientToCurl for .NET: Debugging HttpRequestMessage Made Easy](https://medium.com/@mozhgan.etaati/new-feature-in-httpclienttocurl-for-net-debugging-httprequestmessage-made-easy-18cb66dd55f0)


## 💡 **Contribute**

Found a bug or want to improve this project?
Open an issue or submit a pull request.

📧 Contact: amin.golmahalle@gmail.com

## ⭐ **Give a Star**

If you find this project helpful, please give it a ⭐ — it helps others discover it too!

## 🙌 **Contributors**

<a href="https://github.com/amingolmahalle/HttpClientToCurlGenerator/graphs/contributors"> <img src="https://contrib.rocks/image?repo=amingolmahalle/HttpClientToCurlGenerator" /> </a> 
