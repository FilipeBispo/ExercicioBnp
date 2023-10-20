namespace ExercicioBnp.Exceptions
{
    public class IsinInsertionException : CustomException
    {
        public override int StatusCode => StatusCodes.Status500InternalServerError;

        public IsinInsertionException(string isin)
            : base($"There was an error inserting ISIN {isin} into the database.")
        { }

        public IsinInsertionException( )
            : base($"There was an error inserting the ISINs into the database.")
        { }
    }
}
