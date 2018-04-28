﻿
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace testproj.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [Authorize]
        public IActionResult GetValues()
        {
            var values = new string[] { "abc", "def", "ghi", "jkl" };
            return Ok(values);
        }

        // GET api/values/5
        
        [HttpGet("{id}")]
        
        public IActionResult GetValue(int id)
        {
            // throw new Exception("Test exception");
            var value = "sample";
            return Ok(value);
        }

    }
}
