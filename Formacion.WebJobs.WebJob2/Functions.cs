using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formacion.WebJobs.WebJob2
{
    public class Functions
    {
        public static void Func1([BlobTrigger("upload/{name}")] Stream uploadBlob, [Blob("process/{name}", FileAccess.Write)] Stream processBlob, string name, TextWriter log)
        {
            log.WriteLine($">> PROCESO INICIADO {DateTime.Now}");
            log.WriteLine($">>>> Blob: {name}");
            log.WriteLine($">>>> P R O C E S A D O");
            uploadBlob.CopyTo(processBlob);
            log.WriteLine($">> PROCESO FINALIZADO {DateTime.Now}");
        }
    }
}
