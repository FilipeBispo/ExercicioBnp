using ExercicioBnp.Model;

namespace ExercicioBnp.Infrastructure.Interfaces
{
    public interface IIsinRepository
    {
        Task<Isin?> GetByIsinIdentifierAsync(string isinIdentifier);
        Task InsertAsync(Isin isin);
        Task BatchInsertAsync(IEnumerable<Isin> isins);

    }
}
