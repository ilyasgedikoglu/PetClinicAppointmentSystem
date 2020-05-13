using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service.Interfaces
{
    public interface IUserService
    {
        UserDTO GetById(int id);
        UserDTO GetByGuid(Guid guid);
        UserDTO GetByKullaniciAdiAndSifre(string kullaniciAdi, string sifre);
        UserDTO GetByKullaniciAdi(string kullaniciAdi);
        int Create(UserDTO kullanici);
        int Update(UserDTO kullanici);
        void Delete(int id);
        string Sifrele(string metin, string tuzlamaDegeri);
        string GetTuzlamaDegeri();
        bool KullaniciYetkiKontrol(int kullaniciId, int[] yetkiler);
        List<UserDTO> GetAllUsers();
    }
}
