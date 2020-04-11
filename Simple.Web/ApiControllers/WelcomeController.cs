using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Simple.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Welcome : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "success";
        }
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"success{id}";
        }
    }
}
