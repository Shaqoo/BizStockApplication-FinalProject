using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class TwoFactorSetupDto
    {
        public string ManualEntryKey { get; set; } = string.Empty;
        public string QrCodeImageUrl { get; set; } = string.Empty;
        public IEnumerable<string> RecoveryCodes { get; set; } = [];
    }

}
