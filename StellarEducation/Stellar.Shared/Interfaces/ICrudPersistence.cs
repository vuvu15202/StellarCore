using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.Shared.Interfaces.Persistence;

public interface ICrudPersistence<E, ID>
{
    E Save(E entity);

    IEnumerable<E> SaveAll(IEnumerable<E> entities);

    E? FindById(ID id);

    bool ExistsById(ID id);

    IEnumerable<E> FindAllById(IEnumerable<ID> ids);

    void DeleteById(ID id);

    void DeleteByIds(IEnumerable<ID> ids);

    void Delete(E entity);

    void DeleteAll(IEnumerable<E> entities);
}
