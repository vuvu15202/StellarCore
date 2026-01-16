using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stellar.Shared.Repositories
{
    public class GetAllRepository<E> : BaseRepository<E>, IGetAllPersistence<E>
    where E : class
    {
        public GetAllRepository(DbContext context) : base(context) { }

        public IQueryable<E> Query()
        {
            return DbSet.AsQueryable();
        }
    }

}
