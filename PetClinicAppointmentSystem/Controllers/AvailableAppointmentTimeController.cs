using System;
using System.Collections.Generic;
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
    public class AvailableAppointmentTimeController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IUserService _userService;
        private readonly IAppointmentService _appointmentService;
        private readonly IAvailableAppointmentService _availableAppointmentService;

        public AvailableAppointmentTimeController(IPetService petService, IUserService userService, IAppointmentService appointmentService, IAvailableAppointmentService availableAppointmentService)
        {
            _userService = userService;
            _petService = petService;
            _appointmentService = appointmentService;
            _availableAppointmentService = availableAppointmentService;
        }

        [HttpGet]
        [Route("{availableAppointmentTimeGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult GetAvailableAppointmentTime(Guid availableAppointmentTimeGuid)
        {
            var sonuc = new ResultDTO();

            var availableAppointmentTime = _availableAppointmentService.GetByGuid(availableAppointmentTimeGuid);
            if (availableAppointmentTime == null)
            {
                throw new PetClinicAppointmentNotFoundException("Available appointment time not found!");
            }

            if (availableAppointmentTime.AppointmentTime < DateTime.Now)
            {
                _availableAppointmentService.Delete(availableAppointmentTime.Id);

                throw new PetClinicAppointmentNotFoundException("Available appointment time not found!");
            }

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "Available appointment time information has been successfully sent."
            });
            sonuc.Data = new
            {
                availableAppointmentTime = new
                {
                    availableAppointmentTime.Guid,
                    availableAppointmentTime.AppointmentTime,
                    availableAppointmentTime.Time
                }
            };

            return Ok(sonuc);
        }

        [HttpGet]
        [Route("GetAllAvailableAppointmentTimes")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult GetAllAvailableAppointmentTimes()
        {
            var sonuc = new ResultDTO();

            var deletedAvailableAppointmentTimes = _availableAppointmentService.GetAllAvailableAppointmentTimes();

            foreach (var item in deletedAvailableAppointmentTimes)
            {
                if (item.AppointmentTime < DateTime.Now)
                {
                    _availableAppointmentService.Delete(item.Id);
                }
            }

            var availableAppointmentTimes = _availableAppointmentService.GetAllAvailableAppointmentTimes();

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "All available appointment times information has been successfully sent."
            });
            sonuc.Data = new
            {
                availableAppointmentTimes = availableAppointmentTimes.Select(availableAppointmentTime => new
                {
                    availableAppointmentTime.Guid,
                    availableAppointmentTime.AppointmentTime,
                    availableAppointmentTime.Time
                })
            };

            return Ok(sonuc);
        }

        [HttpPost]
        [Route("")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult AvailableAppointmentTimeCreate([FromForm] AvailableAppointmentTimeCO request)
        {
            var sonuc = new ResultDTO();

            if (request == null)
            {
                throw new PetClinicAppointmentBadRequestException("You have not sent any data!");
            }

            if (string.IsNullOrEmpty(request.Time.ToString()))
            {
                throw new PetClinicAppointmentBadRequestException("Time cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.AppointmentTime.ToLongDateString()))
            {
                throw new PetClinicAppointmentBadRequestException("Pet appointment time cannot be empty!");
            }

            var dto = new AvailableAppointmentTimeDTO()
            {
                Guid = Guid.NewGuid(),
                Deleted = false,
                Actived = true,
                CreatedDate = DateTime.Now,
                AppointmentTime = request.AppointmentTime,
                Time = request.Time
            };

            var durum = _availableAppointmentService.Create(dto);
            if (durum > 0)
            {
                sonuc.Status = EDurum.SUCCESS;
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.OK,
                    Status = EDurum.SUCCESS,
                    Description = "Available appointment time was created successfully."
                });
                sonuc.Data = new
                {
                    availableAppointmentTime = new
                    {
                        dto.Guid,
                        dto.AppointmentTime,
                        dto.Time
                    }
                };
            }
            else
            {
                throw new PetClinicAppointmentBadRequestException("Couldn't create an available appointment time!");
            }

            return Ok(sonuc);
        }

        [HttpPut]
        [Route("{availableAppointmentTimeGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult AvailableAppointmentTimeUpdate(Guid availableAppointmentTimeGuid, [FromForm] AvailableAppointmentTimeCO request)
        {
            var sonuc = new ResultDTO();

            if (request == null)
            {
                throw new PetClinicAppointmentBadRequestException("You have not sent any data!");
            }

            if (availableAppointmentTimeGuid == default(Guid) || availableAppointmentTimeGuid == null)
            {
                throw new PetClinicAppointmentBadRequestException("Submit valid available appointment time information!");
            }

            var availableAppointmentTime = _availableAppointmentService.GetByGuid(availableAppointmentTimeGuid);
            if (availableAppointmentTime == null)
            {
                throw new PetClinicAppointmentNotFoundException("Available appointment time not found!");
            }

            if (string.IsNullOrEmpty(request.Time.ToString()))
            {
                throw new PetClinicAppointmentBadRequestException("Time cannot be empty!");
            }

            if (string.IsNullOrEmpty(request.AppointmentTime.ToLongDateString()))
            {
                throw new PetClinicAppointmentBadRequestException("Pet appointment time cannot be empty!");
            }

            availableAppointmentTime.AppointmentTime = request.AppointmentTime;
            availableAppointmentTime.Time = request.Time;

            var durum = _availableAppointmentService.Update(availableAppointmentTime);
            if (durum > 0)
            {
                sonuc.Status = EDurum.SUCCESS;
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.OK,
                    Status = EDurum.SUCCESS,
                    Description = "The available appointment time has been successfully updated."
                });
                sonuc.Data = new { availableAppointmentTime = new { availableAppointmentTime.Guid } };
            }
            else
            {
                throw new PetClinicAppointmentBadRequestException("The available appointment time could not be updated!");
            }

            return Ok(sonuc);
        }

        [HttpDelete("{availableAppointmentTimeGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult AvailableAppointmentTimeDelete(Guid availableAppointmentTimeGuid)
        {
            var availableAppointmentTime = _availableAppointmentService.GetByGuid(availableAppointmentTimeGuid);
            if (availableAppointmentTime == null)
            {
                throw new PetClinicAppointmentNotFoundException("The available appointment time you want to delete was not found!");
            }

            var sonuc = new ResultDTO();

            _availableAppointmentService.Delete(availableAppointmentTime.Id);

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "The available appointment time has been successfully deleted."
            });

            return Ok(sonuc);
        }
    }
}