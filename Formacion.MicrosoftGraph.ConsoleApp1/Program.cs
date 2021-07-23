using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Formacion.MicrosoftGraph.ConsoleApp1
{
    class Program
    {
        static IConfidentialClientApplication app;
        static void Main(string[] args)
        {
            Console.Clear();

            app = ConfidentialClientApplicationBuilder
                .Create("<Application ID>")
                .WithClientSecret("<Application Secret>")
                .WithAuthority("https://login.microsoftonline.com/<Tenant ID>")
                .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider(app);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

            AuthenticationResult token = app.AcquireTokenForClient(scopes).ExecuteAsync().Result;

            //Muestra por pantalla el Token generado
            //Console.WriteLine(token.AccessToken);

            var http = new HttpClient();
            string url = "https://graph.microsoft.com/v1.0/users/";

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            string response = http.GetStringAsync(url).Result;

            //Muestra por pantalla la respuesta en formato JSON
            //Console.WriteLine(response);

            OData data = JsonConvert.DeserializeObject<OData>(response);
            List<User> usuarios = JsonConvert.DeserializeObject<List<User>>(data.Value.ToString());
            foreach (var item in usuarios) Console.WriteLine($"Usuario: {item.DisplayName}");
            Console.WriteLine(Environment.NewLine);

            Console.ReadKey();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            //Obtener los perfiles de todos los usuarios del Azure AD
            var Result = graphClient.Users.Request().GetAsync().Result;

            foreach (var item in Result) Console.WriteLine($"Usuario: {item.DisplayName}");
            Console.WriteLine(Environment.NewLine);


            //Obtener el perfil de un usuario del Azure AD
            var Usuario = graphClient.Users["Chris@azuredemos.site"].Request().GetAsync().Result;

            Console.WriteLine($"Usuario: {Usuario.DisplayName}");

            //Modificar datos del perfil del usuario
            Usuario.City = "Madrid";
            Usuario.Country = "Spain";

            var UsuarioUpdate = graphClient.Users["Chris@azuredemos.site"].Request().UpdateAsync(Usuario).Result;
            Console.WriteLine($"Modificado el perfil de {UsuarioUpdate.DisplayName}");

        }
    }

    public class OData
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public Object Value { get; set; }
    }
}
