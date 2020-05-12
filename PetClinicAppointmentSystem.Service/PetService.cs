using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Service.Interfaces;
using PetClinicAppointmentSystem.Shared.DTO;

namespace PetClinicAppointmentSystem.Service
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            this._petRepository = petRepository;
        }

        public PetDTO GetByGuid(Guid guid)
        {
            return _petRepository.GetByGuid(guid);
        }

        public int Create(PetDTO dto)
        {
            return _petRepository.Create(dto);
        }

        public void Delete(int id)
        {
            _petRepository.Delete(id);
        }

        public PetDTO GetById(int id)
        {
            return _petRepository.GetById(id);
        }

        public int Update(PetDTO dto)
        {
            return _petRepository.Update(dto);
        }
    }
}
