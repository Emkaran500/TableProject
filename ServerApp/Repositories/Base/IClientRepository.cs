using System.Net;
using ServerApp.Data;
using SharedLib.Models;

namespace ServerApp.Repositories.Base
{
    public interface IClientRepository
    {
        public Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext);
        public Task Add(HttpListenerContext? context, StreamWriter writer, StreamReader reader, ServerDbContext serverDbContext);
        public Task UpdateUp(Client? updatedClient, HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext);
        public Task UpdateDown(Client? updatedClient, HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext);
        public Task Delete(HttpListenerContext? context, string[]? rawItems, StreamWriter writer, ServerDbContext serverDbContext);
    }
}