using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem.Model.Entity
{
    [Table("Giris")]
    public partial class Giris : Base
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [MaxLength(511)]
        public string Token { get; set; }
        public bool Durum { get; set; }

        public virtual User User { get; set; }
    }
}
