using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Repositories
{
    public interface IPaymentRepository
    {
    }

    public class PaymentRepository : IPaymentRepository
    {
        private readonly ILogger<PaymentRepository> _logger;

        private readonly IApplicationConfiguration _configuration;
        private readonly BusinessId _businessId;

        public PaymentRepository(ILogger<PaymentRepository> logger,
            IApplicationConfiguration configuration,
            BusinessId businessId)
        {
            _logger = logger;
            _configuration = configuration;
            _businessId = businessId;
        }
    }
}