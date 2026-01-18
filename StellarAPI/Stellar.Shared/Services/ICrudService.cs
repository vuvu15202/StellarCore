using Stellar.Shared.Interfaces;

namespace Stellar.Shared.Services
{
    public interface ICrudService<E, ID, RES, REQ> :
        ICreateService<E, ID, RES, REQ>,
        IUpdateService<E, ID, RES, REQ>,
        // IPatchService<E, ID, RES, REQ>,
        IGetService<E, ID, RES>,
        IDeleteService<E, ID>
    {

    }
}
