using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Features.Jobs.Commands.CreateJobCommand;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetJobDetailsQuery;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetMyJobsQuery;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace LaserPointer.WebApi.WebApi.Controllers
{
    [Authorize]
    public class JobsController : ApiController
    {
        /// <summary>
        /// Get all the possible states for a job.
        /// </summary>
        /// <returns>List with Status' as strings.</returns>
        [HttpGet("states")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<JobStatus>), Status200OK)]
        [AllowAnonymous]
        public ActionResult<IEnumerable<JobStatus>> States()
        { 
            var list = Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>();
            return Ok(list);
        }

        /// <summary>
        /// Get all the jobs from the logged in person.
        /// </summary>
        /// <returns>Object with jobs.</returns>
        [HttpGet("my")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MyJobsVm), Status200OK)]
        public async Task<ActionResult<MyJobsVm>> My()
        {
            return await Mediator.Send(new GetMyJobsQuery());
        }
        
        /// <summary>
        /// Get the details of a job.
        /// </summary>
        /// <param name="id">The job ID.</param>
        /// <returns>A job with its details.</returns>
        [HttpGet("details/{id:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JobDetailsDto), Status200OK)]
        [ProducesResponseType(Status204NoContent)]
        public async Task<ActionResult<JobDetailsDto>> My(int id)
        {
            return await Mediator.Send(new GetJobDetailsQuery(id));
        }

        /// <summary>
        /// Create a job for LaserPointer to execute.
        /// </summary>
        /// <param name="command">Object with the hash type and a list of hashes.</param>
        /// <returns>Id of the job.</returns>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(int), Status200OK)]
        public async Task<ActionResult<int>> Create(CreateJobCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
