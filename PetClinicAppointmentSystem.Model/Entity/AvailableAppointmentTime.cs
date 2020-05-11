using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem.Model.Entity
{
    [Table("AvailableAppointmentTime")]
    public partial class AvailableAppointmentTime : Base
    {
        [Key]
        public int Id { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int Time { get; set; }
    }
}
