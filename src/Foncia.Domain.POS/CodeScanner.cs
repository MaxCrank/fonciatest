using Foncia.Domain.POS.Interfaces;
using System;
using System.Text;

namespace Foncia.Domain.POS
{
    /// <inheritdoc/>
    internal class CodeScanner : ICodeScanner
    {
        /// <inheritdoc/>
        public int Scan(byte[] imageData)
        {
            if (imageData is null || imageData.Length == 0)
            {
                throw new ArgumentException("Can't scan code from null or empty image data");
            }

            var stringData = Encoding.UTF8.GetString(imageData);
            return stringData.GetHashCode();
        }
    }
}
