namespace Application.Dto
{
    public record PoTrendDto
    {
        public IEnumerable<string> Labels { get; set; } = [];
        public IEnumerable<int> Data { get; set; } = [];
    }

}
