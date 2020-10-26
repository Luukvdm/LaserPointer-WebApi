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
        internal static async Task WriteSseEventAsync(this HttpResponse response, IServerSentEvent serverSentEvent)
        {
            if (!string.IsNullOrWhiteSpace(serverSentEvent.Id))
                await response.WriteSseEventFieldAsync("id", serverSentEvent.Id);

            if (!string.IsNullOrWhiteSpace(serverSentEvent.Type))
                await response.WriteSseEventFieldAsync("event", serverSentEvent.Type);

            if (serverSentEvent.Data != null)
            {
                // TODO this is very ugly :(
                var jsonOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                
                foreach (var data in serverSentEvent.Data)
                {
                    string msg;
                    if (data.GetType() != typeof(string)) msg = JsonSerializer.Serialize(serverSentEvent.Data, jsonOptions);
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
