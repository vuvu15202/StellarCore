using Stellar.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Stellar.Shared.Services
{
    public interface IBaseService<E, ID, RES, REQ, PAGE_RES> : 
        ICrudService<E, ID, RES, REQ>,
        IGetAllService<E, PAGE_RES>
    {
    }

}
