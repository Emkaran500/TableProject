using System.Net;
using SimpleInjector;
using ServerApp.Functions;
using ServerApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using SharedLib.Models;

internal class Program
{
    private static async Task Main()
    {
        const int port = 55555;
        Container container = new Container();
        ServerDbContext serverDbContext = new ServerDbContext();
        HttpListener httpListener = new HttpListener();

        ServerDbFunctions.RegisterContainer(container);
        ServerDbFunctions.InitialDbCreate(serverDbContext);
        ServerDbFunctions.CreateHttpServer(httpListener, port);


        while (true)
        {
            var context = await httpListener.GetContextAsync();
            var rawUrl = context.Request.RawUrl?.Trim('/').ToLower();
            var writer = new StreamWriter(context.Response.OutputStream);
            var reader = new StreamReader(context.Request.InputStream);
            var rawItems = rawUrl?.Split('/');
            context.Response.ContentType = "application/json";

            if (context.Request.HttpMethod == HttpMethod.Get.Method)
            {
                ServerDbFunctions.HttpGetMethods(context, rawItems, writer, serverDbContext);
            }
            else if (context.Request.HttpMethod == HttpMethod.Post.Method)
            {
                ServerDbFunctions.HttpPostMethods(context, rawItems, writer, reader, serverDbContext);
            }
            else if (context.Request.HttpMethod == HttpMethod.Put.Method)
            {
                ServerDbFunctions.HttpPutMethods(context, rawItems, writer, reader, serverDbContext);
            }
            else if (context.Request.HttpMethod == HttpMethod.Delete.Method)
            {
                ServerDbFunctions.HttpDeleteMethods(context, rawItems, writer, serverDbContext);
            }
        }
    }
}