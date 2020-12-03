using System;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetUnfinishedJobsQuery;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaserPointer.WebApi.WebApi.Controllers
{
    [Authorize]
    public class JobEventsController : ApiController
    {
        private readonly IClientEventService _sseService;

        public JobEventsController(IClientEventService sseService)
        {
            _sseService = sseService;
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult<UnfinishedJobsVm>> GetActive()
        {
            return await Mediator.Send(new GetUnfinishedJobsQuery());
        }
        
        [HttpGet("activeStream")]
        [AllowAnonymous]
        public async Task GetActiveStream()
        {
            var response = Response;

            Response.StatusCode = 200;
            response.Headers.Add("Content-Type", "text/event-stream");
            // response.ContentType = "text/event-stream";
            
            await Response.Body.FlushAsync();

            var client = new ClientEventDispatcher(response);
            var clientId = _sseService.AddClient(client);

            response.HttpContext.RequestAborted.WaitHandle.WaitOne();

            _sseService.RemoveClient(clientId);
        }
    }
}
