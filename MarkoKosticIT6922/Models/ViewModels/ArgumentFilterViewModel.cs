using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace MarkoKosticIT6922.Models.ViewModels
{
    public class ArgumentFilterViewModel
    {
        public PagedResult<Argument> Argumenti { get; set; }
        //public IEnumerable<Argument> Argumenti { get; set; }
        public SelectList StrankaSelektList { get; set; }
        public int? SelectedStrankaId { get; set; }
        public string Pretraga { get; set; }
        public string Sortiranje { get; set; }
    }
}
