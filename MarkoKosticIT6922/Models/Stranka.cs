namespace MarkoKosticIT6922.Models
{
    public class Stranka
    {
        public int StrankaId { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;

        public ICollection<Argument> Argumenti { get; set; } = new List<Argument>();
    }
}
