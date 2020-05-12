using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository.Interfaces
{
    public interface IUserRepository : ICrud<UserDTO>
    {
        UserDTO GetByKullaniciAdiAndSifre(string kullaniciAdi, string sifre);
        UserDTO GetByKullaniciAdi(string kullaniciAdi);
        string GetByTuzlamaDegeri(string kullaniciAdi);
        bool KullaniciYetkiKontrol(int kullaniciId, int[] yetkiler);
    }
}
