using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.Store;
using static travel_app.MVVM.ViewModel.UserHomeViewModel;

namespace travel_app.MVVM.ViewModel
{
    class UserDetailsViewModel:ObservableObject
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int Price { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public ICommand BackCommand { get; }

        public UserDetailsViewModel(NavigationStore navigationStore, Travel travel) {

            Name = travel.Name == null ? "Nedostaju podaci" : travel.Name;
            ShortDescription = travel.ShortDescription == null ? "Nedostaju podaci" : travel.ShortDescription;
            Description = travel.Description == null ? "Nedostaju podaci" : travel.Description;
            Image = travel.Image;
            Price = travel.Price;
            Start = travel.Start == null ? "Nedostaju podaci" : travel.Start;
            End = travel.End == null ? "Nedostaju podaci" : travel.End;
            BackCommand = new NavigateCommand<UserHomeViewModel>(navigationStore, () => new UserHomeViewModel(navigationStore));
        }

    }
}
