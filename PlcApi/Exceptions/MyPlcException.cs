using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Exceptions
{
    public class MyPlcException :Exception
    {
        public MyPlcException(string message) : base(message)
        {
        }
    }
}
