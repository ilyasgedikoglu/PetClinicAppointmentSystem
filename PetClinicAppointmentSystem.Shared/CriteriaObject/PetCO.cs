using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.CriteriaObject
{
    public class PetCO
    {
        public Guid UserGuid { get; set; }
        public string Name { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
