using ExercicioBnp.Exceptions;
using ExercicioBnp.Infrastructure;
using ExercicioBnp.Services.Interfaces;
using ExercicioBnp.Settings;
using Microsoft.Extensions.Options;

namespace ExercicioBnp.Services
{
    public class ExternalPriceService : IExternalPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalPriceService> _logger;
        private readonly string _baseUrl;

        public ExternalPriceService(HttpClient httpClient, ILogger<ExternalPriceService> logger, IOptions<ExternalPriceServiceSettings> settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = settings.Value.BaseUrl;
        }

        public async Task<decimal> GetPriceForIsin(string isinIdentifier)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/securityprice/{isinIdentifier}");

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
