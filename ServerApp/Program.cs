using System.Net;
using SimpleInjector;
using ServerApp.Functions;
using ServerApp.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    

    private static async Task Main(string[] args)
    {
        Container container = new Container();
        ServerDbContext serverDbContext = new ServerDbContext();  

        ServerDbFunctions.RegisterContainer(container);
        serverDbContext.Database.EnsureCreated();

        HttpListener httpListener = new HttpListener();

        const int port = 9999;

        httpListener.Prefixes.Add($"http://*:{port}/");
        httpListener.Start();
        Console.WriteLine($"Server started on '{port}' port");

        while (false)
        {
            var context = await httpListener.GetContextAsync();
            var rawUrl = context.Request.RawUrl?.Trim('/').ToLower();
            using var writer = new StreamWriter(context.Response.OutputStream);
            var rawItems = rawUrl?.Split('/');
        }
    }
}