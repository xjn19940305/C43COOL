using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Global
{
    public class InternationalException : Exception
    {
        public string Code { get; set; }
        public InternationalException(string message) : base(message)
        {
        }
    }
}