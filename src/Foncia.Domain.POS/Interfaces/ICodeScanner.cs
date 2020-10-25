using System.Threading.Tasks;

namespace Foncia.Domain.POS.Interfaces
{
    /// <summary>
    /// Code scanner for goods.
    /// </summary>
    public interface ICodeScanner
    {
        /// <summary>
        /// Scans the image to recognize the code.
        /// </summary>
        /// <param name="imageData">Image data.</param>
        /// <returns>Integer code of the good.</returns>
        int Scan(byte[] imageData);
    }
}
