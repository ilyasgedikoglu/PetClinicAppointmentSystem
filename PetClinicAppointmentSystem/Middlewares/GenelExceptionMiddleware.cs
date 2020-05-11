using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PetClinicAppointmentSystem.Shared.DTO;
using PetClinicAppointmentSystem.Shared.Enumerations;
using PetClinicAppointmentSystem.Shared.Exception;

namespace PetClinicAppointmentSystem.Middlewares
{
    public class GenelExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly JsonSerializerSettings _jsonSettings;

        public GenelExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            _jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task Invoke(HttpContext context)
        {
            var sonuc = new ResultDTO();
            try
            {
                await _next(context);
            }
            catch (PetClinicAppointmentBadRequestException ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("exception", "BadRequest");
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.BadRequest,
                    Description = ex.Message,
                    Status = EDurum.ERROR
                });
                sonuc.Status = EDurum.ERROR;
                var json = JsonConvert.SerializeObject(sonuc, _jsonSettings);
                await context.Response.WriteAsync(json);
            }
            catch (PetClinicAppointmentNotFoundException ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("exception", "NotFound");
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.NotFound,
                    Description = ex.Message,
                    Status = EDurum.ERROR
                });
                sonuc.Status = EDurum.ERROR;
                var json = JsonConvert.SerializeObject(sonuc, _jsonSettings);
                await context.Response.WriteAsync(json);
            }
            catch (PetClinicAppointmentNotImplementedException ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("exception", "NotImplemented");
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.NotImplemented,
                    Description = ex.Message,
                    Status = EDurum.ERROR
                });
                sonuc.Status = EDurum.ERROR;
                var json = JsonConvert.SerializeObject(sonuc, _jsonSettings);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {

                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("exception", "InternalServerError");
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.InternalServerError,
                    Description = ex.Message,
                    Status = EDurum.ERROR
                });
                sonuc.Status = EDurum.ERROR;
                var json = JsonConvert.SerializeObject(sonuc, _jsonSettings);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
