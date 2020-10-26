using System;
using System.Collections.Generic;
using System.Linq;
using LaserPointer.WebApi.Domain.Enums;
using LaserPointer.WebApi.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaserPointer.WebApi.WebApi.Controllers
{
    
    public class HashesController : ApiController
    {
        [HttpGet("types")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<HashType>> Types()
        { 
            var list = Enum.GetValues(typeof(HashType)).Cast<HashType>();
            return Ok(list);
        }
        
    }
}
