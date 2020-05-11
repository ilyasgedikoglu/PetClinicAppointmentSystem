using System;
using System.Collections.Generic;
using System.Text;
using PetClinicAppointmentSystem.Shared.Enumerations;

namespace PetClinicAppointmentSystem.Shared.DTO
{
    public class ResultDTO
    {
        public EDurum Status { get; set; }
        public List<MessageDTO> Message { get; set; }
        public object Data { get; set; }

        public ResultDTO()
        {
            this.Message = new List<MessageDTO>();
        }
    }
}
