using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.DTO
{
    [DataContract]
    [Serializable]
    public class AppointmentDTO : BaseDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime AppointmentTime { get; set; }
        [DataMember]
        public int Time { get; set; }
        [DataMember]
        public int PetId { get; set; }
        [DataMember]
        public virtual PetDTO Pet { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual UserDTO User { get; set; }
    }
}
