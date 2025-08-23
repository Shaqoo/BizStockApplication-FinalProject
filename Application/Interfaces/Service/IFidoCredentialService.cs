using Domain.Entities;
using Fido2NetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IFidoCredentialService
    {
        Task<CredentialCreateOptions> GenerateRegistrationOptionsAsync(Guid userId);
        Task<FidoCredential> RegisterCredentialAsync(AuthenticatorAttestationRawResponse attestation);
        Task<AssertionOptions> GenerateLoginOptionsAsync(string userIdentifier);
        Task<Guid> VerifyAssertionAsync(AuthenticatorAssertionRawResponse assertion);

    }

}
