using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository.Interfaces
{
    public interface IAppointmentRepository : ICrud<AppointmentDTO>
    {
    }
}
