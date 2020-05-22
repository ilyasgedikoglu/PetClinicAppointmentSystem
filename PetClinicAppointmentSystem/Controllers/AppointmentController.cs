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
    public class AppointmentController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IUserService _userService;
        private readonly IAppointmentService _appointmentService;
        private readonly IAvailableAppointmentService _availableAppointmentService;

        public AppointmentController(IPetService petService, IUserService userService, IAppointmentService appointmentService, IAvailableAppointmentService availableAppointmentService)
        {
            _userService = userService;
            _petService = petService;
            _appointmentService = appointmentService;
            _availableAppointmentService = availableAppointmentService;
        }

        [HttpGet]
        [Route("{appointmentGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN, (int)Yetkiler.USER } })]
        public ActionResult GetAppointment(Guid petGuid)
        {
            var sonuc = new ResultDTO();

            var appointment = _appointmentService.GetByGuid(petGuid);
            if (appointment == null)
            {
                throw new PetClinicAppointmentNotFoundException("Appointment not found!");
            }

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "Appointment information has been successfully sent."
            });
            sonuc.Data = new
            {
                appointment = new
                {
                    appointment.Guid,
                    appointment.AppointmentTime,
                    appointment.CreatedDate,
                    Pet = new
                    {
                        appointment.Pet.Guid,
                        appointment.Pet.Name,
                        Birthdate = appointment.Pet.DogumTarihi,
                        PlaceOfBirth = appointment.Pet.DogumYeri
                    },
                    User = new
                    {
                        appointment.User.Guid,
                        appointment.User.Name,
                        appointment.User.Surname,
                        appointment.User.Email
                    }
                }
            };

            return Ok(sonuc);
        }

        [HttpGet]
        [Route("GetAllAppointments")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.ADMIN } })]
        public ActionResult GetAllAppointments()
        {
            var sonuc = new ResultDTO();

            var appointments = _appointmentService.GetAllAppointments();

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "Appointments information has been successfully sent."
            });
            sonuc.Data = new
            {
                appointments = appointments.Select(appointment => new
                {
                    appointment.Guid,
                    appointment.AppointmentTime,
                    appointment.CreatedDate,
                    Pet = new
                    {
                        appointment.Pet.Guid,
                        appointment.Pet.Name,
                        Birthdate = appointment.Pet.DogumTarihi,
                        PlaceOfBirth = appointment.Pet.DogumYeri
                    },
                    User = new
                    {
                        appointment.User.Guid,
                        appointment.User.Name,
                        appointment.User.Surname,
                        appointment.User.Email
                    }
                })
            };

            return Ok(sonuc);
        }

        [HttpGet]
        [Route("GetUserAppointments")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER, (int)Yetkiler.ADMIN } })]
        public ActionResult GetUserAppointments(Guid userGuid)
        {
            var sonuc = new ResultDTO();

            if (userGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Submit valid user information!");
            }

            var user = _userService.GetByGuid(userGuid);
            if (user == null)
            {
                throw new PetClinicAppointmentNotFoundException("User not found!");
            }

            var appointments = _appointmentService.GetUserAppointments(userGuid);

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "User's appointments information has been successfully sent."
            });
            sonuc.Data = new
            {
                appointments = appointments.Select(appointment => new
                {
                    appointment.Guid,
                    appointment.AppointmentTime,
                    appointment.CreatedDate,
                    Pet = new
                    {
                        appointment.Pet.Guid,
                        appointment.Pet.Name,
                        Birthdate = appointment.Pet.DogumTarihi,
                        PlaceOfBirth = appointment.Pet.DogumYeri
                    },
                    User = new
                    {
                        appointment.User.Guid,
                        appointment.User.Name,
                        appointment.User.Surname,
                        appointment.User.Email
                    }
                })
            };

            return Ok(sonuc);
        }

        [HttpPost]
        [Route("")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER } })]
        public ActionResult AppointmentCreate([FromForm] AppointmentCO request)
        {
            var sonuc = new ResultDTO();

            if (request.PetGuid == null && request.PetGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Pet guid cannot be empty!");
            }

            if (request.AvailableAppointmentTimeGuid == null && request.AvailableAppointmentTimeGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Available appointment time guid cannot be empty!");
            }

            var pet = _petService.GetByGuid(request.PetGuid);
            if (pet == null)
            {
                throw new PetClinicAppointmentNotFoundException("Pet not found!");
            }

            var availableAppointment = _availableAppointmentService.GetByGuid(request.AvailableAppointmentTimeGuid);
            if (availableAppointment == null)
            {
                throw new PetClinicAppointmentNotFoundException("Admin has not created such an appointment time. Please send another appointment time!");
            }

            var appointmentVarMi = _appointmentService.GetByAppointment(request.PetGuid, request.AvailableAppointmentTimeGuid);
            if (appointmentVarMi != null)
            {
                throw new PetClinicAppointmentBadRequestException("Such an appointment already exists!");
            }

            var dto = new AppointmentDTO()
            {
                Guid = Guid.NewGuid(),
                Deleted = false,
                Actived = true,
                CreatedDate = DateTime.Now,
                AppointmentTime = availableAppointment.AppointmentTime,
                UserId = pet.UserId,
                PetId = pet.Id
            };

            var durum = _appointmentService.Create(dto);
            if (durum > 0)
            {
                sonuc.Status = EDurum.SUCCESS;
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.OK,
                    Status = EDurum.SUCCESS,
                    Description = "Appointment was created successfully."
                });
                sonuc.Data = new
                {
                    appointment = new
                    {
                        dto.Guid,
                        dto.AppointmentTime,
                        pet = new
                        {
                            dto.Pet.Guid,
                            dto.Pet.Name,
                            dto.Pet.DogumTarihi,
                            dto.Pet.DogumYeri,
                            owner = new
                            {
                                dto.User.Guid,
                                dto.User.Name,
                                dto.User.Surname,
                                dto.User.Email
                            }
                        }
                    }
                };
            }
            else
            {
                throw new PetClinicAppointmentBadRequestException("Couldn't create an appointment!");
            }

            return Ok(sonuc);
        }

        [HttpPut]
        [Route("{appointmentGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER, (int)Yetkiler.ADMIN } })]
        public ActionResult AppointmentUpdate(Guid appointmentGuid, [FromForm] AppointmentCO request)
        {
            var sonuc = new ResultDTO();

            if (request == null)
            {
                throw new PetClinicAppointmentBadRequestException("You have not sent any data!");
            }

            if (appointmentGuid == null && appointmentGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Appointment guid cannot be empty!");
            }

            var appointment = _appointmentService.GetByGuid(appointmentGuid);
            if (appointment == null)
            {
                throw new PetClinicAppointmentBadRequestException("Appointment guid cannot be empty!");
            }

            if (request.PetGuid == null && request.PetGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Pet guid cannot be empty!");
            }

            if (request.AvailableAppointmentTimeGuid == null && request.AvailableAppointmentTimeGuid == default(Guid))
            {
                throw new PetClinicAppointmentBadRequestException("Available appointment time guid cannot be empty!");
            }

            var pet = _petService.GetByGuid(request.PetGuid);
            if (pet == null)
            {
                throw new PetClinicAppointmentNotFoundException("Pet not found!");
            }

            var availableAppointment = _availableAppointmentService.GetByGuid(request.AvailableAppointmentTimeGuid);
            if (availableAppointment == null)
            {
                throw new PetClinicAppointmentNotFoundException("Admin has not created such an appointment time. Please send another appointment time!");
            }

            var appointmentVarMi = _appointmentService.GetByAppointment(request.PetGuid, request.AvailableAppointmentTimeGuid);
            if (appointmentVarMi != null)
            {
                throw new PetClinicAppointmentBadRequestException("Such an appointment already exists!");
            }

            appointment.AppointmentTime = availableAppointment.AppointmentTime;
            appointment.PetId = pet.Id;
            appointment.UserId = pet.UserId;

            var durum = _appointmentService.Update(appointment);
            if (durum > 0)
            {
                sonuc.Status = EDurum.SUCCESS;
                sonuc.Message.Add(new MessageDTO()
                {
                    Code = HttpStatusCode.OK,
                    Status = EDurum.SUCCESS,
                    Description = "The appointment has been successfully updated."
                });
                sonuc.Data = new { appointment = new { appointment.Guid } };
            }
            else
            {
                throw new PetClinicAppointmentBadRequestException("The appointment could not be updated!");
            }

            return Ok(sonuc);
        }

        [HttpDelete("{appointmentGuid:GUID}")]
        [TypeFilter(typeof(YetkiKontrol), Arguments = new object[] { new int[] { (int)Yetkiler.USER, (int)Yetkiler.ADMIN } })]
        public ActionResult AppointmentDelete(Guid appointmentGuid)
        {
            var appointment = _appointmentService.GetByGuid(appointmentGuid);
            if (appointment == null)
            {
                throw new PetClinicAppointmentNotFoundException("The appointment you want to delete was not found!");
            }

            var sonuc = new ResultDTO();

            _appointmentService.Delete(appointment.Id);

            sonuc.Status = EDurum.SUCCESS;
            sonuc.Message.Add(new MessageDTO()
            {
                Code = HttpStatusCode.OK,
                Status = EDurum.SUCCESS,
                Description = "The appointment has been successfully deleted."
            });

            return Ok(sonuc);
        }
    }
}