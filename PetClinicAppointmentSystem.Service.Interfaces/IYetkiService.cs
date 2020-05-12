using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service.Interfaces
{
    public interface IYetkiService
    {
        YetkiDTO GetById(int id);
        YetkiDTO GetByGuid(Guid guid);
        int Create(YetkiDTO dto);
        void Delete(int id);
        int Update(YetkiDTO dto);
    }
}
