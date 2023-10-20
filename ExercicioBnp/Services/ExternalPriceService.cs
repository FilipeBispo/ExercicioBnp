using ExercicioBnp.Exceptions;
using ExercicioBnp.Infrastructure;
using ExercicioBnp.Services.Interfaces;

namespace ExercicioBnp.Services
{
    public class ExternalPriceService : IExternalPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalPriceService> _logger;

        public ExternalPriceService(HttpClient httpClient, ILogger<ExternalPriceService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<decimal> GetPriceForIsin(string isinIdentifier)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://securities.dataprovider.com/securityprice/{isinIdentifier}");

                response.EnsureSuccessStatusCode();

                var price = await response.Content.ReadFromJsonAsync<decimal>();

                return price;
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Error fetching data for ISIN {isinIdentifier}: {ex.Message}");
            }
        }
    }
}
