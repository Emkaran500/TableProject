using System.Net;
using ServerApp.Data;
using SharedLib.Models;

namespace ServerApp.Repositories.Base
{
    public interface IOperatorRepository
    {
        public Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext);
        public Task CheckAutorisation(Operator? questionOperator, HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext);
    }
}