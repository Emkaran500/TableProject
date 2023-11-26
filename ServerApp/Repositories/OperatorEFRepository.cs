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
        public async Task CheckAutorisation(Operator? questionOperator, HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            try
            {
                Operator? correctOperator = serverDbContext.Operators.FirstOrDefault(o => o.Name == questionOperator.Name && o.Pass == questionOperator.Pass);

                if (correctOperator == null)
                {
                    throw new Exception("Invalid credentials!");
                }
                else
                {
                    var correctOperatorJson = JsonSerializer.Serialize(correctOperator, options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
                    context.Response.StatusCode = 200;
                    await writer.WriteLineAsync(correctOperatorJson);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "plain/text";
                await writer.WriteLineAsync($"Error 401. Unauthorized access! {ex.Message}");
            }
            writer.Dispose();
        }

        public async Task SentAll(HttpListenerContext? context, StreamWriter writer, ServerDbContext serverDbContext)
        {
            var jsonOperators = JsonSerializer.Serialize(serverDbContext.Operators.AsEnumerable(), options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            if (jsonOperators != null)
            {
                context.Response.StatusCode = 200;
                await writer.WriteLineAsync(jsonOperators);
            }
            writer.Dispose();
        }
    }
}