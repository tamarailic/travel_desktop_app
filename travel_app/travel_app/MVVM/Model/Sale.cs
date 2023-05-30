using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app.MVVM.Model
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; }
        public int TravelId { get; set; }
        public Travel Travel { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public Sale()
        {
        }

        public Sale(DateTime dateTime, string type, int travelId, int userId)
        {
            DateTime = dateTime;
            Type = type;
            TravelId = travelId;
            UserId = userId;
        }

        public Sale(DateTime dateTime, string type, int travelId, Travel travel, int userId, User user)
        {
            DateTime = dateTime;
            Type = type;
            TravelId = travelId;
            Travel = travel;
            UserId = userId;
            User = user;
        }

        public Sale(int id, DateTime dateTime, string type, int travelId, Travel travel, int userId, User user)
        {
            Id = id;
            DateTime = dateTime;
            Type = type;
            TravelId = travelId;
            Travel = travel;
            UserId = userId;
            User = user;
        }
    }
}
