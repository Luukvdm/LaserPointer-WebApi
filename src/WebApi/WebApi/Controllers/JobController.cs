using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Features.Jobs.Commands.CreateJobCommand;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetJobDetailsQuery;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetMyJobsQuery;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaserPointer.WebApi.WebApi.Controllers
{
    [Authorize]
    public class JobsController : ApiController
    {
        [HttpGet("states")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<JobStatus>> States()
        { 
            var list = Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>();
            return Ok(list);
        }

        [HttpGet("my")]
        public async Task<ActionResult<MyJobsVm>> My()
        {
            return await Mediator.Send(new GetMyJobsQuery());
        }
        
        [HttpGet("details/{id:int}")]
        public async Task<ActionResult<JobDetailsDto>> My(int id)
        {
            return await Mediator.Send(new GetJobDetailsQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateJobCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
