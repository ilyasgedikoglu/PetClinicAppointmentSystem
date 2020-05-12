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
    public class PetRepository : IPetRepository
    {
        private readonly IRepository<Pet> _petRepository;
        private readonly DatabaseContext _context;

        public PetRepository(IRepository<Pet> petRepository, DatabaseContext context,
            IRepository<User> userRepository)
        {
            _petRepository = petRepository;
            _context = context;
        }

        public PetDTO GetByGuid(Guid guid)
        {
            var entity = _context.Pets.Where(x => x.Guid == guid && !x.Deleted).AsNoTracking().FirstOrDefault();

            var dto = ModelMapper.Mapper.Map<PetDTO>(entity);
            return dto;
        }

        public int Create(PetDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Pet>(dto);
            entity.EntityState = EntityState.Added;
            return _petRepository.Save(entity);
        }

        public void Delete(int id)
        {
            var entity = _petRepository.GetSingle(x => x.Id == id && !x.Deleted);
            entity.Deleted = true;
            entity.Actived = false;
            entity.EntityState = EntityState.Modified;
            _petRepository.Save(entity);
        }

        public PetDTO GetById(int id)
        {
            var entity = _petRepository.GetSingle(x => x.Id == id && !x.Deleted);
            var dto = ModelMapper.Mapper.Map<PetDTO>(entity);
            return dto;
        }

        public int Update(PetDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Pet>(dto);
            entity.EntityState = EntityState.Modified;
            return _petRepository.Save(entity);
        }
    }
}
