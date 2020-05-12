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
            //var entity = _yetkiRepository.GetSingle(x => x.Guid == guid && !x.Silindi);
            var entity = _context.Appointments.Where(x => x.Guid == guid && !x.Deleted).AsNoTracking().FirstOrDefault();

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
            var entity = _appointmentRepository.GetSingle(x => x.Id == id && !x.Deleted);
            var dto = ModelMapper.Mapper.Map<AppointmentDTO>(entity);
            return dto;
        }

        public int Update(AppointmentDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Appointment>(dto);
            entity.EntityState = EntityState.Modified;
            return _appointmentRepository.Save(entity);
        }
    }
}
