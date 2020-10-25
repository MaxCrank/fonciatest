using Foncia.Domain.POS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foncia.Domain.POS
{
    /// <inheritdoc/>
    internal class PointOfSaleTerminal : IPointOfSaleTerminal
    {
        private readonly IGoodsPriceManager _priceManager;
        private readonly ICodeScanner _scanner;
        private readonly IGoodsStoreManager _storeManager;
        private readonly IPaymentService _paymentService;

        // each terminal is assumed to be used in a non-concurrent environment
        private readonly Dictionary<int, int> _currentGoodsCount = new Dictionary<int, int>();
        private readonly Dictionary<int, decimal> _currentGoodsPrices = new Dictionary<int, decimal>();
        private readonly Dictionary<int, int> _soldGoodsCount = new Dictionary<int, int>();

        /// <inheritdoc/>
        public decimal CurrentTotalPrice => _currentGoodsPrices.Values.Sum();

        public PointOfSaleTerminal(IGoodsPriceManager priceManager, ICodeScanner scanner, IGoodsStoreManager storeManager,
            IPaymentService paymentService)
        {
            _priceManager = priceManager;
            _scanner = scanner;
            _storeManager = storeManager;
            _paymentService = paymentService;
        }

        /// <inheritdoc/>
        public ScanGoodResult ScanGood(byte[] imageData)
        {
            var code = _scanner.Scan(imageData);
            var prices = _priceManager.GetPrices(code);
            if (prices is null || prices.Count == 0)
            {
                throw new Exception($"Prices are not found for good with code {code}");
            }

            int newCount = 1;
            if (_currentGoodsCount.ContainsKey(code))
            {
                newCount += _currentGoodsCount[code];
                _currentGoodsCount[code] = newCount;
            }
            else
            {
                _currentGoodsCount.Add(code, newCount);
            }

            var newGoodPrice = CalculateGoodPrice(newCount, prices);
            if (!_currentGoodsPrices.ContainsKey(code))
            {
                _currentGoodsPrices.Add(code, newGoodPrice);
            }
            else
            {
                _currentGoodsPrices[code] = newGoodPrice;
            }

            return new ScanGoodResult(code, newGoodPrice, CurrentTotalPrice);
        }

        /// <inheritdoc/>
        public void SetPrice(int code, int count, decimal price)
        {
            _priceManager.SetPrice(code, count, price);
        }

        /// <inheritdoc/>
        public void CancelCurrentGoods()
        {
            Reset();
        }

        /// <inheritdoc/>
        public void SaleCurrentGoods()
        {
            try
            {
                var transactionCode = _paymentService.Initiate(CurrentTotalPrice);

                foreach (var good in _currentGoodsCount)
                {
                    _storeManager.ConfirmSold(good.Key, good.Value);
                    _soldGoodsCount.Add(good.Key, good.Value);
                }

                _paymentService.Finalize(transactionCode);

                Reset();
            }
            catch
            {
                // rollback on failure
                foreach (var good in _soldGoodsCount)
                {
                    _storeManager.Add(good.Key, good.Value);
                }

                Reset();

                throw;
            }
        }

        private void Reset()
        {
            _currentGoodsCount.Clear();
            _currentGoodsPrices.Clear();
            _soldGoodsCount.Clear();
        }

        private decimal CalculateGoodPrice(int count, IReadOnlyDictionary<int, decimal> prices)
        {
            var applicableCounts = prices.Where(p => p.Key <= count).Select(p => p.Key).ToList();
            if (applicableCounts.Count == 1 && applicableCounts[0] == 1)
            {
                return count * prices[applicableCounts[0]];
            }

            applicableCounts.Sort();
            applicableCounts.Reverse();
            Dictionary<int, int> chunks = new Dictionary<int, int>();
            foreach (int applicableCount in applicableCounts)
            {
                var chunkSize = count / applicableCount;
                chunks.Add(applicableCount, chunkSize);
                count -= chunkSize * applicableCount;
            }

            return chunks.Select(ch => prices[ch.Key] * ch.Value).Sum();
        }
    }
}
