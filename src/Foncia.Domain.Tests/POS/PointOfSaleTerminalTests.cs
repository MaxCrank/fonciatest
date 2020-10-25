using Foncia.Domain.POS;
using Foncia.Domain.POS.Interfaces;
using System.Text;
using Xunit;

namespace Foncia.Domain.Tests.POS
{
    /// <summary>
    /// Decimal isn't a constant to use in <see cref="InlineData"/>, but in our case it's OK to pass double instead.
    /// </summary>
    public class PointOfSaleTerminalTests
    {
        private readonly IGoodsPriceManager _priceManager;
        private readonly ICodeScanner _scanner;
        private readonly IGoodsStoreManager _storeManager;
        private readonly IPaymentService _paymentService;
        private readonly IPointOfSaleTerminal _terminal;

        public PointOfSaleTerminalTests()
        {
            _scanner = GetCodeScanner();
            _priceManager = GetPriceManager(_scanner);
            _storeManager = GetStoreManager(_scanner);
            _paymentService = GetPaymentService();
            _terminal = GetTerminal();
        }

        [Theory]
        [InlineData("ABCDABA", 13.25)]
        [InlineData("CCCCCCC", 6)]
        [InlineData("ABCD", 7.25)]
        public void BasicTestCase(string goodsChars, decimal expectedValue)
        {
            foreach (var goodChar in goodsChars)
            {
                _terminal.ScanGood(GetStringData(goodChar.ToString()));
            }

            Assert.Equal(expectedValue, _terminal.CurrentTotalPrice);
        }

        [Theory]
        [InlineData("CCCCCCCCCC", 8.5)]
        [InlineData("CCCCCCCCCCC", 9.5)]
        [InlineData("CCCCCCCCCCCC", 10)]
        [InlineData("CCCCCCCCCCCCC", 11)]
        [InlineData("CCCCCCCCCCCCCCC", 12.5)]
        public void TestCaseWithMultipleVolumes(string goodsChars, decimal expectedValue)
        {
            _priceManager.SetPrice(_scanner.Scan(GetStringData("C")), 3, 2.5m);

            foreach (var goodChar in goodsChars)
            {
                _terminal.ScanGood(GetStringData(goodChar.ToString()));
            }

            Assert.Equal(expectedValue, _terminal.CurrentTotalPrice);
        }

        private IPointOfSaleTerminal GetTerminal()
        {
            return new PointOfSaleTerminal(
                _priceManager,
                _scanner,
                _storeManager,
                _paymentService
                );
        }

        private ICodeScanner GetCodeScanner()
        {
            return new CodeScanner();
        }

        private IPaymentService GetPaymentService()
        {
            return new PaymentService();
        }

        private IGoodsPriceManager GetPriceManager(ICodeScanner scanner)
        {
            var priceManager = new GoodsPriceManager();
            priceManager.SetPrice(scanner.Scan(GetStringData("A")), 1, 1.25m);
            priceManager.SetPrice(scanner.Scan(GetStringData("A")), 3, 3m);
            priceManager.SetPrice(scanner.Scan(GetStringData("B")), 1, 4.25m);
            priceManager.SetPrice(scanner.Scan(GetStringData("C")), 1, 1m);
            priceManager.SetPrice(scanner.Scan(GetStringData("C")), 6, 5m);
            priceManager.SetPrice(scanner.Scan(GetStringData("D")), 1, 0.75m);
            return priceManager;
        }

        private IGoodsStoreManager GetStoreManager(ICodeScanner scanner)
        {
            var storeManager = new GoodsStoreManager();
            storeManager.Add(scanner.Scan(GetStringData("A")), 4);
            storeManager.Add(scanner.Scan(GetStringData("B")), 3);
            storeManager.Add(scanner.Scan(GetStringData("C")), 9);
            storeManager.Add(scanner.Scan(GetStringData("D")), 2);
            return storeManager;
        }

        private byte[] GetStringData(string data)
        {
            return Encoding.Unicode.GetBytes(data);
        }
    }
}
