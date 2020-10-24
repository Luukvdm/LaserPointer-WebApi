using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetUnfinishedJobsQuery;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace LaserPointer.WebApi.WebApi.Controllers
{
    [Route("jobEvents")]
    public class JobEventsController : ApiController
    {
        private readonly IServerSentEventsService _sseService;

        public JobEventsController(IServerSentEventsService sseService)
        {
            _sseService = sseService;
        }

        [HttpGet("getactive")]
        public async Task<ActionResult<UnfinishedJobsVm>> GetActive()
        {
            return await Mediator.Send(new GetUnfinishedJobsQuery());
        }
        
        [HttpGet("getactivestream")]
        public async Task GetActiveStream(CancellationToken cancellationToken)
        {
            var response = Response;

            response.ContentType = "text/event-stream";
            await response.Body.FlushAsync(cancellationToken);
            
            var client = new ServerSentEventsClient(response);
            var clientId = _sseService.AddClient(client);

            response.HttpContext.RequestAborted.WaitHandle.WaitOne();

            _sseService.RemoveClient(clientId);
        }
    }
}
