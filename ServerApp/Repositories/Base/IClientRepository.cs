using System.Net;
using ServerApp.Data;
using SharedLib.Models;

namespace ServerApp.Repositories.Base
{
    public interface IClientRepository
    {
        public Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext);
        public Task Add(HttpListenerContext? context, StreamWriter writer, StreamReader reader, ServerDbContext serverDbContext);
    }
}