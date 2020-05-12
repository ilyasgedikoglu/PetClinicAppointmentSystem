using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentSystem.Model.Entity;
using PetClinicAppointmentSystem.Model.Infrastructure;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Repository.Mapping;
using PetClinicAppointmentSystem.Shared.DTO;
using PetClinicAppointmentSystem.Shared.Enumerations;
using PetClinicAppointmentSystem.Shared.Exception;

namespace PetClinicAppointmentSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<User> _userRepository;
        private readonly DatabaseContext _context;

        public UserRepository(IRepository<User> userRepository, DatabaseContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public UserDTO GetById(int id)
        {
            var user = _userRepository.GetSingle(u => !u.Deleted && u.Id == id, y => y.Yetki);
            return ModelMapper.Mapper.Map<UserDTO>(user);
        }

        public int Create(UserDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<User>(dto);
            entity.Yetki = null;
            entity.EntityState = EntityState.Added;
            return _userRepository.Save(entity);
        }

        public void Delete(int id)
        {
            var entity = _userRepository.GetSingle(x => x.Id == id);
            entity.Deleted = true;
            entity.Actived = false;
            entity.EntityState = EntityState.Modified;
            _userRepository.Save(entity);
        }

        public int Update(UserDTO dto)
        {
            var entity = ModelMapper.Mapper.Map<Model.Entity.User>(dto);
            entity.Yetki = null;
            entity.EntityState = EntityState.Modified;
            return _userRepository.Save(entity);
        }


        public UserDTO GetByKullaniciAdiAndSifre(string kullaniciAdi, string sifre)
        {
            var kullanici = _userRepository.GetSingle(x => !x.Deleted && x.Email != null && x.Email == kullaniciAdi && x.Password == sifre, y => y.Yetki);

            return ModelMapper.Mapper.Map<UserDTO>(kullanici);
        }

        public UserDTO GetByKullaniciAdi(string kullaniciAdi)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                throw new PetClinicAppointmentBadRequestException("Email is not null!");
            }

            var kullanici = _context.Users
                .Where(x => x.Email != null && x.Email == kullaniciAdi && !x.Deleted)
                .Include(x => x.Yetki).AsNoTracking().ToList().FirstOrDefault();
            return ModelMapper.Mapper.Map<UserDTO>(kullanici);
        }

        public string GetByTuzlamaDegeri(string kullaniciAdi)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                throw new PetClinicAppointmentBadRequestException("Email is not null!");
            }

            var user = _userRepository.GetSingle(x => !x.Deleted && x.Email == kullaniciAdi);
            if (user == null)
            {
                throw new PetClinicAppointmentNotFoundException("User is not found!");
            }

            var tuzlama = _context.Users.Where(x => !x.Deleted && x.Email != null && x.Email == kullaniciAdi).Select(y => new { y.Email, y.TuzlamaDegeri }).AsNoTracking().FirstOrDefault();

            return tuzlama.TuzlamaDegeri;
        }

        public UserDTO GetByGuid(Guid guid)
        {
            var user = _userRepository.GetSingle(u => !u.Deleted && u.Guid == guid, y => y.Yetki);
            if (user == null)
            {
                throw new PetClinicAppointmentNotFoundException("Kullanıcı bulunamadı!");
            }
            return ModelMapper.Mapper.Map<UserDTO>(user);
        }

        public bool KullaniciYetkiKontrol(int kullaniciId, int[] yetkiler)
        {
            var sonuc = _userRepository.GetSingle(u => !u.Deleted && u.YetkiId.HasValue && yetkiler.Contains(u.YetkiId.Value) && u.Id == kullaniciId);
            if (sonuc == null)
            {
                return false;
            }
            return true;
        }
    }
}
