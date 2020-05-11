using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem.Model.Entity
{
    [Table("Pet")]
    public partial class Pet : Base
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [MaxLength(63)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Resim { get; set; }

        [MaxLength(63)]
        public string DogumYeri { get; set; }
        public DateTime DogumTarihi { get; set; }

        public virtual User User { get; set; }
    }
}
