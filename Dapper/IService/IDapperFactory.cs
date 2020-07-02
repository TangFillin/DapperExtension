using DapperLib.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperLib.IService
{
    public interface IDapperFactory
    {
        DapperClient CreateClient(string name);
    }
}
