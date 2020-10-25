namespace Foncia.Domain.POS
{
    /// <summary>
    /// The result of scanning a single good by POS terminal.
    /// </summary>
    public class ScanGoodResult
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="code">Code of the good which was scanned.</param>
        /// <param name="goodPrice">How much do all goods with the same code cost after another item was added.</param>
        /// <param name="totalPrice">Total price in the check after new item was added.</param>
        public ScanGoodResult(int code, decimal goodPrice, decimal totalPrice)
        {
            Code = code;
            GoodPrice = goodPrice;
            TotalPrice = totalPrice;
        }

        /// <summary>
        /// Code of the good which was scanned.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// How much do all goods with the same code cost after another item was added.
        /// </summary>
        public decimal GoodPrice { get; }

        /// <summary>
        /// Total price in the check after new item was added.
        /// </summary>
        public decimal TotalPrice { get; }
    }
}
