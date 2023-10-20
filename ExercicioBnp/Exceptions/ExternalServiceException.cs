namespace ExercicioBnp.Exceptions
{
    public class ExternalServiceException : CustomException
    {
        public override int StatusCode => StatusCodes.Status502BadGateway;

        public ExternalServiceException(string message)
            : base($"There was an error, {message}")
        { }
    }
}
