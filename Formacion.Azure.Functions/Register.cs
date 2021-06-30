using System;
using System.Collections.Generic;
using System.Text;

namespace Formacion.Azure.Functions
{
    public class Register
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
