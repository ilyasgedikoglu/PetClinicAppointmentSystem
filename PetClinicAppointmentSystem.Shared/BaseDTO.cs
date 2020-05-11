using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared
{
    [Serializable]
    [DataContract]
    public abstract class BaseDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public bool Actived { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}
