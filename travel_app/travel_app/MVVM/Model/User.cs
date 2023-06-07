using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_app.MVVM.Model
{
    public class User : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }

        private string _email;
        public string Email {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Pro { get; set; }
        public List<Travel> Travels { get; } = new();
        public List<Sale> Sales { get; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User()
        {
        }

        public User(int id, string email, string password, string role, bool pro)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;
            Pro = pro;
        }
    }
}
