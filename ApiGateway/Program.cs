using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Steeltoe.Discovery;
using System.IO;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseDiscoveryClient();

app.MapMethods("/{serviceName}/{**path}", new[] { "GET", "POST", "DELETE" }, HandleRequestAsync);

app.MapGet("/", () => "API Gateway is running!");

app.Run();

async Task<IResult> HandleRequestAsync(string serviceName, string path, IDiscoveryClient discoveryClient, HttpClient httpClient, HttpContext context)
{
    var instances = discoveryClient.GetInstances(serviceName);

    if (!instances.Any())
    {
        return Results.NotFound($"Service '{serviceName}' not found.");
    }

    var instance = instances.First();
    var serviceUrl = instance.Uri.ToString();
    var downstreamUrl = $"{serviceUrl}{path}";

    try
    {
        var response = await ForwardRequestAsync(context, downstreamUrl, httpClient);
        return await ProcessResponseAsync(response);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while forwarding request: {ex.Message}");
        return Results.StatusCode(500);
    }
}

async Task<HttpResponseMessage> ForwardRequestAsync(HttpContext context, string downstreamUrl, HttpClient httpClient)
{
    HttpResponseMessage response;

    switch (context.Request.Method.ToUpper())
    {
        case "POST":
            var content = await GetRequestContentAsync(context);
            response = await httpClient.PostAsync(downstreamUrl, content);
            break;

        case "GET":
            response = await httpClient.GetAsync(downstreamUrl);
            break;

        case "DELETE":
            response = await httpClient.DeleteAsync(downstreamUrl);
            break;

        default:
            response = new HttpResponseMessage(System.Net.HttpStatusCode.MethodNotAllowed);
            break;
    }

    return response;
}

async Task<StringContent> GetRequestContentAsync(HttpContext context)
{
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var content = new StringContent(body);

    if (context.Request.ContentType != null)
    {
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(context.Request.ContentType);
    }

    return content;
}

async Task<IResult> ProcessResponseAsync(HttpResponseMessage response)
{
    if (response.Content != null)
    {
        var content = await response.Content.ReadAsStringAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";
        Encoding contentEncoding = null;
        return Results.Content(content,contentType, contentEncoding, (int)response.StatusCode);
    }

    return Results.StatusCode((int)response.StatusCode);
}
