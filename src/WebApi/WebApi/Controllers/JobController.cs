using System;
using System.Collections.Generic;
using System.Linq;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace LaserPointer.WebApi.WebApi.Controllers
{
    [Route("jobs")]
    public class JobsController : ApiController
    {
        [HttpGet("goals")]
        public ActionResult<IEnumerable<JobStatus>> GetGoals()
        { 
            var list = Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>();
            return Ok(list);
        }
    }
}
