namespace ExercicioBnp.Exceptions
{
    public class DatabaseConnectionException : CustomException
    {
        public override int StatusCode => StatusCodes.Status500InternalServerError;


        public DatabaseConnectionException(string isin)
            : base($"There was an error connecting to the database retriving this Isin: {isin}.")
        { }

        public DatabaseConnectionException()
            : base("There was an error connecting to the database.")
        { }
    }
}
