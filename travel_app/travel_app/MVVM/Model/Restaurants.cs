using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app.MVVM.Model
{
    public class Restaurants
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public int RestaurantTypesId { get; set; }
        public virtual RestaurantTypes RestaurantTypes { get; set; }

        public Restaurants()
        {
        }

        public Restaurants(string name, string location, int restaurantTypesId)
        {
            Name = name;
            Location = location;
            RestaurantTypesId = restaurantTypesId;
        }
        public Restaurants(string name, string location, RestaurantTypes restaurantTypes)
        {
            Name = name;
            Location = location;
            RestaurantTypes = restaurantTypes;
        }
        public Restaurants(int id, string name, string location, int restaurantTypesId)
        {
            Id = id;
            Name = name;
            Location = location;
            RestaurantTypesId = restaurantTypesId;
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
