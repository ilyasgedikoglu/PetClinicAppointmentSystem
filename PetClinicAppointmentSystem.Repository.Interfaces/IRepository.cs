using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetClinicAppointmentSystem.Repository.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] includeProperties);
        T GetSingle(Func<T, bool> @where, params Expression<Func<T, object>>[] includeProperties);
        T GetLast(params Expression<Func<T, object>>[] includeProperties);
        int Save(T item);
        bool ExecuteQuery(string query, params object[] parameters);
        List<T> SqlQuery(string query, params object[] parameters);
        object ExecuteNonQuery(string query, params object[] parameters);
        void SaveAll(List<T> items);
        Task<int> SaveChangesAsync(T item);
    }
}
