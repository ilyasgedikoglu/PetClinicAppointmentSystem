using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.DTO
{
    [DataContract]
    [Serializable]
    public class YetkiDTO : BaseDTO
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
