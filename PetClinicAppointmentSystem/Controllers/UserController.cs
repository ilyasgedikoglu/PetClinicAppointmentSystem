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
        private readonly IPetService _petService;

        public UserController(IUserService userService, IPetService petService)
        {
            _userService = userService;
            _petService = petService;
        }

        [HttpGet]
        [Route("{userGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN, (int)Yetkiler.USER } })]
        public ActionResult GetUser(Guid userGuid)
        {
            var sonuc = new ResultDTO();

            var user = _userService.GetByGuid(userGuid);
            if (user == null)
            {
                throw new PetClinicAppointmentNotFoundException("User not found!");
            }

            var pets = _petService.GetAllPets().Where(x => x.UserId == user.Id).ToList();

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "User information has been successfully sent."
            });
            sonuc.Data = new
            {
                user = new
                {
                    user.Guid,
                    user.Name,
                    user.Email,
                    user.KimlikNo,
                    Birthdate = user.DogumTarihi,
                    PlaceOfBirth = user.DogumYeri,
                    role = user.Yetki != null ? new { user.Yetki.Guid, user.Yetki.Name, user.Yetki.Description } : null,
                    Pets = pets.Select(pet => new
                    {
                        pet.Guid,
                        pet.Name,
                        Birthdate = pet.DogumTarihi,
                        PlaceOfBirth = pet.DogumYeri,
                    })
                }
            };

            return Ok(sonuc);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult GetUsers()
        {
            var sonuc = new ResultDTO();

            var users = _userService.GetAllUsers();

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "Users information has been successfully sent."
            });
            sonuc.Data = new
            {
                users = users.Select(user => new
                {
                    user.Guid,
                    user.Name,
                    user.Email,
                    user.KimlikNo,
                    Birthdate = user.DogumTarihi,
                    PlaceOfBirth = user.DogumYeri,
                    role = user.Yetki != null ? new { user.Yetki.Guid, user.Yetki.Name, user.Yetki.Description } : null,
                })
            };

            return Ok(sonuc);
        }

        [HttpPut]
        [Route("{userGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER } })]
        public ActionResult UserUpdate(Guid userGuid, [FromForm] UserCO request)
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

            if (string.IsNullOrEmpty(request.Email))
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
    }
}