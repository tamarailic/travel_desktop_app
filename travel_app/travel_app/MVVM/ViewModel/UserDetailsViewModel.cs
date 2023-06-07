using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.Store;
using WPFCustomMessageBox;

namespace travel_app.MVVM.ViewModel
{
    class UserDetailsViewModel:ObservableObject
    {
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand BuyCommand { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int Price { get; set; }
        public string Date { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public List<Attractions> Attractions { get; set; }
        public List<Hotels> Hotels { get; set; }
        public List<Restaurants> Restaurants { get; set; }
        public ICommand BackCommand { get; }

        public UserDetailsViewModel(NavigationStore navigationStore, Travel travel) {
            Id = travel.Id;
            Name = travel.Name == null ? "Nedostaju podaci" : travel.Name;
            ShortDescription = travel.ShortDescription == null ? "Nedostaju podaci" : travel.ShortDescription;
            Description = travel.Description == null ? "Nedostaju podaci" : travel.Description;
            Image = travel.Image == null ? File.ReadAllBytes("D:/Fakultet/Treca_godina/HCI/Projekat/travel_desktop_app/travel_app/travel_app/images/putokazi_logo.png") : travel.Image;
            Price = travel.Price;
            Date = travel.Date.Split("T")[0];
            Start = travel.Start == null ? "Nedostaju podaci" : travel.Start;
            End = travel.End == null ? "Nedostaju podaci" : travel.End;

            using(var db = new TravelContext())
            {
                Travel t = db.Travels.Include("Attractions").Include("Hotels").Include("Restaurants").Single(tr => tr.Id == Id);
                Attractions = t.Attractions;
                Restaurants = t.Restaurants;
                Hotels = t.Hotels;
            }

            BackCommand = new NavigateCommand<UserHomeViewModel>(navigationStore, () => new UserHomeViewModel(navigationStore));

            ReserveCommand = new RelayCommand(Reserve);
            BuyCommand = new RelayCommand(Buy);
        }

        private void Reserve(object element)
        {
            var result = CustomMessageBox.ShowYesNo("Želite da rezervišete ovo putovanje?", "Rezervacija", "Da", "Ne");

            if (result == MessageBoxResult.Yes)
            {
                using(var db = new TravelContext())
                {
                    if (TravelAlreadyBookedOrPurchased())
                    {
                        CustomMessageBox.ShowOK($"Hey, već ste rezervisali ovo putavanje. Vidimo se {Date} na adresi {Start} ;)", "Neuspela rezervacija", "U redu");
                        return;
                    }
                    
                    User currentUser = db.Users.Include("Travels").Single(el => el.Id == UserMainWindow.LogedInUser.Id);
                    currentUser.Travels.Add(db.Travels.Find(Id));
                    db.Sales.Add(new Sale(DateTime.Now, "reservation", Id, UserMainWindow.LogedInUser.Id));
                    db.SaveChanges();
                    CustomMessageBox.ShowOK("Uspešna rezervacija.", "Rezervacija","U redu");
                }
            }
        }

        private void Buy(object element)
        {
            var result = CustomMessageBox.ShowYesNo("Želite da kupite ovo putovanje?", "Kupovina", "Da", "Ne");

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new TravelContext())
                {
                    if (TravelAlreadyBookedOrPurchased())
                    {
                        CustomMessageBox.ShowOK($"Hey, već ste kupili ovo putavanje. Vidimo se {Date} na adresi {Start} :)", "Neuspela rezervacija", "U redu");
                        return;
                    }
                    User currentUser = db.Users.Include("Travels").Single(el => el.Id == UserMainWindow.LogedInUser.Id);
                    currentUser.Travels.Add(db.Travels.Find(Id));
                    db.Sales.Add(new Sale(DateTime.Now, "sell", Id, UserMainWindow.LogedInUser.Id));
                    db.SaveChanges();
                    CustomMessageBox.ShowOK("Uspešna kupovina.", "Kupovina", "U redu");
                }
            }
        }

        private bool TravelAlreadyBookedOrPurchased()
        {
            using(var db = new TravelContext())
            {
                User currentUser = db.Users.Include("Travels").Single(el => el.Id == UserMainWindow.LogedInUser.Id);
                return currentUser.Travels.Contains(db.Travels.Find(Id));
            }
        }
    }
}
