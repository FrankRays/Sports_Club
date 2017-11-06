using Microsoft.AspNetCore.Mvc;
using Sport.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    public class DummyController : Controller
    {
        private SportContext _ctx;

        public DummyController(SportContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
