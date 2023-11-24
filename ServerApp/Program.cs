using System.Net;

HttpListener httpListener = new HttpListener();

const int port = 9999;

httpListener.Prefixes.Add($"http://*:{port}/");
httpListener.Start();
System.Console.WriteLine($"Server started on '{port}' port");

while (true)
{
    var context = await httpListener.GetContextAsync();
    var rawUrl = context.Request.RawUrl?.Trim('/').ToLower();
    using var writer = new StreamWriter(context.Response.OutputStream);
    var rawItems = rawUrl?.Split('/');
}