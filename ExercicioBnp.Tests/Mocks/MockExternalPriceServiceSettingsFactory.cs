using ExercicioBnp.Settings;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioBnp.Tests.Mocks
{
    public static class MockExternalPriceServiceSettingsFactory
    {
        public static IOptions<ExternalPriceServiceSettings> CreateMockSettings()
        {
            var mockSettings = new Mock<IOptions<ExternalPriceServiceSettings>>();
            mockSettings.Setup(s => s.Value).Returns(new ExternalPriceServiceSettings
            {
                BaseUrl = "https://securities.dataprovider.com/securityprice/"
            });

            return mockSettings.Object;
        }
    }
}
