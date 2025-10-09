using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using Fido2NetLib;
using Fido2NetLib.Objects;
using MediatR;
using System.Runtime.Serialization;

namespace Application.Commands.Users.AddBiometrics
{
    public enum FixedPublicKeyCredentialType
    {
        [EnumMember(Value = "public-key")]
        PublicKey
    }
    public class RegisterBiometricsHandler(IFidoCredentialService fidoCredentialService,
        IFidoCredentialRepository fidoCredentialRepository,
        IAuditLogRepository auditLogRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RegisterFingerprintCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(RegisterFingerprintCommand request, CancellationToken cancellationToken)
        {
            var attestation = new AuthenticatorAttestationRawResponse
            {
                Id = Base64Url.Decode(request.RegistrationDto.Id),
                RawId = Base64Url.Decode(request.RegistrationDto.RawId),
                Response = new AuthenticatorAttestationRawResponse.ResponseData
                {
                    AttestationObject = Base64Url.Decode(request.RegistrationDto.Response.AttestationObject),
                    ClientDataJson = Base64Url.Decode(request.RegistrationDto.Response.ClientDataJSON)
                },
                Type = request.RegistrationDto.Type == "public-key"
        ? PublicKeyCredentialType.PublicKey
        : throw new ArgumentException($"Unsupported credential type: {request.RegistrationDto.Type}")
            };

            try
            {
                await unitOfWork.BeginTransactionAsync();
                var credential = await fidoCredentialService.RegisterCredentialAsync(attestation);
                await fidoCredentialRepository.AddAsync(credential);
                await unitOfWork.CommitTransactionAsync();
                if (credential is null)
                {
                    return Result<object>.Failure("Fingerprint registration failed: Credential is null");
                }
                await auditLogRepository.AddAsync(new AuditLog(
                    credential.UserId,
                    "Fingerprint Registration",
                    "User",
                    credential.Id,
                    $"Fingerprint registered successfully for user {credential.UserId}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                    ));
                return Result<object>.Success(credential);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                await auditLogRepository.AddAsync(new AuditLog(
                    Guid.Empty,  
                    "Fingerprint Registration Failed",
                    "User",
                    null,
                    $"Fingerprint registration failed: {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));
                return Result<object>.Failure("Fingerprint registration failed: " + ex.Message);
            }
            
        }
    }
}
