using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using LaserPointer.WebApi.Application.Features.Jobs.Queries.GetUnfinishedJobsQuery;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace LaserPointer.WebApi.WebApi.Controllers
{
    
    public class HashesController : ApiController
    {
        /// <summary>
        /// Get all the supported hash types.
        /// </summary>
        /// <returns>List with Hashtypes as strings.</returns>
        [HttpGet("types")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<HashType>), Status200OK)]
        public ActionResult<IEnumerable<HashType>> Types()
        { 
            var list = Enum.GetValues(typeof(HashType)).Cast<HashType>();
            return Ok(list);
        }
        
    }
}
