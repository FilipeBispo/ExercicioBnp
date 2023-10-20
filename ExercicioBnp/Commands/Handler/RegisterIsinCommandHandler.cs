using Microsoft.AspNetCore.Mvc.ActionConstraints;
using ExercicioBnp.Infrastructure;
using ExercicioBnp.Model;
using ExercicioBnp.Services;
using ExercicioBnp.Services.Interfaces;
using ExercicioBnp.Infrastructure.Interfaces;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using ExercicioBnp.Helpers;

namespace ExercicioBnp.Commands.Handler
{
    public class RegisterIsinCommandHandler : IRequestHandler<RegisterIsinCommand, Unit>
    {
        private readonly IIsinRepository _isinRepository;
        private readonly IExternalPriceService _priceService;

        public RegisterIsinCommandHandler(IIsinRepository isinRepository, IExternalPriceService priceService)
        {
            _isinRepository = isinRepository;
            _priceService = priceService;
        }

        public async Task<Unit> Handle(RegisterIsinCommand request, CancellationToken cancellationToken)
        {
            var batchInsertIsin = new List<Isin>();
            foreach (var isinIdentifier in request.IsinIdentifierList)
            {
                IsinValidationHelper.EnsureValid(isinIdentifier);
                var isin = await _isinRepository.GetByIsinIdentifierAsync(isinIdentifier);

                if (isin == null)
                {
                    var price = await _priceService.GetPriceForIsin(isinIdentifier);
                    var newIsin = new Isin { Identifier = isinIdentifier, Price = price };

                    batchInsertIsin.Add(newIsin);
                }
            }

            return Unit.Value;
        }
    }

}
