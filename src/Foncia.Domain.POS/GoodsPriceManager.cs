using Foncia.Domain.POS.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Foncia.Domain.POS
{
    /// <inheritdoc/>
    internal class GoodsPriceManager : IGoodsPriceManager
    {
        // it is assumed that price manager state would be modified in multi-threaded context
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, decimal>> Prices = 
            new ConcurrentDictionary<int, ConcurrentDictionary<int, decimal>>();

        public IReadOnlyDictionary<int, decimal>? GetPrices(int code)
        {
            return GetModifiablePrices(code);
        }

        /// <inheritdoc/>
        public void SetPrice(int code, int count, decimal price)
        {
            if (count <= 0 || price <= 0)
            {
                throw new ArgumentException("Count and price should be positive!");
            }

            var specificPrices = GetModifiablePrices(code);
            if (specificPrices is null)
            {
                Prices.TryAdd(code, new ConcurrentDictionary<int, decimal>(new Dictionary<int, decimal>()
                {
                    { count, price }
                }));
            }
            else
            {
                specificPrices.AddOrUpdate(count, price, (key, existingValue) => price);
            }
        }

        private ConcurrentDictionary<int, decimal>? GetModifiablePrices(int code)
        {
            Prices.TryGetValue(code, out var result);
            return result;
        }
    }
}
