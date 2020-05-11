using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.DTO
{
    [Serializable]
    [DataContract]
    public class UserDTO : BaseDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string TuzlamaDegeri { get; set; }
        [DataMember]
        public string KimlikNo { get; set; }
        [DataMember]
        public string Resim { get; set; }
        [DataMember]
        public string DogumYeri { get; set; }
        [DataMember]
        public DateTime? DogumTarihi { get; set; }
        [DataMember]
        public int? YetkiId { get; set; }
        [DataMember]
        public virtual YetkiDTO Yetki { get; set; }
    }
}
