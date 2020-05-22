using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service.Interfaces
{
    public interface IAvailableAppointmentService
    {
        AvailableAppointmentTimeDTO GetById(int id);
        AvailableAppointmentTimeDTO GetByGuid(Guid guid);
        int Create(AvailableAppointmentTimeDTO dto);
        void Delete(int id);
        int Update(AvailableAppointmentTimeDTO dto);
        List<AvailableAppointmentTimeDTO> GetAllAvailableAppointmentTimes();
    }
}
