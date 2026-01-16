using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.Shared.Interfaces.Persistence;

public interface IBasePersistence<E, ID> : ICrudPersistence<E, ID>, IGetAllPersistence<E>
{
}
