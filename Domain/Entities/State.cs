namespace Domain.Entities
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Lga> Lgas { get; set; } = new HashSet<Lga>();
    }
}
