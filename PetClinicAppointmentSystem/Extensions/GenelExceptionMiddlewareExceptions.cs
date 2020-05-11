using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using PetClinicAppointmentSystem.Middlewares;

namespace PetClinicAppointmentSystem.Extensions
{
    public static class GenelExceptionMiddlewareExtensionsExtensions
    {
        public static IApplicationBuilder UseGenelExceptionMiddlewareExtensions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GenelExceptionMiddleware>();
        }
    }
}
