using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetClinicAppointmentSystem.Filters;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.CriteriaObject;
using PetClinicAppointmentSystem.Shared.DTO;
using PetClinicAppointmentSystem.Shared.Enumerations;
using PetClinicAppointmentSystem.Shared.Exception;

namespace PetClinicAppointmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IYetkiService _yetkiService;

        public UserController(IUserService userService, IYetkiService yetkiService)
        {
            _userService = userService;
            _yetkiService = yetkiService;
        }

        [HttpPut]
        [Route("{userGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER } })]
        public ActionResult UserUpdate(Guid userGuid, [FromForm] UserCO request, [FromForm(Name = "file")] IFormFile file)
        {
            if (request == null)
            {
                throw new PetClinicAppointmentBadRequestException("You have not sent any data!");
            }

            if (userGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Submit valid user information!");
            }

            var kullanici = _userService.GetByGuid(userGuid);
            if (kullanici == null)
            {
                throw new PetClinicAppointmentNotFoundException("User not found!");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new PetClinicAppointmentBadRequestException("Name is not null!");
            }

            if (string.IsNullOrEmpty(request.Surname))
            {
                throw new PetClinicAppointmentBadRequestException("Surname is not null!");
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                throw new PetClinicAppointmentBadRequestException("Email is not null!");
            }

            var getKullanici = _userService.GetByKullaniciAdi(request.Email);
        

            if (getKullanici != null && kullanici.Email != getKullanici.Email)
            {
                throw new PetClinicAppointmentBadRequestException("Such a email already exists!");
            }

            var sonuc = new ResultDTO();

            kullanici.Name = request.Name;
            kullanici.Surname = request.Surname;
            kullanici.Email = request.Email;
            kullanici.DogumTarihi = request.DogumTarihi;
            kullanici.KimlikNo = request.KimlikNo;
            kullanici.DogumYeri = request.DogumYeri;
            if (!string.IsNullOrEmpty(request.Password))
            {
                kullanici.TuzlamaDegeri = _userService.GetTuzlamaDegeri();

                kullanici.Password = _userService.Sifrele(request.Password, kullanici.TuzlamaDegeri);
            }
            if (file != null)
            {
                var uzantiAdi = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(uzantiAdi))
                {
                    throw new PetClinicAppointmentBadRequestException("Uzantısı olmayan dosya yüklenemez!");
                }

                var klasorAdi = Guid.NewGuid().ToString();
                var yeniDosyaAdi = Guid.NewGuid() + uzantiAdi;

                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                var path = Path.Combine(webRoot, klasorAdi);

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var filePath = Path.Combine("Uploads", klasorAdi, yeniDosyaAdi);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                kullanici.Resim = filePath;
            }

            var durum = _userService.Update(kullanici);

            if (durum < 1)
            {
                throw new PetClinicAppointmentBadRequestException("The user could not be updated!");
            }

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "The user has been successfully updated."
            });
            sonuc.Data = new { user = new { kullanici.Guid } };
            return Ok(sonuc);
        }

        [HttpDelete("{userGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER, (int)Yetkiler.ADMIN } })]
        public ActionResult UserDelete(Guid userGuid)
        {
            var kullanici = _userService.GetByGuid(userGuid);
            if (kullanici == null)
            {
                throw new PetClinicAppointmentNotFoundException("The user you want to delete was not found!");
            }

            var sonuc = new ResultDTO();

            _userService.Delete(kullanici.Id);

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "The user has been successfully deleted."
            });

            return Ok(sonuc);
        }

        [HttpGet]
        [Route("GetUserPicture/{userGuid:GUID}")]
        public FileStreamResult GetDosyaBilgileriByGuid(Guid userGuid)
        {
            if (userGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Submit a valid user guid!");
            }

            var userVarMi = _userService.GetByGuid(userGuid);
            if (userVarMi == null)
            {
                throw new PetClinicAppointmentNotFoundException("User not found!");
            }

            if (string.IsNullOrEmpty(userVarMi.Resim))
            {
                throw new PetClinicAppointmentNotFoundException("Picture path not found!");
            }

            FileStream dosya = System.IO.File.OpenRead(userVarMi.Resim); //Dokümanı okuma

            var dokumanUzantiAciklama = "image/jpeg";

            return new FileStreamResult(dosya, dokumanUzantiAciklama);
        }
    }
}