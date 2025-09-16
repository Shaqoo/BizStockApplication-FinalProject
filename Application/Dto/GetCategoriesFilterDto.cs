namespace Application.Dto
{
    public class GetCategoriesFilter
    {
        public int? Depth { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
