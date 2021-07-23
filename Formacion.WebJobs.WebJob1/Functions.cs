using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Formacion.WebJobs.WebJob1
{
    public class Functions
    {
        public static void Func1([TimerTrigger("*/30 * * * * *")] TimerInfo timer, TextWriter log)
        {
            log.WriteLine($">> PROCESO INICIADO {DateTime.Now}");
            try
            {
                var ficheros = Directory.GetFiles(@"D:\home\site\wwwroot\wwwroot\upload");
                log.WriteLine($">>>> {ficheros.Length} encontrados.");
                foreach (var ruta in ficheros)
                {
                    var fichero = new FileInfo(ruta);
                    log.WriteLine($">>>>>> {fichero.FullName}");
                    File.Move(fichero.FullName, @"D:\home\site\wwwroot\wwwroot\process\" + fichero.Name);
                    log.WriteLine($">>>>>> P R O C E S A D O");
                }
            }
            catch (Exception ex)
            {
                log.WriteLine($">> ERROR: {ex.Message}");
            }

            log.WriteLine($">> PROCESO FINALIZADO {DateTime.Now}");
        }
        public static void Func2([BlobTrigger("upload/{name}")] TextReader uploadBlob, [Blob("process/{name}")] out string processBlob, string name, TextWriter log)
        {
            log.WriteLine($">> PROCESO INICIADO {DateTime.Now}");
            log.WriteLine($">>>> {name}");
            processBlob = uploadBlob.ReadToEnd();
            log.WriteLine($">> PROCESO FINALIZADO {DateTime.Now}");
        }        
    }
}
