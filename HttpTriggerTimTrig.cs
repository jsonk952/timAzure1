using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class HttpTriggerTimTrig
    {
        [FunctionName("HttpTriggerTimTrig")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://timfuncappbd87.file.core.windows.net/test1/azure_test_1.txt?sp=rl&st=2019-07-21T09:01:17Z&se=2019-07-22T09:01:17Z&sv=2018-03-28&sig=OphHu9ghDvJ9sSyiOmqqH1RFWGOXJpV%2Fs0D9WvQTNCE%3D&sr=f");
            WebResponse resp = myReq.GetResponse();
            string body = "unk";
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                body = reader.ReadToEnd(); // do something fun...
            }
            
            return name != null
                ? (ActionResult)new OkObjectResult($"Hello dude, {name} {body}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
