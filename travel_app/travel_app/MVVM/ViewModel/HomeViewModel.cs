using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.MVVM.View;
using travel_app.Services;
using travel_app.Store;
using System.Diagnostics;
using static travel_app.MVVM.ViewModel.UserHomeViewModel;
using System.IO;

namespace travel_app.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public NavigationStore NavigationStore { get; }
        public ICommand CreateNewCommand { get; }
        public ICommand DetailsCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigationStore = navigationStore;
            CreateNewCommand = new NavigateCommand<CreateNewTravelViewModel>(navigationStore, () => new CreateNewTravelViewModel(navigationStore));
        }

        public List<TravelCard> Travels
        {
            get
            {
                using (var db = new TravelContext())
                {
                    List<TravelCard> travels = new List<TravelCard>();
                    db.Travels.ToList().ForEach(t => travels.Add(new TravelCard(t, NavigationStore)));
                    return travels;
                }
            }
        }

        public class TravelCard
        {
            public string Name { get; set; }
            public string ShortDescription { get; set; }
            public string Description { get; set; }
            public byte[] Image { get; set; }
            public int Price { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public ICommand Command { get; }

            public TravelCard(Travel travel, NavigationStore navigationStore)
            {
                Name = travel.Name == null ? "Nedostaju podaci" : travel.Name;
                ShortDescription = travel.ShortDescription == null ? "Nedostaju podaci" : travel.ShortDescription;
                Description = travel.Description == null ? "Nedostaju podaci" : travel.Description;
                Image = travel.Image == null ? File.ReadAllBytes("D:/Fakultet/Treca_godina/HCI/Projekat/travel_desktop_app/travel_app/travel_app/images/putokazi_logo.png") : travel.Image;
                Price = travel.Price;
                Start = travel.Start == null ? "Nedostaju podaci" : travel.Start;
                End = travel.End == null ? "Nedostaju podaci" : travel.End;
                Command = new NavigateCommand<DetailsViewModel>(navigationStore, () => new DetailsViewModel(navigationStore, travel));
            }
        }
    }
}
