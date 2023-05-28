using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app.MVVM.Model
{
    public class RestaurantTypes
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
