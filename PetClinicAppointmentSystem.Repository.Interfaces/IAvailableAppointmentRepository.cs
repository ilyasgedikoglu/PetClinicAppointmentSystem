﻿using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository.Interfaces
{
    public interface IAvailableAppointmentRepository : ICrud<AvailableAppointmentTimeDTO>
    {
        List<AvailableAppointmentTimeDTO> GetAllAvailableAppointmentTimes();
    }
}
