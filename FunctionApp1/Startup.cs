using FunctionApp1.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]

namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Registrar la implementación del servicio de correo
            builder.Services.AddSingleton<IEmailService, SmtpEmailService>();
        }
    }
}

