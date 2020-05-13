using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.CriteriaObject
{
    public class AppointmentCO
    {
        public Guid AvailableAppointmentTimeGuid { get; set; }
        public Guid PetGuid { get; set; }
    }
}
