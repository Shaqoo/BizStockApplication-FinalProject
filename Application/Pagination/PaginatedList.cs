namespace Application.Pagination
{
    public record PaginatedList<T>
        where T : class
    {
        public IReadOnlyCollection<T> Items { get; init; } = new List<T>();
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public PaginatedList()
        {

        }
        public PaginatedList(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
