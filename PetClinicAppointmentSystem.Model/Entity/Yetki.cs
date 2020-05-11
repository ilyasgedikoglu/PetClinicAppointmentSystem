using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem.Model.Entity
{
    [Table("Yetki")]
    public partial class Yetki : Base
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(63)]
        public string Name { get; set; }

        [MaxLength(127)]
        public string Description { get; set; }

        public bool Showed { get; set; }
    }
}
