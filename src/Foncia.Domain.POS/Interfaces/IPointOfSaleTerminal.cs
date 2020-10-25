using System.Threading.Tasks;

namespace Foncia.Domain.POS.Interfaces
{
    /// <summary>
    /// Point of sale terminal.
    /// </summary>
    public interface IPointOfSaleTerminal : IGoodsPriceSetter
    {
        /// <summary>
        /// Total price for the current goods.
        /// </summary>
        decimal CurrentTotalPrice { get; }

        /// <summary>
        /// Scans goods code and adds an item.
        /// </summary>
        /// <param name="imageData">Image with the good code.</param>
        /// <returns>How much the specific good costs in total, as well as total price for all current goods.</returns>
        ScanGoodResult ScanGood(byte[] imageData);

        /// <summary>
        /// Cancels current goods processing and resets the contextual data.
        /// </summary>
        void CancelCurrentGoods();

        /// <summary>
        /// Performs the sale transaction of current goods and resets the contextual data.
        /// </summary>
        void SaleCurrentGoods();
    }
}
