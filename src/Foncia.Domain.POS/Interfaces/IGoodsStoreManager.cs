namespace Foncia.Domain.POS.Interfaces
{
    /// <summary>
    /// Manages goods in store.
    /// </summary>
    public interface IGoodsStoreManager
    {
        /// <summary>
        /// Adds certain amount of goods by code.
        /// </summary>
        /// <param name="code">Goods code.</param>
        /// <param name="count">Goods count.</param>
        void Add(int code, int count);

        /// <summary>
        /// Receives confirmation that certain amount of goods were sold and updates the info in the system.
        /// </summary>
        /// <param name="code">Goods code.</param>
        /// <param name="count">Goods count.</param>
        void ConfirmSold(int code, int count);
    }
}
