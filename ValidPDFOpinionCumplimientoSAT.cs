using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using POC_REPSE.Utils;
using Org.BouncyCastle.Asn1.Ocsp;

namespace POC_REPSE
{
    public static class ValidPDFOpinionCumplimientoSAT
    {
        [FunctionName("ValidPDFOpinionCumplimientoSAT")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = "v1/ValidPDFOpinionCumplimientoSAT")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            RequestPDF requestPDF = new RequestPDF();
            try
            {
                var requestBody = new StreamReader(req.Body).ReadToEnd();
                requestPDF = JsonConvert.DeserializeObject<RequestPDF>(requestBody);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error al intentar obtener la información", null);
                return new BadRequestResult();
            }
            ValidPDFOpinionCumplimientoSATV1 validPDFOpinionCumplimientoSATV1 = new ValidPDFOpinionCumplimientoSATV1();
            var resultado = validPDFOpinionCumplimientoSATV1.Validar(requestPDF);

            return new OkObjectResult(resultado);
        }
    }
}
