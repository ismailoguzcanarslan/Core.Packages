using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions.Extentions;

public static class ExceptionMiddlewareExtentions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleWare>();
}
