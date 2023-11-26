using System.Net;
using System.Text.Json;
using ServerApp.Data;
using ServerApp.Repositories;
using ServerApp.Repositories.Base;
using SharedLib.Models;
using SimpleInjector;

namespace ServerApp.Functions
{
    public class ServerDbFunctions
    {
        private static IClientRepository clientRepository = new ClientEFRepository();
        private static IOperatorRepository operatorRepository = new OperatorEFRepository();
        private static ITableRepository tableRepository = new TableEFRepository();

        public static void RegisterContainer(Container container)
        {
            container.RegisterSingleton<ITableRepository, TableEFRepository>();
            container.RegisterSingleton<IClientRepository, ClientEFRepository>();
            container.RegisterSingleton<IOperatorRepository, OperatorEFRepository>();

            container.Verify();
        }

        public static void InitialDbCreate(ServerDbContext serverDbContext)
        {
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
            }

        public static void CreateHttpServer(HttpListener httpListener, int port)
        {
            httpListener.Prefixes.Add($"http://*:{port}/");
            httpListener.Start();
            Console.WriteLine($"Server started on '{port}' port");
        }

        public async static Task HttpGetMethods(HttpListenerContext? context, string[]? rawItems, StreamWriter writer, ServerDbContext serverDbContext)
        {
            if (rawItems?.FirstOrDefault() == "clients" && rawItems.Length == 1)
            {
                clientRepository.SentAll(context, writer, serverDbContext);
            }
            else if (rawItems?.FirstOrDefault() == "operators")
            {
                operatorRepository.SentAll(context, writer, serverDbContext);
            }
            else if (rawItems.First() == "tables")
            {
                tableRepository.SentAll(context, writer, serverDbContext);
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "plain/text";
                await writer.WriteLineAsync("Error 404. Not Found!");
                writer.Dispose();
            }
        }

        public static async Task HttpPostMethods(HttpListenerContext? context, string[]? rawItems, StreamWriter writer, StreamReader reader, ServerDbContext serverDbContext)
        {
            if (rawItems?.FirstOrDefault() == "clients")
            {
                clientRepository.Add(context, writer, reader, serverDbContext);
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "plain/text";
                await writer.WriteLineAsync("Error 404. Not Found!");
            }
        }

        public static async Task HttpPutMethods(HttpListenerContext? context, string[]? rawItems, StreamWriter writer, StreamReader reader, ServerDbContext serverDbContext)
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

        public static async Task HttpDeleteMethods(HttpListenerContext? context, string[]? rawItems, StreamWriter writer, ServerDbContext serverDbContext)
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

                            var movingClients = serverDbContext.Clients.Where(c => c.QueueNumber > removingClient.QueueNumber);

                            foreach (var movingClient in movingClients)
                            {
                                movingClient.QueueNumber--;
                            }
                            serverDbContext.Clients.UpdateRange(movingClients);

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