using System.Collections.ObjectModel;
using System.Net;
using ServerApp.Data;
using SharedLib.Models;

namespace ServerApp.Repositories.Base
{
    public interface ITableRepository
    {        
        public Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext);
    }
}