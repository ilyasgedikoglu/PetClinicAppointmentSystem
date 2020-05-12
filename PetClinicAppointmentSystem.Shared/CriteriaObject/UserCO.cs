using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.CriteriaObject
{
    public class UserCO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string KimlikNo { get; set; }
        public string DogumYeri { get; set; }
        public DateTime? DogumTarihi { get; set; }
    }
}
