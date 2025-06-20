using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MarkoKosticIT6922.Models
{
    public class Glasanje
    {
        public int GlasanjeId { get; set; }

        public int BorbaId { get; set; }
        public Borba? Borba { get; set; }

        [AllowNull]
        public string GlasacId { get; set; }
        public Glasac? Glasac { get; set; } 
    }
}
