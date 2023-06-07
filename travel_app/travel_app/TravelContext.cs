using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using travel_app.MVVM.Model;

namespace travel_app
{
    internal class TravelContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<AttractionTypes> AttractionTypes { get; set; }
        public DbSet<Attractions> Attractions { get; set; }
        public DbSet<Hotels> Hotels { get; set; }
        public DbSet<RestaurantTypes> RestaurantTypes { get; set; }
        public DbSet<Restaurants> Restaurants { get; set;}
        public DbSet<Sale> Sales { get; set; }
    }
}
