using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LaserPointer.WebApi.WebApi.Sse
{
    internal static class SseHttpResponseExtension
    {
        internal static async Task WriteSseEventAsync(this HttpResponse response, IClientEvent clientEvent)
        {
            if (!string.IsNullOrWhiteSpace(clientEvent.Id))
                await response.WriteSseEventFieldAsync("id", clientEvent.Id);

            if (!string.IsNullOrWhiteSpace(clientEvent.Type))
                await response.WriteSseEventFieldAsync("event", clientEvent.Type);

            if (clientEvent.Data != null)
            {
                // TODO this is very ugly :(
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                
                foreach (var data in clientEvent.Data)
                {
                    string msg;
                    if (data.GetType() != typeof(string)) msg = JsonSerializer.Serialize(clientEvent.Data, jsonOptions);
                    else msg = data as string;
                    
                    await response.WriteSseEventFieldAsync("data", msg);
                }
            }

            await response.WriteSseEventBoundaryAsync();
            await response.Body.FlushAsync();
        }
        
        private static Task WriteSseEventFieldAsync(this HttpResponse response, string field, string data)
        {
            return response.WriteAsync($"{field}: {data}\n");
        }

        private static Task WriteSseEventBoundaryAsync(this HttpResponse response)
        {
            return response.WriteAsync("\n");
        }
    }
}
