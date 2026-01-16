using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.Shared.Services
{
    public interface IBaseService<E, ID, RES, REQ, PAGE_RES> :
    ICrudService<E, ID, RES, REQ>
    {

    }

}
