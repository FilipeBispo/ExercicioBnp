using ExercicioBnp.Exceptions;
using System.Linq;

namespace ExercicioBnp.Helpers
{
    public static class IsinValidationHelper
    {
        private const int ExpectedLength = 12;

        public static bool IsValid(string isin)
        {
            if (string.IsNullOrEmpty(isin)) { return false; }

            if (isin.Length != ExpectedLength) { return false; }

            if (!isin.All(char.IsLetterOrDigit)) { return false; }

            return true;
        }

        public static void EnsureValid(string isin)
        {
            if (!IsValid(isin))
                throw new InvalidIsinException(isin);
        }
    }
}
