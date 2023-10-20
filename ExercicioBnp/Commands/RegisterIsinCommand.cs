using MediatR;

namespace ExercicioBnp.Commands
{
    public class RegisterIsinCommand : IRequest<Unit>
    {
        public List<string> IsinIdentifierList { get; set; }
    }
}
