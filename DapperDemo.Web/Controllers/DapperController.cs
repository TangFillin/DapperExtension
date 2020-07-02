using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperLib.IService;
using DapperLib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DapperController : ControllerBase
    {
        private readonly DapperClient _client;
        public DapperController(IDapperFactory dapperFactory)
        {
            _client = dapperFactory.CreateClient("OracleDB");
        }
        [HttpGet]
        public List<dynamic> GetAll()
        {
            dynamic list = _client.Query<dynamic>(@"select * from testUser");
            return list;
        }
    }
}
