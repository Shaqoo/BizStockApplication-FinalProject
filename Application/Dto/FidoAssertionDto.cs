using Fido2NetLib;
using Fido2NetLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class AttestationResponseDto
    {
        public string AttestationObject { get; set; } = default!;
        public string ClientDataJSON { get; set; } = default!;
    }
    public class FingerprintRegistrationDto
    {
        public string Id { get; set; } = default!;
        public string RawId { get; set; } = default!;       
        public string Type { get; set; } = default!;           
        public AttestationResponseDto Response { get; set; } = default!;

    }

    public class FingerprintLoginDto
    {
        public string Id { get; set; } = default!;
        public string RawId { get; set; } = default!;
        public string Type { get; set; } = default!;
        public AssertionResponseDto Response { get; set; } = default!;
    }

    public class AssertionResponseDto
    {
        public string ClientDataJSON { get; set; } = default!;
        public string AuthenticatorData { get; set; } = default!;
        public string Signature { get; set; } = default!;
        public string? UserHandle { get; set; } = default!;
    }

}
