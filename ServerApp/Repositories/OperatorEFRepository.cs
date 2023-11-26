using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ServerApp.Data;
using ServerApp.Repositories.Base;
using SharedLib.Models;

namespace ServerApp.Repositories
{
    public class OperatorEFRepository : IOperatorRepository
    {
        public async Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            var jsonOperators = JsonSerializer.Serialize(serverDbContext.Operators.AsEnumerable(), options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            if (jsonOperators != null)
            {
                context.Response.StatusCode = 200;
                await writer.WriteLineAsync(jsonOperators);
                writer.Dispose();
            }
        }
    }
}