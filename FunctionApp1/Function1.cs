using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp1.Services;
using FunctionApp1.Models;

public class SendEmailFunction
{
    private readonly IEmailService _emailService;

    public SendEmailFunction(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [FunctionName("SendEmailFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Processing email request...");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        EmailMessage emailMessage = JsonConvert.DeserializeObject<EmailMessage>(requestBody);

        if (emailMessage == null || emailMessage.To.Count == 0)
        {
            return new BadRequestObjectResult("Invalid email request.");
        }

        try
        {
            await _emailService.SendEmailAsync(emailMessage);
            return new OkObjectResult("Email sent successfully!");
        }
        catch (System.Exception ex)
        {
            log.LogError($"Error sending email: {ex.Message}");
            return new ObjectResult(new { error = "Failed to send email", details = ex.Message })
            {
                StatusCode = 500
            };
        }
    }
}

/*
 ejemplo de peticion:
http://localhost:7270/api/SendEmailFunction


{
    "from": "rafiki121@hotmail.com",
    "To": ["r.leontorres1988@gmail.com"],
    "Cc": ["rafiki6688@gmail.com"],
    "Bcc": ["rafiki121@hotmail.com"],
    "Subject": "Correo de Prueba",
    "body": "<!DOCTYPE html><html><head><style>body { font-family: Arial, sans-serif; } .container { width: 100%; max-width: 600px; margin: auto; } .header { background: #007bff; color: white; padding: 10px; text-align: center; } .content { padding: 20px; } .footer { background: #f1f1f1; padding: 10px; text-align: center; }</style></head><body><div class='container'><div class='header'><h2>Correo de Prueba</h2></div><div class='content'><p>Hola <b><p>Este es un correo de prueba enviado con Azure Functions y SendGrid.</p></div><div class='footer'><p>&copy; 2025 - Azure Functions</p></div></div></body></html>"
}
 
 
 */
