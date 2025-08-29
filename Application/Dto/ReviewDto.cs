namespace Application.Dto
{
    public record ProductReviewDto(Guid Id,Guid ProductId, ReviewUserDto User,string comment,int Rating,DateTime ReviewedAt);
    public record ReviewUserDto(Guid Id, string Name, string ProfileImageUrl);
}
