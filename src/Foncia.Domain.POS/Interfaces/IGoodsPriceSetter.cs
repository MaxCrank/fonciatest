namespace Foncia.Domain.POS.Interfaces
{
    /// <summary>
    /// Goods price setter.
    /// </summary>
    public interface IGoodsPriceSetter
    {
        /// <summary>
        /// Sets the price for certain amount of goods.
        /// </summary>
        /// <param name="code">Goods code.</param>
        /// <param name="count">Goods count.</param>
        /// <param name="price">Goods price for the specified amount.</param>
        void SetPrice(int code, int count, decimal price);
    }
}
