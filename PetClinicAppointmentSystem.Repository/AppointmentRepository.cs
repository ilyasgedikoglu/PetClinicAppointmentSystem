using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetClinicAppointmentSystem.Model.Entity;
using PetClinicAppointmentSystem.Model.Infrastructure;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Repository.Mapping;
using PetClinicAppointmentSystem.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace PetClinicAppointmentSystem.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly DatabaseContext _context;

        public AppointmentRepository(IRepository<Appointment> appointmentRepository, DatabaseContext context)
        {
            _appointmentRepository = appointmentRepository;
            _context = context;
        }

        public AppointmentDTO GetByGuid(Guid guid)
        {
            var entity = _context.Appointments.Where(x => x.Guid == guid && !x.Deleted).Include(x=>x.User).Include(x=>x.Pet).AsNoTracking().FirstOrDefault();

            var dto = ModelMapper.Mapper.Map<AppointmentDTO>(entity);
            return dto;
        }

        public int Create(AppointmentDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Appointment>(dto);
            entity.EntityState = EntityState.Added;
            return _appointmentRepository.Save(entity);
        }

        public void Delete(int id)
        {
            var entity = _appointmentRepository.GetSingle(x => x.Id == id && !x.Deleted);
            entity.Deleted = true;
            entity.Actived = false;
            entity.EntityState = EntityState.Modified;
            _appointmentRepository.Save(entity);
        }

        public AppointmentDTO GetById(int id)
        {
            var entity = _context.Appointments.Where(x => x.Id == id && !x.Deleted).Include(x => x.User).Include(x => x.Pet).AsNoTracking().FirstOrDefault();
            var dto = ModelMapper.Mapper.Map<AppointmentDTO>(entity);
            return dto;
        }

        public int Update(AppointmentDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Appointment>(dto);
            entity.EntityState = EntityState.Modified;
            return _appointmentRepository.Save(entity);
        }

        public List<AppointmentDTO> GetAllAppointments()
        {
            var entities = _context.Appointments.Where(x => !x.Deleted && x.AppointmentTime >= DateTime.Now).Include(x=>x.User).Include(x => x.Pet).AsNoTracking().ToList();
            return ModelMapper.Mapper.Map<List<AppointmentDTO>>(entities);
        }

        public List<AppointmentDTO> GetUserAppointments(Guid userGuid)
        {
            var user = _context.Users.FirstOrDefault(x => !x.Deleted && x.Guid == userGuid);
            var entities = _context.Appointments.Where(x => !x.Deleted && x.UserId == user.Id && x.AppointmentTime >= DateTime.Now).Include(x => x.User).Include(x => x.Pet).AsNoTracking().ToList();
            return ModelMapper.Mapper.Map<List<AppointmentDTO>>(entities);
        }

        public List<AppointmentDTO> GetByAppointment(Guid petGuid, Guid availableAppointmentGuid)
        {
            var availableAppointment =
                _context.AvailableAppointmentTimes.FirstOrDefault(x =>
                    !x.Deleted && x.Guid == availableAppointmentGuid);

            var pet = _context.Pets.FirstOrDefault(x =>
                !x.Deleted && x.Guid == petGuid);

            var entity = _context.Appointments.Where(x => !x.Deleted && x.AppointmentTime == availableAppointment.AppointmentTime && x.PetId == pet.Id).AsNoTracking().ToList();
            var dto = ModelMapper.Mapper.Map<List<AppointmentDTO>>(entity);
            return dto;
        }
    }
}
