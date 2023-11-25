using System.Net;
using SimpleInjector;
using ServerApp.Functions;
using ServerApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Container container = new Container();
        ServerDbContext serverDbContext = new ServerDbContext();  

        ServerDbFunctions.RegisterContainer(container);
        serverDbContext.Database.EnsureCreated();

        HttpListener httpListener = new HttpListener();

        const int port = 55555;

        httpListener.Prefixes.Add($"http://*:{port}/");
        httpListener.Start();
        Console.WriteLine($"Server started on '{port}' port");

        while (true)
        {
            var context = await httpListener.GetContextAsync();
            var rawUrl = context.Request.RawUrl?.Trim('/').ToLower();
            using var writer = new StreamWriter(context.Response.OutputStream);
            var rawItems = rawUrl?.Split('/');
            context.Response.ContentType = "application/json";

            if (context.Request.HttpMethod == HttpMethod.Get.Method)
            {
                if (rawItems.First() == "clients")
                {
                    if (rawItems.Last() == "getall" && rawItems.SkipLast(1).Last() == "clients")
                    {
                        var jsonClients = JsonSerializer.Serialize(serverDbContext.Clients.AsEnumerable());

                        if (jsonClients != null)
                        {
                            context.Response.StatusCode = 200;
                            await writer.WriteLineAsync(jsonClients);
                        }
                    }
                }
                else if (rawItems.First() == "operators")
                {
                    if (rawItems.Last() == "getall" && rawItems.SkipLast(1).Last() == "operators")
                    {
                        var jsonOperators = JsonSerializer.Serialize(serverDbContext.Operators.AsEnumerable());

                        if (jsonOperators != null)
                        {
                            context.Response.StatusCode = 200;
                            await writer.WriteLineAsync(jsonOperators);
                        }
                    }
                }
                else if (rawItems.First() == "tables")
                {
                    if (rawItems.Last() == "getall" && rawItems.SkipLast(1).Last() == "tables")
                    {
                        var jsonTables = JsonSerializer.Serialize(serverDbContext.Tables.AsEnumerable());

                        if (jsonTables != null)
                        {
                            context.Response.StatusCode = 200;
                            await writer.WriteLineAsync(jsonTables);
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "plain/text";
                    await writer.WriteLineAsync("Error 404. Not Found!");
                }
            }
            else if (context.Request.HttpMethod == HttpMethod.Post.Method)
            {

            }
            else if (context.Request.HttpMethod == HttpMethod.Put.Method)
            {

            }
            else if (context.Request.HttpMethod == HttpMethod.Delete.Method)
            {

            }
        }
    }
}