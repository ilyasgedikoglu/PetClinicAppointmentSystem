using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinicAppointmentSystem.Repository.Interfaces
{
    public interface ICrud<T>
    {
        T GetById(int id);
        T GetByGuid(Guid guid);
        int Create(T dto);
        void Delete(int id);
        int Update(T dto);
    }
}
