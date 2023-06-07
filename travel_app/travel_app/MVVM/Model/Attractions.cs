using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app.MVVM.Model
{
    public class Attractions
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public int AttractionTypesId { get; set; }
        public AttractionTypes AttractionTypes { get; set; }
        public List<Travel> Travels { get; } = new();

        public Attractions()
        {
        }
        public Attractions(string name, string address, int attractionTypesId)
        {
            Name = name;
            Address = address;
            AttractionTypesId = attractionTypesId;
        }
        public Attractions(string name, string address, AttractionTypes attractionTypes)
        {
            Name = name;
            Address = address;
            AttractionTypes = attractionTypes;
        }
        public Attractions(int id, string name, string address, int attractionTypesId)
        {
            Id = id;
            Name = name;
            Address = address;
            AttractionTypesId = attractionTypesId;
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
