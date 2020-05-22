using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service
{
    public class AvailableAppointmentService : IAvailableAppointmentService
    {
        private readonly IAvailableAppointmentRepository _availableAppointmentRepository;

        public AvailableAppointmentService(IAvailableAppointmentRepository availableAppointmentRepository)
        {
            this._availableAppointmentRepository = availableAppointmentRepository;
        }

        public AvailableAppointmentTimeDTO GetByGuid(Guid guid)
        {
            return _availableAppointmentRepository.GetByGuid(guid);
        }

        public int Create(AvailableAppointmentTimeDTO dto)
        {
            return _availableAppointmentRepository.Create(dto);
        }

        public void Delete(int id)
        {
            _availableAppointmentRepository.Delete(id);
        }

        public AvailableAppointmentTimeDTO GetById(int id)
        {
            return _availableAppointmentRepository.GetById(id);
        }

        public int Update(AvailableAppointmentTimeDTO dto)
        {
            return _availableAppointmentRepository.Update(dto);
        }

        public List<AvailableAppointmentTimeDTO> GetAllAvailableAppointmentTimes()
        {
            return _availableAppointmentRepository.GetAllAvailableAppointmentTimes();
        }
    }
}
