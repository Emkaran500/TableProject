using System.Net;
using SimpleInjector;
using ServerApp.Functions;
using ServerApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using SharedLib.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Container container = new Container();
        ServerDbContext serverDbContext = new ServerDbContext();  

        ServerDbFunctions.RegisterContainer(container);
        serverDbContext.Database.EnsureCreated();

        if (serverDbContext.Operators.Any() == false)
        {
            serverDbContext.Operators.AddRange(new Operator { Name = "Emil"}, new Operator { Name = "Nijat"}, new Operator { Name = "Ayhan"}, new Operator { Name = "Qasim"}, new Operator { Name = "Raul"});
        }
        if (serverDbContext.Tables.Any() == false)
        {
            serverDbContext.Tables.AddRange(new Table { TableNumber = 1}, new Table { TableNumber = 2}, new Table { TableNumber = 3}, new Table { TableNumber = 4}, new Table { TableNumber = 5});
        }
        serverDbContext.SaveChanges();

        if (serverDbContext.TableOperators.Any() == false)
        {
            serverDbContext.TableOperators.AddRange(new TableOperator { OperatorId = 1, TableId = 1}, new TableOperator { OperatorId = 2, TableId = 2}, new TableOperator { OperatorId = 3, TableId = 3}, new TableOperator { OperatorId = 4, TableId = 4}, new TableOperator { OperatorId = 5, TableId = 5});
        }
        serverDbContext.SaveChanges();

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
            using var reader = new StreamReader(context.Request.InputStream);
            var rawItems = rawUrl?.Split('/');
            context.Response.ContentType = "application/json";

            if (context.Request.HttpMethod == HttpMethod.Get.Method)
            {
                if (rawItems?.FirstOrDefault() == "clients")
                {
                    if (rawItems.LastOrDefault() == "getall" && rawItems.SkipLast(1).Last() == "clients")
                    {
                        var jsonClients = JsonSerializer.Serialize(serverDbContext.Clients.AsEnumerable());

                        if (jsonClients != null)
                        {
                            context.Response.StatusCode = 200;
                            await writer.WriteLineAsync(jsonClients);
                        }
                    }
                }
                else if (rawItems?.FirstOrDefault() == "operators")
                {
                    if (rawItems?.LastOrDefault() == "getall" && rawItems.SkipLast(1).LastOrDefault() == "operators")
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
                if (rawItems?.FirstOrDefault() == "clients")
                {
                    var newClientJson = await reader.ReadToEndAsync();
                    try
                    {
                        Client? newClient = JsonSerializer.Deserialize<Client>(newClientJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        newClient.Table = serverDbContext.Tables.FirstOrDefault(t => t.Id == newClient.TableId);
                        newClient.QueueNumber = serverDbContext.Clients.Where(c => c.TableId == newClient.TableId).Count() + 1;
                        serverDbContext.Clients.Add(newClient);
                        serverDbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 405;
                        context.Response.ContentType = "plain/text";
                        await writer.WriteLineAsync($"Error 405. Method not allowed! {ex.Message}");
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "plain/text";
                    await writer.WriteLineAsync("Error 404. Not Found!");
                }
            }
            else if (context.Request.HttpMethod == HttpMethod.Put.Method)
            {
                if (rawItems?.FirstOrDefault() == "clients" && rawItems.Last().Contains("?up=") && rawItems.Length == 2)
                {
                    var updatedClientJson = await reader.ReadToEndAsync();

                    try
                    {
                        Client? updatedClient = JsonSerializer.Deserialize<Client>(updatedClientJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });                        
                        var oldClient = serverDbContext.Clients.First(c => c.Id == updatedClient.Id);

                        if (rawItems.Last().Contains("?up=true"))
                        {
                            if (updatedClient.QueueNumber >= oldClient.QueueNumber)
                            {
                                throw new Exception("Wrong use of '?up'!");
                            }

                            if (updatedClient.QueueNumber <= 1 || oldClient.QueueNumber - updatedClient.QueueNumber > 1)
                            {
                                context.Response.StatusCode = 405;
                                context.Response.ContentType = "plain/text";
                                await writer.WriteLineAsync("Error 405. Method not allowed!");
                            }
                            else
                            {
                                Client deposedClient = serverDbContext.Clients.First(c => c.TableId == updatedClient.TableId && c.QueueNumber == updatedClient.QueueNumber);
                                deposedClient.QueueNumber = oldClient.QueueNumber;
                                oldClient.QueueNumber = updatedClient.QueueNumber;
                                serverDbContext.UpdateRange(oldClient, deposedClient);
                                serverDbContext.SaveChanges();
                            }
                        }
                        else if (rawItems.Last().Contains("?up=false"))
                        {
                            if (updatedClient.QueueNumber <= oldClient.QueueNumber)
                            {
                                throw new Exception("Wrong use of '?up'!");
                            }
                            
                            if (updatedClient.QueueNumber == 1 || updatedClient.QueueNumber - oldClient.QueueNumber > 1 || serverDbContext.Clients.FirstOrDefault(c => c.TableId == oldClient.TableId && c.QueueNumber > oldClient.QueueNumber) == null)
                            {
                                context.Response.StatusCode = 405;
                                context.Response.ContentType = "plain/text";
                                await writer.WriteLineAsync("Error 405. Method not allowed!");
                            }
                            else
                            {
                                Client deposedClient = serverDbContext.Clients.First(c => c.TableId == updatedClient.TableId && c.QueueNumber == updatedClient.QueueNumber);
                                deposedClient.QueueNumber = oldClient.QueueNumber;
                                oldClient.QueueNumber = updatedClient.QueueNumber;
                                serverDbContext.UpdateRange(oldClient, deposedClient);
                                serverDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                            context.Response.ContentType = "plain/text";
                            await writer.WriteLineAsync("Error 400. Bad request!");
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 405;
                        context.Response.ContentType = "plain/text";
                        await writer.WriteLineAsync($"Error 405. Method not allowed! {ex.Message}");
                    }
                }
            }
            else if (context.Request.HttpMethod == HttpMethod.Delete.Method)
            {
                if (rawItems?.FirstOrDefault() == "clients" && rawItems.Length == 2)
                {
                    try
                    {
                        if (rawItems.Last().Contains("?id="))
                        {
                            try
                            {
                                int.TryParse(rawItems.Last().Skip(4).ToArray(), out int id);
                                Client? removingClient = serverDbContext.Clients.First(c => c.Id == id);
                                serverDbContext.Clients.Remove(removingClient);
                                serverDbContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = 400;
                                context.Response.ContentType = "plain/text";
                                await writer.WriteLineAsync($"Error 400. Bad request! {ex.Message}");
                            }
                        }
                        else
                        {
                            throw new Exception("Wrong URL after /clients/!");
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 405;
                        context.Response.ContentType = "plain/text";
                        await writer.WriteLineAsync($"Error 405. Method not allowed! {ex.Message}");
                    }
                }
            }
        }
    }
}