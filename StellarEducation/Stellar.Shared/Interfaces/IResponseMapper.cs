using System;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;

namespace Stellar.Shared.Interfaces
{
    public interface IResponseMapper<E, RES>
    {
        public RES MappingResponse(HeaderContext context, E entity)
        {
            // Requires RES to have a parameterless constructor
            RES res = Activator.CreateInstance<RES>();
            FnCommon.CopyProperties(res, entity);
            return res;
        }
    }
}
