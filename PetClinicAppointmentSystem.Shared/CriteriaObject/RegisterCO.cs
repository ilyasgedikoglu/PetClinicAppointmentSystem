using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinicAppointmentSystem.Shared.CriteriaObject
{
    public class RegisterCO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }

        public string AgainPassword { get; set; }
    }
}
