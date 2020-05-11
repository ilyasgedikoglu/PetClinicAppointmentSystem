using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using PetClinicAppointmentSystem.Shared.Enumerations;

namespace PetClinicAppointmentSystem.Shared.DTO
{
    public class MessageDTO
    {
        public HttpStatusCode Code { get; set; }
        public string Description { get; set; }
        public EDurum Status { get; set; }

    }
}
