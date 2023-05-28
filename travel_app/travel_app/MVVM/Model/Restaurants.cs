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
        public string Name { get; set; }
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

        public Restaurants(int id, string name, string location, int restaurantTypesId)
        {
            Id = id;
            Name = name;
            Location = location;
            RestaurantTypesId = restaurantTypesId;
        }
    }
}
