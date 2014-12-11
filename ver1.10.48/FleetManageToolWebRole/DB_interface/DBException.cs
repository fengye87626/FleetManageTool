using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.DB_interface
{
    public class DBException : System.Exception
    {
        public DBException(string message) : base(message) { }
    }
}