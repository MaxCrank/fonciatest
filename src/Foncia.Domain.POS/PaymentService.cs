using Foncia.Domain.POS.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Foncia.Domain.POS
{
    /// <inheritdoc/>
    internal class PaymentService : IPaymentService
    {
        // it is assumed that payment service state would be modified in multi-threaded context
        private readonly ConcurrentDictionary<string, decimal> _transactions 
            = new ConcurrentDictionary<string, decimal>();

        /// <inheritdoc/>
        public void Finalize(string transactionCode)
        {
            if (!_transactions.ContainsKey(transactionCode))
            {
                throw new ArgumentException($"Transaction code {transactionCode} was not initiated before.");
            }

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            _transactions.TryRemove(transactionCode, out var price);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        /// <inheritdoc/>
        public string Initiate(decimal amount)
        {
            var id = Guid.NewGuid().ToString();
            _transactions.TryAdd(id, amount);
            return id;
        }
    }
}
