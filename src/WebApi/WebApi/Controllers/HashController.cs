using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using LaserPointer.WebApi.Application.Features.HashAlgorithms.Queries.GetHashAlgorithmsQuery;
using LaserPointer.WebApi.Domain.Entities;
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
        [ProducesResponseType(typeof(IEnumerable<HashAlgoType>), Status200OK)]
        public ActionResult<IEnumerable<HashAlgoType>> Types()
        { 
            var list = Enum.GetValues(typeof(HashAlgoType)).Cast<HashAlgoType>();
            return Ok(list);
        }
        
        /// <summary>
        /// Get all the supported hash types with details.
        /// </summary>
        /// <returns>List with all the supported hash algorithms.</returns>
        [HttpGet("algos")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<HashAlgo>), Status200OK)]
        public async Task<IList<HashAlgo>> Algos()
        {
            return await Mediator.Send(new GetHashAlgorithmsQuery());
        }
        
    }
}
