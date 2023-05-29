using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app.MVVM.Model
{
    public class Hotels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public int? Stars { get; set; }
        public List<Travel> Travels { get; } = new();

        public Hotels()
        {
        }

        public Hotels(string name, string location, int? stars)
        {
            Name = name;
            Location = location;
            Stars = stars;
        }

        public Hotels(int id, string name, string location, int? stars)
        {
            Id = id;
            Name = name;
            Location = location;
            Stars = stars;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return $"{Name}";
        }
    }
}
