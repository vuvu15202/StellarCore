using System;
using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;

namespace Stellar.Shared.Services
{
    public interface IDeleteService<E, ID> : ICrudPersistenceProvider<E, ID>
    {
        public void Delete(
            HeaderContext context,
            ID id,
            Action<HeaderContext, ID, E> validationHandler,
            Action<HeaderContext, ID, E> postDeleteHandler)
        {
            E entity = GetCrudPersistence().FindById(id);

            if (validationHandler != null)
            {
                validationHandler(context, id, entity);
            }

            if (postDeleteHandler != null)
            {
                postDeleteHandler(context, id, entity);
            }

            GetCrudPersistence().Delete(entity);
        }

        public void Delete(HeaderContext context, ID id)
        {
            if (GetCrudPersistence() == null)
            {
                throw new ArgumentException("deletePersistence must not be null");
            }

            Delete(context, id, ValidateDelete, PostDeleteHandler);
        }

        protected void ValidateDelete(HeaderContext context, ID id, E entity) { }
        protected void PostDeleteHandler(HeaderContext context, ID id, E entity) { }
    }
}
