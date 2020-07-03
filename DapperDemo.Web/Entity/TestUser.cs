using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo.Web.Entity
{
    [Table("TESTUSER")]
    public class TestUser
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        //public string Password { get; set; }
        //public  string UserType { get; set; }

    }
}
