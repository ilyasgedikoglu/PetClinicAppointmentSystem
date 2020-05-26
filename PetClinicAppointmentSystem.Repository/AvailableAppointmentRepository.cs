using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetClinicAppointmentSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentSystem.Model.Entity;
using PetClinicAppointmentSystem.Model.Infrastructure;
using PetClinicAppointmentSystem.Repository.Mapping;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository
{
    public class AvailableAppointmentRepository : IAvailableAppointmentRepository
    {
        private readonly IRepository<AvailableAppointmentTime> _availableAppointmentRepository;
        private readonly DatabaseContext _context;

        public AvailableAppointmentRepository(IRepository<AvailableAppointmentTime> availableAppointmentRepository, DatabaseContext context)
        {
            _availableAppointmentRepository = availableAppointmentRepository;
            _context = context;
        }

        public AvailableAppointmentTimeDTO GetByGuid(Guid guid)
        {
            var entity = _context.AvailableAppointmentTimes.Where(x => x.Guid == guid && !x.Deleted).AsNoTracking().FirstOrDefault();

            var dto = ModelMapper.Mapper.Map<AvailableAppointmentTimeDTO>(entity);
            return dto;
        }

        public int Create(AvailableAppointmentTimeDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<AvailableAppointmentTime>(dto);
            entity.EntityState = EntityState.Added;
            return _availableAppointmentRepository.Save(entity);
        }

        public void Delete(int id)
        {
            var entity = _availableAppointmentRepository.GetSingle(x => x.Id == id && !x.Deleted);
            entity.Deleted = true;
            entity.Actived = false;
            entity.EntityState = EntityState.Modified;
            _availableAppointmentRepository.Save(entity);
        }

        public AvailableAppointmentTimeDTO GetById(int id)
        {
            var entity = _availableAppointmentRepository.GetSingle(x => x.Id == id && !x.Deleted);
            var dto = ModelMapper.Mapper.Map<AvailableAppointmentTimeDTO>(entity);
            return dto;
        }

        public int Update(AvailableAppointmentTimeDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<AvailableAppointmentTime>(dto);
            entity.EntityState = EntityState.Modified;
            return _availableAppointmentRepository.Save(entity);
        }

        public List<AvailableAppointmentTimeDTO> GetAllAvailableAppointmentTimes()
        {
            var entities = _context.AvailableAppointmentTimes.Where(x => !x.Deleted && x.AppointmentTime >= DateTime.Now).AsNoTracking().ToList();
            return ModelMapper.Mapper.Map<List<AvailableAppointmentTimeDTO>>(entities);
        }
    }
}
