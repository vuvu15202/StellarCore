using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Models.Exceptions;
using System.Net;

namespace Stellar.Shared.Services
{
    public interface IGetService<E, ID, RES> : IResponseMapper<E, RES>, ICrudPersistenceProvider<E, ID>
    {
        public RES GetById(HeaderContext context, ID id)
        {
            E entity = GetCrudPersistence().FindById(id);

            if (entity == null)
            {
                throw new ResponseException("Not found", CommonErrorMessage.OBJECT_NOT_FOUND, 404);
            }

            return MappingResponse(context, entity);
        }
    }
}
