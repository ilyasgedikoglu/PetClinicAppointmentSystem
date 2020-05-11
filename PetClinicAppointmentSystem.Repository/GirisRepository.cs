using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentSystem.Model.Entity;
using PetClinicAppointmentSystem.Model.Infrastructure;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Repository.Mapping;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository
{
    public class GirisRepository : IGirisRepository
    {
        private readonly IRepository<Giris> _girisRepository;
        private readonly DatabaseContext _context;
        public GirisRepository(IRepository<Giris> girisRepository, DatabaseContext context)
        {
            _girisRepository = girisRepository;
            _context = context;
        }

        public int Create(GirisDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Giris>(dto);
            entity.CreatedDate = DateTime.Now;
            entity.Guid = Guid.NewGuid();
            entity.User = null;
            entity.EntityState = EntityState.Added;
            return _girisRepository.Save(entity);
        }

        public void Delete(int id)
        {
            var entity = _girisRepository.GetSingle(x => x.Id == id && !x.Deleted);
            entity.Deleted = true;
            entity.EntityState = EntityState.Modified;
            _girisRepository.Save(entity);
        }

        public GirisDTO GetByGuid(Guid guid)
        {
            var entity = _girisRepository.GetSingle(x => x.Guid == guid && !x.Deleted);
            var dto = ModelMapper.Mapper.Map<GirisDTO>(entity);
            return dto;
        }

        public GirisDTO GetById(int id)
        {
            var entity = _girisRepository.GetSingle(x => x.Id == id && !x.Deleted);
            var dto = ModelMapper.Mapper.Map<GirisDTO>(entity);
            return dto;
        }

        public bool TokenKontrol(string token)
        {
            return _girisRepository.GetSingle(x => x.Token == token && !x.Deleted).Durum;
        }

        public GirisDTO KullanicininSonTokenBilgisi(int kullaniciId)
        {
            var girisEntity = _context.Girisler.Where(x => !x.Deleted && x.Actived && x.UserId == kullaniciId)
                .OrderByDescending(o => o.CreatedDate).First();
            var dto = ModelMapper.Mapper.Map<GirisDTO>(girisEntity);

            return dto;
        }

        public int Update(GirisDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Giris>(dto);
            entity.CreatedDate = DateTime.Now;
            //entity.Guid = Guid.NewGuid();
            entity.User = null;
            entity.EntityState = EntityState.Modified;
            return _girisRepository.Save(entity);
        }
    }
}
