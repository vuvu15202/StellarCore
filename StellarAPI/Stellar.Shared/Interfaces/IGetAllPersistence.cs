using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.Shared.Interfaces.Persistence;

public interface IGetAllPersistence<E>
{
    IQueryable<E> Query();
}
