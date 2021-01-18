using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetUnfinishedJobsQuery;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

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

        /// <summary>
        /// Get all the active jobs.
        /// Active means jobs that aren't finished yet.
        /// </summary>
        /// <returns>List with job objects.</returns>
        [HttpGet("active")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<JobDto>), Status200OK)]
        public async Task<IList<JobDto>> GetActive()
        {
            return await Mediator.Send(new GetUnfinishedJobsQuery());
        }
        
        /// <summary>
        /// Event stream that returns a active job when it is updated.
        /// For instance, when a job is started its status changes so the stream sends that to the connected clients.
        /// </summary>
        /// <returns>Update events for active jobs.</returns>
        [HttpGet("activeStream")]
        [Produces("text/event-stream")]
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
