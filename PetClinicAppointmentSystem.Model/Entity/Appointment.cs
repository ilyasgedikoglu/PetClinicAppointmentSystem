using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem.Model.Entity
{
    [Table("Appointment")]
    public partial class Appointment : Base
    {
        [Key]
        public int Id { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
