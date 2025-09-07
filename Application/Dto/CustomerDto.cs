namespace Application.Dto
{
    public record CustomerDto(Guid Id,string Fullname,string CustomerType,string? TaxId,string? State,string? Address,
        string? BusinessName);
}
