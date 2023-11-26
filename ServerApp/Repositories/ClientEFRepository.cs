using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ServerApp.Data;
using ServerApp.Repositories.Base;
using SharedLib.Models;

namespace ServerApp.Repositories
{
    public class ClientEFRepository : IClientRepository
    {
        public async Task Add(HttpListenerContext? context, StreamWriter writer, StreamReader reader, ServerDbContext serverDbContext)
        {
            var newClientJson = await reader.ReadToEndAsync();
            try
            {
                Client? newClient = JsonSerializer.Deserialize<Client>(newClientJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.Preserve });
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
            writer.Dispose();
            reader.Dispose();
        }

        public async Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            var jsonClients = JsonSerializer.Serialize(serverDbContext.Clients.AsEnumerable(), options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            if (jsonClients != null)
            {
                context.Response.StatusCode = 200;
                await writer.WriteLineAsync(jsonClients);
            }
            writer.Dispose();
        }

        public static void FinishUpdate(Client? updatedClient, Client oldClient, ServerDbContext serverDbContext)
        {
            Client deposedClient = serverDbContext.Clients.First(c => c.TableId == updatedClient.TableId && c.QueueNumber == updatedClient.QueueNumber);
            deposedClient.QueueNumber = oldClient.QueueNumber;
            oldClient.QueueNumber = updatedClient.QueueNumber;
            serverDbContext.UpdateRange(oldClient, deposedClient);
            serverDbContext.SaveChanges();
        }

        public async Task UpdateDown(Client? updatedClient, HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            try
            {
                var oldClient = serverDbContext.Clients.First(c => c.Id == updatedClient.Id && c.TableId == updatedClient.TableId);

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
                    FinishUpdate(updatedClient, oldClient, serverDbContext);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 405;
                context.Response.ContentType = "plain/text";
                await writer.WriteLineAsync($"Error 405. Method not allowed! {ex.Message}");
            }
            writer.Dispose();
        }

        public async Task UpdateUp(Client? updatedClient, HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            try
            {
                var oldClient = serverDbContext.Clients.First(c => c.Id == updatedClient.Id && c.TableId == updatedClient.TableId);

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
                    FinishUpdate(updatedClient, oldClient, serverDbContext);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 405;
                context.Response.ContentType = "plain/text";
                await writer.WriteLineAsync($"Error 405. Method not allowed! {ex.Message}");
            }
            
            writer.Dispose();
        }
    }
}