using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.Exception
{
    public class PetClinicAppointmentBadRequestException : System.Exception
    {
        public PetClinicAppointmentBadRequestException()
        {
        }

        public PetClinicAppointmentBadRequestException(string message) : base(message)
        {
        }
        public PetClinicAppointmentBadRequestException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
