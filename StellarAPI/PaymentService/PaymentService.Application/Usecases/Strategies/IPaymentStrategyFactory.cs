using PaymentService.Domain.Enums;

namespace PaymentService.Application.Usecases.Strategies
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetStrategy(PaymentMethod method);
    }
}
