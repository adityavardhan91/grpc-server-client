using Grpc.Core;

namespace GrpcServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<PriceDetailsResponse> CalculateTax(PriceDetailsRequest request, ServerCallContext context)
        {
            var taxPercentage = GetTaxPercentageBasedOnTheCountry(request.CountryName);
            var taxAmount = GetTaxAmount(request, taxPercentage);
            return Task.FromResult(new PriceDetailsResponse
            {
                TaxAmount = (Double)taxAmount,
                TotalAmount = request.SalePrice - (double)taxAmount
            });
        }

        private decimal GetTaxAmount(PriceDetailsRequest request, decimal taxPercentage)
        {
            var taxamount = (decimal)request.SalePrice * (taxPercentage / 100);

            return taxamount;
        }

        private decimal GetTaxPercentageBasedOnTheCountry(Country countryName)
        {
            if (countryName == Country.Netherlands)
            {
                decimal netherLandsVatPercentage = 21;
                return netherLandsVatPercentage;
            }

            decimal generalVatPercentage = 15;
            return generalVatPercentage;
        }
    }
}
