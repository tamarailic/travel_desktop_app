using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app.MVVM.Model
{
    public class Travel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string ShortDescription { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Range(0, 1000000)]
        public int Price { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public List<Attractions> Attractions { get; } = new();
        public List<Hotels> Hotels { get; } = new();
        public List<Restaurants> Restaurants { get; } = new();

    }
}
