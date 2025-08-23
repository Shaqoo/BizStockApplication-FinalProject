using Application.Dto;
using Fido2NetLib;
using Fido2NetLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configurations
{
    public static class FidoMapper
    {
        public static AuthenticatorAssertionRawResponse ToFido2Assertion(this FingerprintLoginDto dto)
        {
            return new AuthenticatorAssertionRawResponse
            {
                Id = Convert.FromBase64String(dto.Id),
                RawId = Convert.FromBase64String(dto.RawId),
                Type = Enum.Parse<PublicKeyCredentialType>(dto.Type, true),
                Response = new AuthenticatorAssertionRawResponse.AssertionResponse
                {
                    AuthenticatorData = Convert.FromBase64String(dto.Response.AuthenticatorData),
                    ClientDataJson = Convert.FromBase64String(dto.Response.ClientDataJSON),
                    Signature = Convert.FromBase64String(dto.Response.Signature),
                    UserHandle = string.IsNullOrEmpty(dto.Response.UserHandle)
                        ? null
                        : Convert.FromBase64String(dto.Response.UserHandle)
                }
            };
        }
    }

}
