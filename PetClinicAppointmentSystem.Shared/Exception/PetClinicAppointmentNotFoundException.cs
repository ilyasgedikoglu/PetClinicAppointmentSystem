using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.Exception
{
    public class PetClinicAppointmentNotFoundException : System.Exception
    {
        public PetClinicAppointmentNotFoundException()
        {
        }

        public PetClinicAppointmentNotFoundException(string message) : base(message)
        {
        }
        public PetClinicAppointmentNotFoundException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
