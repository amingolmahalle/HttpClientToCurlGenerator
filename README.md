# 🥇 HttpClientToCurl

Generate curl commands directly from your `HttpClient` or `HttpRequestMessage` in .NET — perfect for debugging, logging, and sharing HTTP requests.

### 📊 Badges
![Build](https://github.com/amingolmahalle/HttpClientToCurlGenerator/actions/workflows/dotnet.yml/badge.svg)
[![License](https://img.shields.io/github/license/amingolmahalle/HttpClientToCurlGenerator?style=flat-square)](https://github.com/amingolmahalle/HttpClientToCurlGenerator/blob/master/LICENSE)
[![Stars](https://img.shields.io/github/stars/amingolmahalle/HttpClientToCurlGenerator?style=flat-square)](https://github.com/amingolmahalle/HttpClientToCurlGenerator/stargazers)
[![NuGet Version](https://img.shields.io/nuget/v/HttpClientToCurl?style=flat-square)](https://www.nuget.org/packages/HttpClientToCurl/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HttpClientToCurl?style=flat-square)](https://www.nuget.org/packages/HttpClientToCurl/)
[![Used by](https://img.shields.io/badge/Used%20by-.NET%20Projects-512BD4?style=flat-square&logo=dotnet&logoColor=white)](
https://github.com/amingolmahalle/HttpClientToCurlGenerator/network/dependents)

---

## 📖 Overview
**HttpClientToCurl** is a lightweight and powerful .NET extension library that turns your HTTP requests into curl commands.
It works with both **`HttpClient`** and **`HttpRequestMessage`**, giving you two simple ways to generate curl commands:

---

### 🧰 1. Manual Mode

Generate curl commands **on demand** using extension methods on either `HttpClient` or `HttpRequestMessage`.

**Best for:**  
Debugging individual requests, creating reproducible Postman calls, or sharing API examples.

---

### 🧩 2. Automatic Mode

Automatically generates curl output whenever your app sends a request.  
You can configure it through dependency injection:

- **Global Registration** — enable for all `HttpClient` instances created via `IHttpClientFactory`  
- **Per-Client Registration** — enable only for selected clients  

**Best for:**  
Logging, monitoring, or tracing outgoing requests across the application.

---

### 💡 Why Use HttpClientToCurl?

- 🧪 Instantly visualise and debug request payloads or headers  
- 🤝 Share exact API calls with teammates or QA engineers  
- ⚙️ Simplify Postman and CLI reproduction  
- 🧩 Lightweight, dependency-free, and easy to integrate  

---
## ⚙️ Installation

```bash
dotnet add package HttpClientToCurl
```
Or visit the NuGet page here: <a href="https://www.nuget.org/packages/HttpClientToCurl" target="_blank">HttpClientToCurl</a>

## 📚 Documentation

For full examples, detailed usage, and advanced configuration options, please see the **Wiki**:

👉 [Open Wiki → More Details](https://github.com/amingolmahalle/HttpClientToCurlGenerator/wiki)

---

## 🚀 Quick Start

## 🧰 Manual Mode Usage Example

```csharp
using System.Text;
using HttpClientToCurl;

class Program
{
    static async Task Main()
    {
        var baseAddress = new Uri("http://localhost:1213/v1/");
        var requestUri = "api/test";

        using var httpClientInstance = new HttpClient { BaseAddress = baseAddress };

        string requestBody = @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";
        var httpRequestMessageInstance = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
        };
        httpRequestMessageInstance.Headers.Add("Authorization", "Bearer YourAccessToken");

        // Option 1: Generate curl from HttpClient
        httpClientInstance.GenerateCurlInConsole(httpRequestMessageInstance);

        // Option 2: Generate curl from HttpRequestMessage
        httpRequestMessageInstance.GenerateCurlInConsole(baseAddress);

        await httpClientInstance.SendAsync(httpRequestMessageInstance);
    }
}
```

✅ **Example Output**
```bash
curl -X POST 'http://localhost:1213/v1/api/test' \
  -H 'Authorization: Bearer YourAccessToken' \
  -H 'Content-Type: application/json; charset=utf-8' \
  -d '{"name":"sara","requestId":10001001,"amount":20000}'
```

---

## 🧩 Automatic Mode Usage Example

### 1️⃣ Per-Client Registration

Enable curl logging for specific named clients only.

**Program.cs / Startup.cs**
```csharp
using HttpClientToCurl;

// Register the curl generator once
builder.Services.AddHttpClientToCurl(builder.Configuration);

// Enable curl logging for selected clients
builder.Services.AddHttpClient("my-client1").AddCurlLogging();
```

**appsettings.json**
```json
"HttpClientToCurl": {
  "Enable": true, // Master switch: enable or disable the entire HttpClientToCURL logging system

  "ShowOnConsole": {
    "TurnOn": true, // Enable console output for generated curl commands
    "NeedAddDefaultHeaders": true, // Include default headers (like User-Agent, Accept, etc.) in the curl output
    "EnableCompression": false, // Compress the console log output (not recommended for debugging readability)
    "EnableCodeBeautification": true // Beautify and format the curl command for better readability
  },

  "SaveToFile": {
    "TurnOn": true, // Enable saving the generated curl commands into a file
    "NeedAddDefaultHeaders": true, // Include default headers (like User-Agent, Accept, etc.) in the curl output
    "EnableCompression": false, // Compress the saved file (useful if logging a large number of requests)
    "Filename": "curl_commands", // Name of the output file without extension (e.g., will produce curl_commands.log)
    "Path": "C:\\Users\\Public" // Directory path where the log file will be created
  },

  "SendToLogger": {
    "TurnOn": true, // Enable sending curl commands to ILogger (integrates with Application Insights, Seq, Serilog, etc.)
    "NeedAddDefaultHeaders": true, // Include default headers (like User-Agent, Accept, etc.) in the curl output
    "EnableCompression": false, // Compress the logged output
    "LogLevel": "Debug" // Log level: Trace, Debug, Information, Warning, Error, Critical
  }
}
```

---

### 2️⃣ Global Registration

Enable curl generation globally — every `HttpClient` created through `IHttpClientFactory` will automatically log curl commands.

**Program.cs / Startup.cs**
```csharp
using HttpClientToCurl;

// Register global curl generation
builder.Services.AddAllHttpClientToCurl(builder.Configuration);

// Register default HttpClient (now curl-enabled)
builder.Services.AddHttpClient();
```

**appsettings.json**
(same configuration options as above)

---

## 🧩 Features

| Feature | Description |
|----------|--------------|
| 🔁 Methods | Supports `GET`, `POST`, `PUT`, `PATCH`, `DELETE` |
| 🧠 Content Types | `JSON`, `XML`, `FormUrlEncodedContent` |
| 💾 Output | Console • File • String • ILogger |
| 🎨 Beautified Output | Optional pretty printing |
| 📊 Logging Integration | Works with Application Insights, Seq, Serilog, and other ILogger providers |

---

## 📚 Articles

- [How to Generate curl Script of the HttpClient in .NET](https://www.c-sharpcorner.com/article/how-to-generate-curl-script-of-the-httpclient-in-net/)
- [New Feature in HttpClientToCurl for .NET: Debugging HttpRequestMessage Made Easy](https://medium.com/@mozhgan.etaati/new-feature-in-httpclienttocurl-for-net-debugging-httprequestmessage-made-easy-18cb66dd55f0)

---

## 💖 **Love HttpClientToCurl? Please support us!**  

If this project has made your life easier, consider buying us a coffee or sending a donation.  
Every bit of support keeps us motivated, helps us add new features, fix bugs, and maintain the project — keeping it free and awesome for everyone! ☕🚀

*USDT (Tether – BEP20 / Binance Smart Chain) wallet address:*  
`0x9d03Be8B979453bE300724FD4bb3eF77517d45AE`

---

## 💡 **Contribute**

Found a bug or want to improve this project?
Open an issue or submit a pull request.

📧 Contact: amin.golmahalle@gmail.com

## ⭐ **Give a Star**

If you find this project helpful, please give it a ⭐ — it helps others discover it too!

## 🙌 **Contributors**

<a href="https://github.com/amingolmahalle/HttpClientToCurlGenerator/graphs/contributors"> <img src="https://contrib.rocks/image?repo=amingolmahalle/HttpClientToCurlGenerator" /> </a> 
