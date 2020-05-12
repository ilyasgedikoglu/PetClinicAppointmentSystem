using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            this._appointmentRepository = appointmentRepository;
        }

        public AppointmentDTO GetByGuid(Guid guid)
        {
            return _appointmentRepository.GetByGuid(guid);
        }

        public int Create(AppointmentDTO dto)
        {
            return _appointmentRepository.Create(dto);
        }

        public void Delete(int id)
        {
            _appointmentRepository.Delete(id);
        }

        public AppointmentDTO GetById(int id)
        {
            return _appointmentRepository.GetById(id);
        }

        public int Update(AppointmentDTO dto)
        {
            return _appointmentRepository.Update(dto);
        }
    }
}
