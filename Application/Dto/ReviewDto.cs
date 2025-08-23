namespace Application.Dto
{
    public record ProductReviewDto(Guid Id,Guid ProductId,Guid UserId,string comment,int Rating,DateTime ReviewedAt);
     
}
