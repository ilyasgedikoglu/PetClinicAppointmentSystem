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
    public class YetkiRepository : IYetkiRepository
    {
        private readonly IRepository<Yetki> _yetkiRepository;
        private readonly IRepository<User> _userRepository;
        private readonly DatabaseContext _context;

        public YetkiRepository(IRepository<Yetki> yetkiRepository, DatabaseContext context,
            IRepository<User> userRepository)
        {
            this._yetkiRepository = yetkiRepository;
            _context = context;
            _userRepository = userRepository;
        }

        public YetkiDTO GetByGuid(Guid guid)
        {
            //var entity = _yetkiRepository.GetSingle(x => x.Guid == guid && !x.Silindi);
            var entity = _context.Yetkiler.Where(x => x.Guid == guid && !x.Deleted).AsNoTracking().FirstOrDefault();

            var dto = ModelMapper.Mapper.Map<YetkiDTO>(entity);
            return dto;
        }

        public int Create(YetkiDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Yetki>(dto);
            entity.CreatedDate = DateTime.Now;
            entity.Guid = Guid.NewGuid();
            entity.EntityState = EntityState.Added;
            return _yetkiRepository.Save(entity);
        }

        public void Delete(int id)
        {
            var entity = _yetkiRepository.GetSingle(x => x.Id == id && !x.Deleted);
            entity.Deleted = true;
            entity.EntityState = EntityState.Modified;
            _yetkiRepository.Save(entity);
        }

        public YetkiDTO GetById(int id)
        {
            var entity = _yetkiRepository.GetSingle(x => x.Id == id && !x.Deleted);
            var dto = ModelMapper.Mapper.Map<YetkiDTO>(entity);
            return dto;
        }

        public int Update(YetkiDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Yetki>(dto);
            entity.EntityState = EntityState.Modified;
            return _yetkiRepository.Save(entity);
        }
    }
}
