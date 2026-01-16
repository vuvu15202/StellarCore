using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stellar.Shared.Repositories
{
    public class BaseRepository<E> where E : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<E> DbSet;

        public BaseRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<E>();
        }
    }

}
