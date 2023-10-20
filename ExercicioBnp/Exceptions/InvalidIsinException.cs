namespace ExercicioBnp.Exceptions
{
    public class InvalidIsinException : CustomException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;

        public InvalidIsinException(string isin)
            : base($"There was an error, the ISIN {isin} is invalid or not supported.")
        { }
    }
}
