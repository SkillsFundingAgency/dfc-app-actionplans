using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DFC.App.ActionPlans.Services
{
    public class ErrorService
    {
        public static async Task LogException(HttpContext context, ILogger logger)
        {
            var exception =
                context.Features.Get<IExceptionHandlerPathFeature>();

            logger.Log(LogLevel.Error, $"Accounts Error: {exception.Error.Message} \r\n" +
                                       $"Path: {exception.Path} \r\n)");
        }
    }
}
