using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Modulo
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string x_str = req.Query["x"];
            double x;
            x = Convert.ToDouble(x_str);

            string y_str = req.Query["y"];
            double y;
            y = Convert.ToDouble(y_str);

            double ans = x % y;

            // TEST        
            string ans_str = Convert.ToString(ans);
            string str_calc = x_str + "%" + y_str + "=" + ans_str;
            string output_json_str = "{" + "\"error\":false,\"string\":\"" + str_calc + "\",\"answer\":" + ans + "}";
            // {"error":false,"string":"3%2=1","answer":1}

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            x_str = x_str ?? data?.x_str;

            string responseMessage = string.IsNullOrEmpty(x_str)
                ? " HTTP successfully triggered. Please pass x and y values, e.g.: <URL>?x=2&y=5"
                : $"{output_json_str}";

            return new OkObjectResult(responseMessage);
        }
    }
}
