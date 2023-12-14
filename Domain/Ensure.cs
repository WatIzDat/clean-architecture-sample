using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Domain
{
    public static class Ensure
    {
        public static void NotNullOrEmpty(
            [NotNull] string? value,
            [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}