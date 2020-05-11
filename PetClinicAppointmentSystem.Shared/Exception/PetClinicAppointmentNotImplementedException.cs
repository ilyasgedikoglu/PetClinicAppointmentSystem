using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.Exception
{
    public class PetClinicAppointmentNotImplementedException : System.Exception
    {
        public PetClinicAppointmentNotImplementedException()
        {
        }

        public PetClinicAppointmentNotImplementedException(string message) : base(message)
        {
        }
        public PetClinicAppointmentNotImplementedException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
