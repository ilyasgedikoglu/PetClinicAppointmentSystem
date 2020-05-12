using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service
{
    public class YetkiService : IYetkiService
    {
        private readonly IYetkiRepository _yetkiRepository;

        public YetkiService(IYetkiRepository yetkiRepository)
        {
            this._yetkiRepository = yetkiRepository;
        }

        public YetkiDTO GetByGuid(Guid guid)
        {
            return _yetkiRepository.GetByGuid(guid);
        }

        public int Create(YetkiDTO dto)
        {
            return _yetkiRepository.Create(dto);
        }

        public void Delete(int id)
        {
            _yetkiRepository.Delete(id);
        }

        public YetkiDTO GetById(int id)
        {
            return _yetkiRepository.GetById(id);
        }

        public int Update(YetkiDTO dto)
        {
            return _yetkiRepository.Update(dto);
        }
    }
}
