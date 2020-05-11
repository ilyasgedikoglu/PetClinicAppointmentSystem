using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Service.Interfaces
{
    public interface IUserService
    {
        //KullaniciDTO GetById(int id);
        //KullaniciDTO GetByGuid(Guid guid);
        //KullaniciDTO GetByKullaniciAdiAndSifre(string kullaniciAdi, string sifre);
        //KullaniciDTO GetByKullaniciAdi(string kullaniciAdi);
        //int Create(KullaniciDTO kullanici);
        //int Update(KullaniciDTO kullanici);
        //void Delete(int id);
        //string Sifrele(string metin, string tuzlamaDegeri);
        //string GetTuzlamaDegeri();
        //SayfalamaDTO<KullaniciDTO> GetKullaniciList(KullaniciCO co);
        //List<KullaniciDTO> GetKullanicilar();
        //int KullaniciSayisiByYetkiId(int yetkiId);
        bool KullaniciYetkiKontrol(int kullaniciId, int[] yetkiler);
    }
}
