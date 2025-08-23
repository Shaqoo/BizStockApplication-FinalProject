using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    namespace Application.Common.Interfaces
    {
        public interface IRecoveryCodeGenerator
        {
            /// <summary>
            /// Generates a collection of unique recovery codes.
            /// </summary>
            /// <param name="count">Number of codes to generate.</param>
            /// <returns>A collection of recovery codes.</returns>
            IReadOnlyCollection<string> Generate(int count);
        }
    }

}
