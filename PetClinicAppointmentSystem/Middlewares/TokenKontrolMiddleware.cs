using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.DTO;
using PetClinicAppointmentSystem.Shared.Enumerations;

namespace PetClinicAppointmentSystem.Middlewares
{
    public class TokenKontrolMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerSettings _jsonSettings;
        public TokenKontrolMiddleware(RequestDelegate next)
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
        
        public async Task Invoke(HttpContext context, IGirisService girisService)
        {
            try
            {
                var sonuc = new ResultDTO();
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                    var expDate = tokenS.ValidTo;
                    if (expDate < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        context.Response.Headers.Add("exception", "Unauthorized");
                        sonuc.Message.Add(new MessageDTO()
                        {
                            Code = HttpStatusCode.Unauthorized,
                            Description = "Your session has expired! Please sign in again..",
                            Status = EDurum.ERROR
                        });
                        sonuc.Status = EDurum.ERROR;
                        var json = JsonConvert.SerializeObject(sonuc, _jsonSettings);
                        await context.Response.WriteAsync(json);
                    }
                    else
                    {
                        var kullaniciId = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "unique_name").Value);
                        var girisDto = girisService.KullanicininSonTokenBilgisi(kullaniciId);

                        if (girisDto != null && girisDto.Token == token)
                        {
                            await _next.Invoke(context);

                        }
                        else
                        {
                            context.Response.StatusCode = 200;
                            context.Response.ContentType = "application/json";
                            context.Response.Headers.Add("exception", "Unauthorized");
                            sonuc.Message.Add(new MessageDTO()
                            {
                                Code = HttpStatusCode.Unauthorized,
                                Description = "Your transaction has been terminated because another session has been opened for you!",
                                Status = EDurum.ERROR
                            });
                            sonuc.Status = EDurum.ERROR;
                            var json = JsonConvert.SerializeObject(sonuc, _jsonSettings);
                            await context.Response.WriteAsync(json);
                        }

                    }
                }
                else
                {
                    await _next.Invoke(context);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
