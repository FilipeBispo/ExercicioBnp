namespace ExercicioBnp.Services.Interfaces
{
    public interface IExternalPriceService
    {
        Task<decimal> GetPriceForIsin(string isinIdentifier);
    }
}
