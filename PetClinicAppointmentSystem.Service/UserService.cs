using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.DTO;
using PetClinicAppointmentSystem.Shared.Exception;

namespace PetClinicAppointmentSystem.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO GetByGuid(Guid guid)
        {
            return _userRepository.GetByGuid(guid);
        }

        public int Create(UserDTO dto)
        {
            return _userRepository.Create(dto);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public UserDTO GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public int Update(UserDTO dto)
        {
            return _userRepository.Update(dto);
        }

        public UserDTO GetByKullaniciAdiAndSifre(string kullaniciAdi, string sifre)
        {
            var tuzlama = _userRepository.GetByTuzlamaDegeri(kullaniciAdi);
            if (string.IsNullOrEmpty(tuzlama))
            {
                throw new PetClinicAppointmentNotFoundException("No user salting value has been created!");
            }

            var sifreliMetin = Sifrele(sifre, tuzlama);
            var user = _userRepository.GetByKullaniciAdiAndSifre(kullaniciAdi, sifreliMetin);
            return user;
        }

        public UserDTO GetByKullaniciAdi(string kullaniciAdi)
        {
            return _userRepository.GetByKullaniciAdi(kullaniciAdi);
        }
        public string Sifrele(string metin, string tuzlamaDegeri)
        {

            // var tuzlamaBytes = Encoding.UTF8.GetBytes(tuzlamaDegeri);
            //var tuzlama = Convert.ToBase64String(tuzlamaBytes);

            var sifrelenmisMetin = KeyDerivation.Pbkdf2(
                password: metin,
                salt: Encoding.UTF8.GetBytes(tuzlamaDegeri),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 1000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(sifrelenmisMetin);
        }

        public string GetTuzlamaDegeri()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public bool KullaniciYetkiKontrol(int kullaniciId, int[] yetkiler)
        {
            return _userRepository.KullaniciYetkiKontrol(kullaniciId, yetkiler);
        }

        public List<UserDTO> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
    }
}
