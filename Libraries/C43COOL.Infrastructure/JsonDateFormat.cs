using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Infrastructure
{
    public class JsonDateFormat : IsoDateTimeConverter
    {
        public JsonDateFormat(string format)
        {
            DateTimeFormat = format;
        }
    }
}
