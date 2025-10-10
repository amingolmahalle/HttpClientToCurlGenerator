# 🥇 HttpClientToCurl
An extension for generating the **CURL script** from **`HttpClient`** and **`HttpRequestMessage`** in .NET.

[![license](https://img.shields.io/github/license/amingolmahalle/HttpClientToCurlGenerator)](https://github.com/amingolmahalle/HttpClientToCurlGenerator/blob/master/LICENSE)
[![stars](https://img.shields.io/github/stars/amingolmahalle/HttpClientToCurlGenerator)](https://github.com/amingolmahalle/HttpClientToCurlGenerator/stargazers)
[![NuGet Version](https://img.shields.io/nuget/v/HttpClientToCurl.svg)](https://www.nuget.org/packages/HttpClientToCurl/)
![Build](https://github.com/amingolmahalle/HttpClientToCurlGenerator/actions/workflows/dotnet.yml/badge.svg)

---

## 📖 Overview
**`HttpClientToCurl`** is a lightweight **.NET extension library** that helps you visualize any HTTP request as a **CURL command**.

You can use its extension methods on both:
- **`HttpClient`** — to generate cURL directly when sending requests  
- **`HttpRequestMessage`** — to inspect or log cURL representations before sending  

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

## 🚀 **Usage Example**
```csharp
using var httpClient = new HttpClient();
var baseAddress = new Uri("http://localhost:1213/v1/");
client.BaseAddress = baseAddress;
var request = new HttpRequestMessage(HttpMethod.Post, "api/test")
{
    Content = new StringContent(@"{ ""name"": ""amin"" }", Encoding.UTF8, "application/json")
};

// Using the HttpClient extension:
httpClient.GenerateCurlInConsole(request);

// Or using the HttpRequestMessage extension:
string curlScript = request.GenerateCurlInString(baseAddress);
```

## ✅ Output:

```bash
curl -X POST "http://localhost:1213/v1/api/test" -H "Content-Type: application/json" -d "{ \"name\": \"amin\" }"
```

## 🧩 **Other Features**

Works with **GET**, **POST**, **PUT**, **PATCH**, and **DELETE**

Supports **JSON**, **XML**, and **FormUrlEncodedContent**

## **Output to:**

**Console** / **String** / **File**

**See more details in the [Wiki](https://github.com/amingolmahalle/HttpClientToCurlGenerator/wiki) **

## 📚 Articles

### English Articles
- [How to Generate cURL Script of the HttpClient in .NET](https://www.c-sharpcorner.com/article/how-to-generate-curl-script-of-the-httpclient-in-net/)
- [New Feature in HttpClientToCurl for .NET: Debugging HttpRequestMessage Made Easy](https://medium.com/@mozhgan.etaati/new-feature-in-httpclienttocurl-for-net-debugging-httprequestmessage-made-easy-18cb66dd55f0)

### Persian Articles
- [دریافت خروجی Curl از HttpClient در دات‌نت (.NET)](https://virgool.io/@amin.golmahalle/%D8%AF%D8%B1%DB%8C%D8%A7%D9%81%D8%AA-%D8%AE%D8%B1%D9%88%D8%AC%DB%8C-curl-%D8%A7%D8%B2-httpclient-%D8%AF%D8%B1-%D8%AF%D8%A7%D8%AA-%D9%86%D8%AA-vgamgtw2midt)
- [دیباگ کردن HttpClient در دات‌نت (.NET)](https://virgool.io/@amin.golmahalle/%D8%AF%DB%8C%D8%A8%D8%A7%DA%AF-%DA%A9%D8%B1%D8%AF%D9%86-httpclient-%D8%AF%D8%B1-%D8%AF%D8%A7%D8%AA-%D9%86%D8%AA-net-jm0kcsmucrbk)


## 💡 **Contribute**

Found a bug or want to improve this project?
Open an issue or submit a pull request.

📧 Contact: amin.golmahalle@gmail.com

## ⭐ **Give a Star**

If you find this project helpful, please give it a ⭐ — it helps others discover it too!

## 🙌 **Contributors**

<a href="https://github.com/amingolmahalle/HttpClientToCurlGenerator/graphs/contributors"> <img src="https://contrib.rocks/image?repo=amingolmahalle/HttpClientToCurlGenerator" /> </a> 
