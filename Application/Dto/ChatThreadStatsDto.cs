namespace Application.Dto
{
    public record ChatThreadStatsDto
    {
        public int OpenThreads { get; set; }
        public int InProgressThreads { get; set; }
        public int ClosedThreads { get; set; }
        public int TotalThreads { get; set; }
    }

}
