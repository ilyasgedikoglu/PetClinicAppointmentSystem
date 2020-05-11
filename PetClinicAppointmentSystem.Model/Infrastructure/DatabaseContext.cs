using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentSystem.Model.Entity;

namespace PetClinicAppointmentSystem.Model.Infrastructure
{
    public sealed partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Giris> Girisler { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AvailableAppointmentTime> AvailableAppointmentTimes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Yetki> Yetkiler { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }
    }
}
