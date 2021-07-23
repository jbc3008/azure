using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formacion.WebJobs.WebJob2
{
    class Program
    {
        static void Main()
        {
            var config = new JobHostConfiguration();
            if (config.IsDevelopment) config.UseDevelopmentSettings();

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
