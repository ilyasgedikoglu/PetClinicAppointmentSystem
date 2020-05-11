using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.CriteriaObject;
using PetClinicAppointmentSystem.Shared.DTO;
using PetClinicAppointmentSystem.Shared.Enumerations;
using PetClinicAppointmentSystem.Shared.Exception;

namespace PetClinicAppointmentSystem.Controllers
{
    [Route("api/")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGirisService _girisService;
        private readonly IYetkiService _yetkiService;
        private IConfiguration Configuration { get; }

        public LoginController(IUserService userService, IGirisService girisService, IConfiguration configuration, IYetkiService yetkiService)
        {
            Configuration = configuration;
            _userService = userService;
            _yetkiService = yetkiService;
            _girisService = girisService;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Giris([FromForm] LoginCO request)
        {
            var sonuc = new ResultDTO();
            if (request == null)
            {
                throw new PetClinicAppointmentBadRequestException("Oturum açmak için kullanıcı adı ve şifresini giriniz!");
            }

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new PetClinicAppointmentBadRequestException("Eksik parametre girdiniz!");
            }

            if (request.Email.Length > 64 || request.Password.Length > 64)
            {
                throw new PetClinicAppointmentBadRequestException("Kullanıcı adı veya şifresi çok uzun!");
            }

            UserDTO user = null;

            user = _userService.GetByKullaniciAdiAndSifre(request.Email, request.Password);

            if (user == null)
            {
                throw new PetClinicAppointmentUnauthorizedException("Email or password incorrect!");
            }

            user.TuzlamaDegeri = "";
            user.Password = "";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim("KullaniciGuid", user.Guid.ToString())
            };
            // Giriş yapan kullanıcının token kaydı veritabanına ekleyelim
            var token = new JwtSecurityToken
            (
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Configuration[
                                "Jwt:SigningKey"])),
                    SecurityAlgorithms.HmacSha256)
            );
            var tokenn = new JwtSecurityTokenHandler().WriteToken(token);
           
            var giris = new GirisDTO()
            {
                KullaniciId = user.Id,
                Token = tokenn,
                Durum = true,
                Actived = true,
                Deleted = false
            };
            var girisId = _girisService.Create(giris);
            if (girisId > 0)
            {
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.OK,
                    Status = EDurum.SUCCESS,
                    Description = "Login Success"
                });
                sonuc.Status = sonuc.Message.OrderBy(x => x.Code).FirstOrDefault().Status;
                sonuc.Data = new
                {
                    token = tokenn,
                    hesap = new
                    {
                        user.Guid,
                        user.Email,
                        user.Name,
                        user.Surname,
                        yetki = user.Yetki != null ? new { user.Yetki.Guid, user.Yetki.Name, user.Yetki.Description } : null,
                    }
                };
            }
            return Ok(sonuc);
        }
    }
}