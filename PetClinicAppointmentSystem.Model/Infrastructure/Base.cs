using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PetClinicAppointmentSystem.Model.Infrastructure
{
    public class Base
    {
        [NotMapped]
        public EntityState EntityState { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Guid { get; set; }
        public bool Actived { get; set; }
        public bool Deleted { get; set; }
    }
}
