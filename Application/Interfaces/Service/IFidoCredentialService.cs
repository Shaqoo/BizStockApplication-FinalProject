using Domain.Entities;
using Fido2NetLib;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces.Service
{
    public interface IFidoCredentialService
    {
        Task<JsonResult> GenerateRegistrationOptionsAsync(Guid userId);
        Task<FidoCredential> RegisterCredentialAsync(AuthenticatorAttestationRawResponse attestation);
        Task<AssertionOptions> GenerateLoginOptionsAsync(string userIdentifier);
        Task<Guid> VerifyAssertionAsync(AuthenticatorAssertionRawResponse assertion);

    }

}
