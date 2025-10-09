using Application.Dto;
using Fido2NetLib;
using Fido2NetLib.Objects;

namespace Application.Configurations
{
    public static class FidoMapper
    {
        public static AuthenticatorAssertionRawResponse ToFido2Assertion(this FingerprintLoginDto dto)
        {
            return new AuthenticatorAssertionRawResponse
            {
                Id = Base64Url.Decode(dto.Id),
                RawId = Base64Url.Decode(dto.RawId),
                Type = Enum.Parse<PublicKeyCredentialType>(dto.Type.Replace("-",""), true),
                Response = new AuthenticatorAssertionRawResponse.AssertionResponse
                {
                    AuthenticatorData = Base64Url.Decode(dto.Response.AuthenticatorData),
                    ClientDataJson = Base64Url.Decode(dto.Response.ClientDataJSON),
                    Signature = Base64Url.Decode(dto.Response.Signature),
                    UserHandle = string.IsNullOrEmpty(dto.Response.UserHandle)
                            ? null
                            : Base64Url.Decode(dto.Response.UserHandle)
                }

            };
        }
    }

    public static class Base64Url
    {
        public static byte[] Decode(string base64Url)
        {
            if (string.IsNullOrWhiteSpace(base64Url))
                return Array.Empty<byte>();

            string base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }

        public static string Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }


}
