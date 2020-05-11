using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentSystem.Model.Infrastructure;
using PetClinicAppointmentSystem.Repository.Interfaces;

namespace PetClinicAppointmentSystem.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DatabaseContext _context;
        private readonly DbContextOptions<DatabaseContext> _dbOptions;


        public Repository(DatabaseContext context, DbContextOptions<DatabaseContext> dbOptions)
        {
            _context = context;
            _dbOptions = dbOptions;
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();
            dbQuery = includeProperties.Aggregate(dbQuery, (current, includeProperty) => current.Include(includeProperty));
            return dbQuery.AsNoTracking().AsQueryable();
        }

        public IQueryable<T> GetList(Func<T, bool> @where, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> dbQuery = _context.Set<T>();
                dbQuery = includeProperties.Aggregate(dbQuery, (current, includeProperty) => current.Include(includeProperty));
                return dbQuery.AsNoTracking().Where(where).AsQueryable();
            }
            catch (DbException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public T GetSingle(Func<T, bool> @where, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> dbQuery = _context.Set<T>();
                dbQuery = includeProperties.Aggregate(dbQuery, (current, includeProperty) => current.Include(includeProperty));
                T result = dbQuery.AsNoTracking().FirstOrDefault(@where);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public T GetLast(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();
            dbQuery = includeProperties.Aggregate(dbQuery, (current, includeProperty) => current.Include(includeProperty));
            T result = dbQuery.AsNoTracking().ToList().Last();
            return result;
        }

        public int Save(T item)
        {
            try
            {

                //using (var context = serviceProvider.GetService<DatabaseContext>()) {
                using (var context = new DatabaseContext(_dbOptions))
                {

                    var dbSet = context.Set<T>();

                    //var local = _context.Set<T>()
                    //    .Local
                    //    .FirstOrDefault(entry => entry.Id.Equals(item.Id));

                    //// check if local is not null 
                    //if (!local.IsNull()) // I'm using a extension method
                    //{
                    //    // detach
                    //    actContext.Entry(local).State = EntityState.Detached;
                    //}
                    //// set Modified flag in your entry
                    //actContext.Entry(entryToUpdate).State = EntityState.Modified;

                    //// save 
                    //actContext.SaveChanges();




                    dbSet.Attach(item);
                    Type type = item.GetType().GetProperty("EntityState").PropertyType;


                    var state = item.GetType().GetProperty("EntityState").GetValue(item).ToString();

                    if (state != "Added")
                    {
                        context.Entry(item).State = EntityState.Modified;
                    }

                    var list = _context.ChangeTracker.Entries<Base>().ToList();
                    foreach (var entry in list)
                    {
                        var entity = entry.Entity;
                        entry.State = GetEntityState(entity.EntityState);
                    }


                    try
                    {
                        context.SaveChanges();
                        var result = Convert.ToInt32(context.Entry(item).GetDatabaseValues()["Id"].ToString());
                        context.Entry(item).State = EntityState.Detached;
                        return result;

                    }
                    catch (Exception ex)
                    {
                        context.Entry(item).State = EntityState.Detached;
                        return 0;

                    }
                }

            }
            catch (Exception ex)//DbEntityValidationException ex
            {
                throw ex;
            }

        }
        public async Task<int> SaveChangesAsync(T item)
        {
            try
            {
                var dbSet = _context.Set<T>();
                dbSet.Attach(item);
                Type type = item.GetType().GetProperty("EntityState").PropertyType;


                var state = item.GetType().GetProperty("EntityState").GetValue(item).ToString();

                if (state != "Added")
                {
                    _context.Entry(item).State = EntityState.Modified;
                }

                var list = _context.ChangeTracker.Entries<Base>().ToList();
                foreach (var entry in list)
                {
                    var entity = entry.Entity;
                    entry.State = GetEntityState(entity.EntityState);
                }


                try
                {
                    await _context.SaveChangesAsync();

                    var result = Convert.ToInt32(_context.Entry(item).GetDatabaseValues()["Id"].ToString());
                    _context.Entry(item).State = EntityState.Detached;
                    return result;

                }
                catch (Exception ex)
                {
                    _context.Entry(item).State = EntityState.Detached;
                    return 0;

                }

            }
            catch (Exception ex)//DbEntityValidationException ex
            {
                throw ex;
            }

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        protected static EntityState GetEntityState(EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Unchanged:
                    return EntityState.Unchanged;
                case EntityState.Added:
                    return EntityState.Added;
                case EntityState.Modified:
                    return EntityState.Modified;
                case EntityState.Deleted:
                    return EntityState.Deleted;
                case EntityState.Detached:
                    return EntityState.Detached;
            }
            return EntityState.Detached;
        }

        public bool ExecuteQuery(string query, params object[] parameters)
        {
            //return _context.Database.ExecuteSqlCommand(query, parameters) > 0;
            return false;
        }

        public List<T> SqlQuery(string query, params object[] parameters)
        {

            //var result = _context.Database.SqlQuery<T>(query, parameters).ToList();
            //  var result = _context.Set<T>().FromSql(query, parameters).ToList();
            // return result;
            return null;
        }

        public object ExecuteNonQuery(string query, params object[] parameters)
        {
            //var result = _context.Database.SqlQuery<string>(query, parameters);
            //var result = _context.Set<T>().FromSql(query, parameters).ToList();
            // var result = _context.Database.ExecuteSqlCommand(query, parameters);

            //  return result;
            return null;
        }


        public void SaveAll(List<T> items)
        {
            try
            {
                Stopwatch stUpdate = new Stopwatch();
                stUpdate.Start();

                using (DatabaseContext _contextSave = new DatabaseContext())
                {
                    items.ForEach(x => { _contextSave.Set<T>().Attach(x); });
                    _contextSave.ChangeTracker.Entries<Base>().ToList().ForEach(x => x.State = EntityState.Modified);
                    _contextSave.SaveChanges();

                }

                stUpdate.Stop();
                var Updateresult = stUpdate.ElapsedMilliseconds;
            }
            catch (Exception ex)//DbEntityValidationException ex
            {
                throw ex;
            }

        }


    }
}
