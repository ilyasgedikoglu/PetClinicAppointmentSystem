using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service.Interfaces
{
    public interface IPetService
    {
        PetDTO GetById(int id);
        PetDTO GetByGuid(Guid guid);
        int Create(PetDTO dto);
        void Delete(int id);
        int Update(PetDTO dto);
    }
}
