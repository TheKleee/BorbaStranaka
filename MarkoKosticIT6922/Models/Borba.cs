namespace MarkoKosticIT6922.Models
{
    public class Borba
    {
        public int BorbaId { get; set; }

        public int Stranka1Id { get; set; }
        public Stranka? Stranka1 { get; set; }

        public int Stranka2Id { get; set; }
        public Stranka? Stranka2 { get; set; }

        public int? PobednikId { get; set; }
        public Stranka? Pobednik { get; set; }

        public ICollection<Glasanje> Glasanja { get; set; } = new List<Glasanje>();
    }
}
