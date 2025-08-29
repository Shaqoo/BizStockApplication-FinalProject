namespace Application.Dto
{
    public class RatingSummaryDto
    {
        public double Average { get; set; }     
        public int Total { get; set; }           
        public Dictionary<int, int> Breakdown { get; set; } = new(); 
    }
}
