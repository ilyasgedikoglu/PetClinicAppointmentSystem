using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.DTO
{
    [Serializable]
    [DataContract]
    public class AvailableAppointmentTimeDTO : BaseDTO
    {
        [DataMember]
        public DateTime AppointmentTime { get; set; }
        [DataMember]
        public int Time { get; set; }
    }
}
