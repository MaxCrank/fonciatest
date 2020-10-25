namespace Foncia.Domain.POS.Interfaces
{
    /// <summary>
    /// Dummy payment service; won't be implemented directly.
    /// Some payment services allow you to finalize transactions, and it's better to use them this way.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Initiates the transaction to ensure it can be done.
        /// Client identification omitted for brievity.
        /// </summary>
        /// <param name="amount">How much money do we want to transfer.</param>
        /// <returns>Transaction code.</returns>
        string Initiate(decimal amount);

        /// <summary>
        /// Finalizes the transaction code when initiator is fine to proceed, too.
        /// </summary>
        /// <param name="transactionCode">Transaction code.</param>
        void Finalize(string transactionCode);
    }
}
