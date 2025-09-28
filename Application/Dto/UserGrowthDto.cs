namespace Application.Dto
{
    public record UserGrowthDto
    {
        public DateTime WeekStart { get; init; }
        public int UserCount { get; init; }  
    }

    public record UserGrowthFullDto 
    {
        public List<UserGrowthDto> UserGrowthDtos { get; init; } = []!;
        public int TotalUsers { get; init; }
    }

}
