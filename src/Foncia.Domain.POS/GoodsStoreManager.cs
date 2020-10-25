using Foncia.Domain.POS.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Foncia.Domain.POS
{
    /// <inheritdoc/>
    internal class GoodsStoreManager : IGoodsStoreManager
    {
        // it is assumed that store manager state would be modified in multi-threaded context
        private readonly ConcurrentDictionary<int, int> Count = new ConcurrentDictionary<int, int>();

        /// <inheritdoc/>
        public void Add(int code, int count)
        {
            Count.AddOrUpdate(code, count, (key, existingValue) => existingValue + count);
        }

        /// <inheritdoc/>
        public void ConfirmSold(int code, int count)
        {
            if (!Count.TryGetValue(code, out var currentValue) || currentValue - count < 0)
            {
                throw new ArgumentException($"Can't update good {code} with current count {currentValue} in store: " +
                    $"either the goods doen't exist in the system or are not enough.");
            }

            Count.TryUpdate(code, currentValue - count, currentValue);
        }
    }
}
