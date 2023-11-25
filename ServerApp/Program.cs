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

            if (rawItems.First() == "clients")
            {
                if (rawItems.Last() == "getall" && rawItems.SkipLast(1).Last() == "clients")
                {
                    if (context.Request.HttpMethod == HttpMethod.Get.Method)
                    {
                        var jsonClients = JsonSerializer.Serialize(serverDbContext.Clients.AsEnumerable());

                        if (jsonClients != null)
                        {
                            context.Response.StatusCode = 200;
                            await writer.WriteLineAsync(jsonClients);
                        }
                    }
                }
                else if (int.TryParse(rawItems.Last(), out int id) && rawItems.SkipLast(1).Last() == "clients")
                {
                    var jsonClients = JsonSerializer.Serialize(serverDbContext.Clients.First(c => c.Id == id));

                    if (jsonClients != null)
                    {
                        context.Response.StatusCode = 200;
                        await writer.WriteLineAsync(jsonClients);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "plain/text";
                    await writer.WriteLineAsync("Error 400. Bad request!");
                }
            }
            else if (rawItems.First() == "operators")
            {
                if (rawItems.Last() == "getall" && rawItems.SkipLast(1).Last() == "operators")
                {
                    if (context.Request.HttpMethod == HttpMethod.Get.Method)
                    {
                        var jsonOperators = JsonSerializer.Serialize(serverDbContext.Operators.AsEnumerable());

                        if (jsonOperators != null)
                        {
                            context.Response.StatusCode = 200;
                            await writer.WriteLineAsync(jsonOperators);
                        }
                    }
                }
                else if (int.TryParse(rawItems.Last(), out int id) && rawItems.SkipLast(1).Last() == "operators")
                {
                    var jsonOperators = JsonSerializer.Serialize(serverDbContext.Operators.First(c => c.Id == id));

                    if (jsonOperators != null)
                    {
                        context.Response.StatusCode = 200;
                        await writer.WriteLineAsync(jsonOperators);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "plain/text";
                    await writer.WriteLineAsync("Error 400. Bad request!");
                }
            }
            else if (rawItems.First() == "tables")
            {
                if (rawItems.Last() == "getall" && rawItems.SkipLast(1).Last() == "tables")
                {
                    if (context.Request.HttpMethod == HttpMethod.Get.Method)
                    {
                        var jsonTables = JsonSerializer.Serialize(serverDbContext.Tables.AsEnumerable());

                        if (jsonTables != null)
                        {
                            context.Response.StatusCode = 200;
                            await writer.WriteLineAsync(jsonTables);
                        }
                    }
                }
                else if (int.TryParse(rawItems.Last(), out int id) && rawItems.SkipLast(1).Last() == "tables")
                {
                    var jsonTables = JsonSerializer.Serialize(serverDbContext.Tables.First(c => c.Id == id));

                    if (jsonTables != null)
                    {
                        context.Response.StatusCode = 200;
                        await writer.WriteLineAsync(jsonTables);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "plain/text";
                    await writer.WriteLineAsync("Error 400. Bad request!");
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                await writer.WriteLineAsync("Error 404. Not Found!");
            }
        }
    }
}