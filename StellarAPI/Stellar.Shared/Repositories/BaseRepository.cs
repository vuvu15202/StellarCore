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
        private DbSet<E>? _dbSet;
        protected DbSet<E> DbSet => _dbSet ??= Context.Set<E>();

        public BaseRepository(DbContext context)
        {
            Context = context;
        }
    }

}
