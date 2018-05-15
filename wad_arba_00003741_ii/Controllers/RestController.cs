using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace wad_arba_00003741_ii.Controllers
{ 
    [Authorize]
    public class RestController : ApiController
    {
        [HttpGet]
        public List<Log> GetLogs()
        {
            List<Log> logs = new LogRepo().GetAll().ToList();
            logs.Reverse();
            return logs;
        }
    }
}
