using PaymentService.Domain.Enums;

namespace PaymentService.Application.Usecases.Strategies
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IEnumerable<IPaymentStrategy> _strategies;

        public PaymentStrategyFactory(IEnumerable<IPaymentStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IPaymentStrategy GetStrategy(PaymentMethod method)
        {
            var strategy = _strategies.FirstOrDefault(s => s.Method == method);
            if (strategy == null)
            {
                throw new ArgumentException($"Payment method {method} is not supported");
            }
            return strategy;
        }
    }
}
