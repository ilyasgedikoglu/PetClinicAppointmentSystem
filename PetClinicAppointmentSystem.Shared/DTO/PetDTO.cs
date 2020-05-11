using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.DTO
{
    [Serializable]
    [DataContract]
    public class PetDTO : BaseDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Resim { get; set; }
        [DataMember]
        public string DogumYeri { get; set; }
        [DataMember]
        public DateTime DogumTarihi { get; set; }
        [DataMember]
        public virtual UserDTO User { get; set; }
    }
}
