using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Formacion.Azure.Functions
{
    /// <summary>
    /// La función se inicia mediante como respuesta a una solicitud HTTP.
    /// Parámetro: [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req
    /// 
    /// El enlace de salida utilizado es Azure Table Storage para escribir documentos en una tabla de una cuenta de Azure Storage.
    /// El constructor del atributo toma el nombre de la tabla y puedes establecer la propiedad Connection para especificar la cuenta de almacenamiento que se usará.
    /// Atributo: [return: Table("operations", Connection = "AzureWebJobsStorage")]
    /// </summary>
    public static class Function2
    {
        [FunctionName("Function2")]
        [return: Table("operations", Connection = "AzureWebJobsStorage")]
        public static async Task<Register> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
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

            return register;
        }
    }
}
