namespace MarkoKosticIT6922.Models
{
    public class Glasanje
    {
        public int GlasanjeId { get; set; }

        public int BorbaId { get; set; }
        public Borba? Borba { get; set; }

        public string GlasacId { get; set; }
        public Glasac? Glasac { get; set; } 
    }
}
