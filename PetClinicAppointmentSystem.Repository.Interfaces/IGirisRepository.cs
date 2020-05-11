using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository.Interfaces
{
    public interface IGirisRepository : ICrud<GirisDTO>
    {
        bool TokenKontrol(string token);
        GirisDTO KullanicininSonTokenBilgisi(int kullaniciId);
    }
}
