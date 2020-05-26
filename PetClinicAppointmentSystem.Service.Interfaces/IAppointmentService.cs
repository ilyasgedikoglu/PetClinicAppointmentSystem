using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service.Interfaces
{
    public interface IAppointmentService
    {
        AppointmentDTO GetById(int id);
        AppointmentDTO GetByGuid(Guid guid);
        int Create(AppointmentDTO dto);
        void Delete(int id);
        int Update(AppointmentDTO dto);
        List<AppointmentDTO> GetAllAppointments();
        List<AppointmentDTO> GetUserAppointments(Guid userGuid);
        List<AppointmentDTO> GetByAppointment(Guid petGuid, Guid availableAppointmentGuid);
    }
}
