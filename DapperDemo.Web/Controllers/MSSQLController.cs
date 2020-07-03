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
    public class MSSQLController : ControllerBase
    {
        private readonly DapperClient _client;
        public MSSQLController(IDapperFactory dapperFactory)
        {
            _client = dapperFactory.CreateClient("MSSqlDB");
        }
        
        [HttpGet("GetAllPerson")]
        public List<dynamic> GetAllPerson()
        {
            dynamic list = _client.SQuery<dynamic>(@"select * from Person");
            return list;
        }
        [HttpPost("GetByID")]
        public dynamic GetByID(int id)
        {
            dynamic item = _client.SQuery<dynamic>("select * from Person where ID=@id", new { id = id });
            return item;
        }
        [HttpPost("GetByDiscrimitor")]
        public List<dynamic> GetByDiscrimitor(string discrimitor)
        {
            dynamic list = _client.SQuery<dynamic>("select * from Person where Discriminator=@discriminator", new { discriminator = discrimitor });
            return list;
        }
        
    }
}
