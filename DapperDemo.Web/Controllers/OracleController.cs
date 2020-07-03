using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDemo.Web.Entity;
using DapperLib.IService;
using DapperLib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OracleController : ControllerBase
    {
        private readonly DapperClient _client;
        public OracleController(IDapperFactory dapperFactory)
        {
            _client = dapperFactory.CreateClient("OracleDB");
        }
        #region Query
        [HttpGet("GetAllUser")]
        public List<dynamic> GetAllUser()
        {
            dynamic list = _client.SQuery<dynamic>(@"select * from testuser");
            return list;
        }
        /// <summary>
        /// 根据用户名查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost("GetByUserName")]
        public dynamic GetByUserName(string userName)
        {
            string sql = @"select * from testuser where username=@username";
            dynamic user = _client.SQuery<dynamic>(sql, new { username = userName });
            return user;
        }
        [HttpPost("GetByID")]
        public async Task<TestUser> GetByID(int id)
        {
            return await _client.EGetByIdAsync<TestUser>(id);
        }
        #endregion
        #region Add
        [HttpPost("AddSingleUser")]
        public async Task<int> AddSingleUser(TestUser user)
        {
            //return await _client.EAddReturnId(user);
            return await _client.SExecuteAsync(@"insert into fillin.testuser(username) values('@username')",user);
        }


        #endregion

    }

}
