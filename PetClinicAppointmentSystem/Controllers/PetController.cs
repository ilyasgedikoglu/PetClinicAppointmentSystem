using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IUserService _userService;

        public PetController(IPetService petService, IUserService userService)
        {
            _userService = userService;
            _petService = petService;
        }

        [HttpGet]
        [Route("{petGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN, (int)Yetkiler.USER } })]
        public ActionResult GetPet(Guid petGuid)
        {
            var sonuc = new ResultDTO();

            var pet = _petService.GetByGuid(petGuid);
            if (pet == null)
            { 
                throw new PetClinicAppointmentNotFoundException("Pet not found!");
            }
         
            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "Pet information has been successfully sent."
            });
            sonuc.Data = new { pet = new
            {
                pet.Guid, 
                pet.Name,
                Birthdate = pet.DogumTarihi,
                PlaceOfBirth = pet.DogumYeri,
                Owner = new {
                    pet.User.Guid,
                    pet.User.Name,
                    pet.User.Surname,
                    pet.User.Email
                }
            } };

            return Ok(sonuc);
        }

        [HttpGet]
        [Route("GetAllPets")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult GetPets()
        {
            var sonuc = new ResultDTO();

            var pets = _petService.GetAllPets();

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "Pets information has been successfully sent."
            });
            sonuc.Data = new
            {
                pets = pets.Select(pet => new
                {
                    pet.Guid,
                    pet.Name,
                    Birthdate = pet.DogumTarihi,
                    PlaceOfBirth = pet.DogumYeri,
                    Owner = new
                    {
                        pet.User.Guid,
                        pet.User.Name,
                        pet.User.Surname,
                        pet.User.Email
                    }
                })
            };

            return Ok(sonuc);
        }

        [HttpPost]
        [Route("")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] {new int[] {(int) Yetkiler.USER}})]
        public ActionResult PetCreate([FromForm] PetCO request)
        {
            var sonuc = new ResultDTO();

            if (request == null)
            {
                throw new PetClinicAppointmentBadRequestException("You have not sent any data!");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new PetClinicAppointmentBadRequestException("Pet name cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.PlaceOfBirth))
            {
                throw new PetClinicAppointmentBadRequestException("Pet place of birth cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.Birthdate.ToLongDateString()))
            {
                throw new PetClinicAppointmentBadRequestException("Pet birthdate cannot be empty!");
            }

            if (request.UserGuid == null)
            {
                throw new PetClinicAppointmentBadRequestException("User guid cannot be empty!");
            }

            var user = _userService.GetByGuid(request.UserGuid);
            if (user == null)
            {
                throw new PetClinicAppointmentNotFoundException("User not found!");
            }

            var picturePath = Path.Combine("Assets", "defaultPet.jpeg");

            var dto = new PetDTO()
            {
                Guid = Guid.NewGuid(),
                Deleted = false,
                Actived = true,
                CreatedDate = DateTime.Now,
                DogumTarihi = request.Birthdate,
                DogumYeri = request.PlaceOfBirth,
                Name = request.Name,
                User = null,
                UserId = user.Id,
                Resim = picturePath
            };
      
            var durum = _petService.Create(dto);
            if (durum > 0)
            {
                sonuc.Status = EDurum.SUCCESS;
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.OK,
                    Status = EDurum.SUCCESS,
                    Description = "Pet was created successfully."
                });
                sonuc.Data = new { pet = new { dto.Guid } };
            }
            else
            {
                throw new PetClinicAppointmentBadRequestException("Error adding pet!");
            }

            return Ok(sonuc);
        }

        [HttpPut]
        [Route("{petGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER } })]
        public ActionResult PetUpdate(Guid petGuid, [FromForm] PetCO request)
        {
            var sonuc = new ResultDTO();

            if (request == null)
            {
                throw new PetClinicAppointmentBadRequestException("You have not sent any data!");
            }

            if (petGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Submit valid pet information!");
            }

            var pet = _petService.GetByGuid(petGuid);
            if (pet == null)
            {
                throw new PetClinicAppointmentNotFoundException("Pet not found!");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new PetClinicAppointmentBadRequestException("Pet name cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.PlaceOfBirth))
            {
                throw new PetClinicAppointmentBadRequestException("Pet place of birth cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.Birthdate.ToLongDateString()))
            {
                throw new PetClinicAppointmentBadRequestException("Pet birthdate cannot be empty!");
            }

            if (request.UserGuid == null)
            {
                throw new PetClinicAppointmentBadRequestException("User guid cannot be empty!");
            }

            var user = _userService.GetByGuid(request.UserGuid);
            if (user == null)
            {
                throw new PetClinicAppointmentNotFoundException("User not found!");
            }

            pet.UserId = user.Id;
            pet.DogumTarihi = request.Birthdate;
            pet.DogumYeri = request.PlaceOfBirth;
            pet.Name = request.Name;
            pet.User = null;

            var durum = _petService.Update(pet);
            if (durum > 0)
            {
                sonuc.Status = EDurum.SUCCESS;
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.OK,
                    Status = EDurum.SUCCESS,
                    Description = "The pet has been successfully updated."
                });
                sonuc.Data = new { pet = new { pet.Guid } };
            }
            else
            {
                throw new PetClinicAppointmentBadRequestException("The pet could not be updated!");
            }

            return Ok(sonuc);
        }

        [HttpDelete("{petGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER, (int)Yetkiler.ADMIN } })]
        public ActionResult PetDelete(Guid petGuid)
        {
            var pet = _petService.GetByGuid(petGuid);
            if (pet == null)
            {
                throw new PetClinicAppointmentNotFoundException("The pet you want to delete was not found!");
            }

            var sonuc = new ResultDTO();

            _petService.Delete(pet.Id);

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "The pet has been successfully deleted."
            });

            return Ok(sonuc);
        }
    }
}