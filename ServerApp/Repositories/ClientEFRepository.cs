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
        public void Add(Client order)
        {
            throw new NotImplementedException();
        }

        public async Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            var jsonClients = JsonSerializer.Serialize(serverDbContext.Clients.AsEnumerable(), options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            if (jsonClients != null)
            {
                context.Response.StatusCode = 200;
                await writer.WriteLineAsync(jsonClients);
                writer.Dispose();
            }
        }
    }
}