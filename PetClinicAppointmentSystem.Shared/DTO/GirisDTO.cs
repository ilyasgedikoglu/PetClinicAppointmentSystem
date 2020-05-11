using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.DTO
{

    [DataContract]
    [Serializable]
    public class GirisDTO : BaseDTO
    {
        [DataMember]
        public int KullaniciId { get; set; }

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public bool Durum { get; set; }
    }
}
