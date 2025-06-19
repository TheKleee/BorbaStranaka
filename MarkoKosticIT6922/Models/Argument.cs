using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MarkoKosticIT6922.Models
{
    public class Argument
    {
        public int ArgumentId { get; set; }
        public string Tekst { get; set; } = string.Empty;

        public string? GlasacId { get; set; }
        public Glasac? Glasac { get; set; }

        public int StrankaId { get; set; }
        public Stranka? Stranka { get; set; }
    }
}
