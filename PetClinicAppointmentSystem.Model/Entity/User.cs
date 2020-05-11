using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem.Model.Entity
{
    [Table("User")]
    public partial class User : Base
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(63)]
        public string Name { get; set; }

        [MaxLength(63)]
        public string Surname { get; set; }

        [MaxLength(127)]
        public string Email { get; set; }

        [MaxLength(63)]
        public string Password { get; set; }

        [MaxLength(63)]
        public string TuzlamaDegeri { get; set; }

        [MaxLength(31)]
        public string KimlikNo { get; set; }

        [MaxLength(255)]
        public string Resim { get; set; }

        [MaxLength(63)]
        public string DogumYeri { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public int? YetkiId { get; set; }
        public virtual Yetki Yetki { get; set; }

    }
}
