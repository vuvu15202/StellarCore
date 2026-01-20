using System;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Usecases.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionResponse> Subscribe(SubscriptionRequest request);
        Task<SubscriptionResponse?> GetActiveSubscription(Guid userId);
    }
}
