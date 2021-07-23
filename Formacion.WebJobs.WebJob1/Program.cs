using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Formacion.WebJobs.WebJob1
{    
    class Program
    {
        static void Main()
        {            
            var config = new JobHostConfiguration();
            if (config.IsDevelopment) config.UseDevelopmentSettings();

            config.UseTimers();

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
