using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stellar.Shared.Repositories
{
    public class CrudRepository<E, ID> : BaseRepository<E>, ICrudPersistence<E, ID> // IGetAllPersistence is not strictly required here if not used, but implicit via ICrud? No, ICrud doesn't inherit it anymore. IBase does. 
    // Wait, the user wants CrudRepository to be usable. Often it implements both. 
    // I will let it implement ICrudPersistence<E, ID> and IGetAllPersistence<E> to match "BasePersistence" capabilities if verified.
    // Actually, checking the Java side, CrudPersistence DOES NOT inherit GetAll. BasePersistence does.
    // So CrudRepository should probably just be Crud. Inheriting IGetAllPersistence is fine if we want it to allow Query().
    // Let's keep IGetAllPersistence support.
    , IGetAllPersistence<E>
    where E : class
    {
        public CrudRepository(DbContext context) : base(context) { }

        public IQueryable<E> Query()
        {
            return DbSet.AsQueryable();
        }

        public E Save(E entity)
        {
            var entry = Context.Entry(entity);
            var idProperty = entry.Metadata.FindPrimaryKey().Properties.FirstOrDefault();
            
            if (idProperty != null)
            {
               var idValue = entry.Property(idProperty.Name).CurrentValue;
               
               if (idValue != null && !idValue.Equals(GetDefault(idValue.GetType())))
               {
                    var existing = DbSet.Find(idValue);
                    if (existing == null) 
                    {
                        DbSet.Add(entity);
                    }
                    else
                    {
                        // Update existing logic
                        if (existing != entity)
                        {
                            Context.Entry(existing).State = EntityState.Detached;
                        }
                        Context.Update(entity);
                    }
               }
               else
               {
                   DbSet.Add(entity);
               }
            }
            else
            {
                DbSet.Add(entity);
            }

            Context.SaveChanges();
            return entity;
        }

        public IEnumerable<E> SaveAll(IEnumerable<E> entities)
        {
            var results = new List<E>();
            foreach (var entity in entities)
            {
                results.Add(Save(entity));
            }
            return results;
        }

        private static object GetDefault(Type type)
        {
           return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public void Delete(E entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public void DeleteAll(IEnumerable<E> entities)
        {
            DbSet.RemoveRange(entities);
            Context.SaveChanges();
        }

        public void DeleteById(ID id)
        {
            var entity = FindById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void DeleteByIds(IEnumerable<ID> ids)
        {
            var entities = new List<E>();
            foreach (var id in ids)
            {
                var entity = FindById(id);
                if (entity != null)
                {
                    entities.Add(entity);
                }
            }
            
            if (entities.Any())
            {
                DeleteAll(entities);
            }
        }

        public E? FindById(ID id)
        {
            return DbSet.Find(id);
        }

        public bool ExistsById(ID id)
        {
            return FindById(id) != null;
        }

        public IEnumerable<E> FindAllById(IEnumerable<ID> ids)
        {
            var results = new List<E>();
            foreach (var id in ids)
            {
                var entity = FindById(id);
                if (entity != null)
                {
                    results.Add(entity);
                }
            }
            return results;
        }
    }

}
