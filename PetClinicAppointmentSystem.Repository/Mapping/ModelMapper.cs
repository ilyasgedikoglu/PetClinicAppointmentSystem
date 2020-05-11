using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository.Mapping
{
    internal static class ModelMapper
    {
        private static IMapper _mapper;

        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Model.Entity.User, UserDTO>();
                        cfg.CreateMap<Model.Entity.User, UserDTO>().ReverseMap();

                        cfg.CreateMap<Model.Entity.Appointment, AppointmentDTO>();
                        cfg.CreateMap<Model.Entity.Appointment, AppointmentDTO>().ReverseMap();

                        cfg.CreateMap<Model.Entity.AvailableAppointmentTime, AvailableAppointmentTimeDTO>();
                        cfg.CreateMap<Model.Entity.AvailableAppointmentTime, AvailableAppointmentTimeDTO>().ReverseMap();

                        cfg.CreateMap<Model.Entity.Giris, GirisDTO>();
                        cfg.CreateMap<Model.Entity.Giris, GirisDTO>().ReverseMap();

                        cfg.CreateMap<Model.Entity.Pet, PetDTO>();
                        cfg.CreateMap<Model.Entity.Pet, PetDTO>().ReverseMap();

                        cfg.CreateMap<Model.Entity.Yetki, YetkiDTO>();
                        cfg.CreateMap<Model.Entity.Yetki, YetkiDTO>().ReverseMap();
                    });

                    _mapper = config.CreateMapper();
                }
                return _mapper;
            }
        }


    }
}
