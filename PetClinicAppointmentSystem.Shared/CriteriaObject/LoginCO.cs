using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.CriteriaObject
{
    [DataContract]
    [Serializable]
    public class LoginCO
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

    }
}
