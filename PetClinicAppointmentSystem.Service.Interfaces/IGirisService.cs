using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service.Interfaces
{
    public interface IGirisService
    {
        GirisDTO GetById(int id);
        GirisDTO GetByGuid(Guid guid);
        int Create(GirisDTO dto);
        void Delete(int id);
        int Update(GirisDTO dto);
        bool TokenKontrol(string token);
        GirisDTO KullanicininSonTokenBilgisi(int kullaniciId);
    }
}
