using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.Shared.Interfaces
{
    public interface ICrudPersistenceProvider<E, ID>
    {
        ICrudPersistence<E, ID> GetCrudPersistence();
    }
}
