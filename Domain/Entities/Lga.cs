namespace Domain.Entities
{
    public class Lga
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public State State { get; set; } = default!;
        public int StateId { get; set; }
    }
}
