using System.Collections.Generic;

namespace Foncia.Domain.POS.Interfaces
{
    /// <summary>
    /// Goods price provider.
    /// </summary>
    public interface IGoodsPriceProvider
    {
        /// <summary>
        /// Get good prices by code.
        /// </summary>
        /// <param name="code">Good code.</param>
        /// <returns>Good prices.</returns>
        IReadOnlyDictionary<int, decimal>? GetPrices(int code);
    }
}
