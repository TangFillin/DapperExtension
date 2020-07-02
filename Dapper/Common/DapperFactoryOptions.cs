using System;
using System.Collections.Generic;
using System.Text;

namespace DapperLib.Common
{
    public class DapperFactoryOptions
    {
        public IList<Action<ConnectionConfig>> DapperActions { get; } = new List<Action<ConnectionConfig>>();
    }
}
