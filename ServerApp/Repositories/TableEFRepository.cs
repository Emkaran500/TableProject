using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ServerApp.Data;
using ServerApp.Repositories.Base;
using SharedLib.Models;

namespace ServerApp.Repositories
{
    public class TableEFRepository : ITableRepository
    {
        public async Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            var jsonTables = JsonSerializer.Serialize(serverDbContext.Tables.AsEnumerable(), options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            System.Console.WriteLine("Hiiii");

            if (jsonTables != null)
            {
                context.Response.StatusCode = 200;
                await writer.WriteLineAsync(jsonTables);
            }
            writer.Dispose();
        }
    }
}