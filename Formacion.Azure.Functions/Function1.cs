using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Formacion.Azure.Functions
{
    public static class Function1
    {
        /// <summary>
        /// La función se inicia mediante como respuesta a una solicitud HTTP.
        /// El enlace de salida predeterminado es HTTP para responder al remitente de la solicitud HTTP. 
        /// Este enlace requiere un desencadenador HTTP y le permite personalizar la respuesta asociada con la solicitud del desencadenador.
        /// Parámetro: [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req
        /// 
        /// También se utiliza una salida Azure Table Storage para escribir documentos en una tabla de una cuenta de Azure Storage.
        /// El constructor del atributo del parámetro outputTable toma el nombre de la tabla y puedes establecer la propiedad Connection para especificar la cuenta de almacenamiento que se usará.
        /// Parámetro: [Table("operations", Connection = "AzureWebJobsStorage")] ICollector<Register> outputTable
        /// </summary>
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, 
            ILogger log, 
            [Table("operations", Connection = "AzureWebJobsStorage")] ICollector<Register> outputTable)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var register = new Register();
            register.PartitionKey = "http";
            register.RowKey = Guid.NewGuid().ToString();
            register.Name = name;
            register.Message = responseMessage;

            outputTable.Add(register);

            return new OkObjectResult(responseMessage);
        }
    }
}
