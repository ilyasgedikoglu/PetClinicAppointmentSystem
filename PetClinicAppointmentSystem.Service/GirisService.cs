using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service
{
    public class GirisService : IGirisService
    {
        private readonly IGirisRepository _girisRepository;
        public GirisService(IGirisRepository girisRepository)
        {
            _girisRepository = girisRepository;
        }
        public int Create(GirisDTO dto)
        {
            return _girisRepository.Create(dto);
        }

        public int Update(GirisDTO dto)
        {
            return _girisRepository.Update(dto);
        }

        public void Delete(int id)
        {
            _girisRepository.Delete(id);
        }

        public GirisDTO GetByGuid(Guid guid)
        {
            return _girisRepository.GetByGuid(guid);
        }

        public GirisDTO GetById(int id)
        {
            return _girisRepository.GetById(id);
        }

        public bool TokenKontrol(string token)
        {
            return _girisRepository.TokenKontrol(token);
        }

        public GirisDTO KullanicininSonTokenBilgisi(int kullaniciId)
        {
            return _girisRepository.KullanicininSonTokenBilgisi(kullaniciId);
        }
    }
}
