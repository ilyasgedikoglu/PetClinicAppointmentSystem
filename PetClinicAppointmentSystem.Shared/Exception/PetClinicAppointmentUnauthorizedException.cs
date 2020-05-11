using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.Exception
{
    public class PetClinicAppointmentUnauthorizedException : System.Exception
    {
        public PetClinicAppointmentUnauthorizedException()
        {
        }

        public PetClinicAppointmentUnauthorizedException(string message) : base(message)
        {
        }
        public PetClinicAppointmentUnauthorizedException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
