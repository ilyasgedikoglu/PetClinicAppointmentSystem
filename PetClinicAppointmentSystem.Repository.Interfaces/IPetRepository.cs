﻿using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Repository.Interfaces
{
    public interface IPetRepository : ICrud<PetDTO>
    {
        List<PetDTO> GetAllPets();
    }
}
