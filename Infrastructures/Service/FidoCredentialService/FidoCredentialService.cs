using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;


namespace Infrastructures.Service.FidoCredentialService
{
    public class FidoCredentialService : IFidoCredentialService
    {
        private readonly Fido2 _fido2;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFidoCredentialRepository _credentialRepository;
        private readonly IUserRepository _userRepository;

        public FidoCredentialService(
            Fido2 fido2,
            IHttpContextAccessor httpContextAccessor,
            IFidoCredentialRepository credentialRepository,
            IUserRepository userRepository)
        {
            _fido2 = fido2;
            _httpContextAccessor = httpContextAccessor;
            _credentialRepository = credentialRepository;
            _userRepository = userRepository;
        }



        public async Task<JsonResult> GenerateRegistrationOptionsAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            var fidoUser = new Fido2User
            {
                DisplayName = user.FullName,
                Name = (string)user.Email,
                Id = user.Id.ToByteArray()
            };

            var existingCredentials = await _credentialRepository.GetByUserIdAsync(userId);

            var excludeCredentials = existingCredentials.Select(c => new PublicKeyCredentialDescriptor
            {
                Id = Base64Url.Decode(c.CredentialId),
                Type = PublicKeyCredentialType.PublicKey
            }).ToList();

            var options = _fido2.RequestNewCredential(
                user: fidoUser,
                excludeCredentials: excludeCredentials,
                authenticatorSelection: new AuthenticatorSelection
                {
                    AuthenticatorAttachment = AuthenticatorAttachment.Platform,
                    RequireResidentKey = false,
                    UserVerification = UserVerificationRequirement.Required
                },
                AttestationConveyancePreference.None
            );

            var session = _httpContextAccessor.HttpContext?.Session;

           
            session?.SetString("fido_challenge", Base64Url.Encode(options.Challenge));

            var optionsJson = options.ToJson();
            session?.SetString("fido_options", optionsJson);

            return new JsonResult(options);
        }

        public async Task<FidoCredential> RegisterCredentialAsync(AuthenticatorAttestationRawResponse attestation)
        {
            var session = _httpContextAccessor.HttpContext?.Session
                          ?? throw new Exception("No HTTP session");

            var challenge = session.GetString("fido_challenge")
                           ?? throw new Exception("Challenge missing");

            var optionsJson = session.GetString("fido_options")
                             ?? throw new Exception("Options missing");

            var options = CredentialCreateOptions.FromJson(optionsJson);

            var result = await _fido2.MakeNewCredentialAsync(attestation, options, async (args, ct) =>
            {
                string credentialId = Base64Url.Encode(args.CredentialId);
                var credential = await _credentialRepository.GetByCredentialIdAsync(credentialId);
                return credential == null;
            });

            var userId = new Guid(result.Result.User.Id);

            return new FidoCredential(
                userId,
                Base64Url.Encode(result.Result.CredentialId),
                Base64Url.Encode(result.Result.PublicKey),
                result.Result.Aaguid,
                result.Result.Counter
            );
        }


        public async Task<AssertionOptions> GenerateLoginOptionsAsync(string userIdentifier)
        {
            var user = await _userRepository.GetByEmailAsync(userIdentifier)
                       ?? throw new Exception("User not found");

            var credentials = await _credentialRepository.GetByUserIdAsync(user.Id);

            var descriptors = credentials.Select(c => new PublicKeyCredentialDescriptor
            {
                Id = Base64Url.Decode(c.CredentialId),
                Type = PublicKeyCredentialType.PublicKey
            }).ToList();

            var options = _fido2.GetAssertionOptions(
                descriptors,
                UserVerificationRequirement.Required
            );

            var session = _httpContextAccessor.HttpContext?.Session;

            session?.SetString("fido_login_challenge", Base64Url.Encode(options.Challenge));
            session?.SetString("assertion_options", JsonSerializer.Serialize(options));

            return options;
        }


        public async Task<Guid> VerifyAssertionAsync(AuthenticatorAssertionRawResponse assertion)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            var optionsJson = session?.GetString("assertion_options")
                              ?? throw new Exception("Missing assertion options");

            var options = JsonSerializer.Deserialize<AssertionOptions>(optionsJson)
                          ?? throw new Exception("Failed to deserialize assertion options");

            var creds = await _credentialRepository.GetByCredentialIdAsync(assertion.Id.ToString())
    ?? throw new Exception("Unknown credential");

            var storedPublicKey = Base64Url.Decode(creds.PublicKey);
            var storedSignatureCounter = creds.SignatureCounter;

            var result = await _fido2.MakeAssertionAsync(
                assertion,
                options,
                storedPublicKey,
                storedSignatureCounter,
                async (args,cr) =>
                {
                    return true;
                });


            
            creds.UpdateSignatureCounter(result.Counter);
            await _credentialRepository.UpdateFidoCredentialAsync(creds);

            return creds.UserId;
        }


    }

}
