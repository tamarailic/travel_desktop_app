using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.Store;
using WPFCustomMessageBox;

namespace travel_app.MVVM.ViewModel
{
    class DetailsViewModel:ObservableObject
    {
        public RelayCommand DeleteCommand { get; set; }
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
        public ICommand EditCommand { get; set; }

        public DetailsViewModel(NavigationStore navigationStore, Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name == null ? "Nedostaju podaci" : travel.Name;
            ShortDescription = travel.ShortDescription == null ? "Nedostaju podaci" : travel.ShortDescription;
            Description = travel.Description == null ? "Nedostaju podaci" : travel.Description;
            Image = travel.Image == null ? File.ReadAllBytes("D:/Fakultet/Treca_godina/HCI/Projekat/travel_desktop_app/travel_app/travel_app/images/putokazi_logo.png") : travel.Image;
            Price = travel.Price;
            Date = travel.Date.Split("T")[0];
            Start = travel.Start == null ? "Nedostaju podaci" : travel.Start;
            End = travel.End == null ? "Nedostaju podaci" : travel.End;

            using (var db = new TravelContext())
            {
                Travel t = db.Travels.Include("Attractions").Include("Hotels").Include("Restaurants").Single(tr => tr.Id == Id);
                Attractions = t.Attractions;
                Restaurants = t.Restaurants;
                Hotels = t.Hotels;
            }

            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new NavigateCommand<EditTravelViewModel>(navigationStore, () => new EditTravelViewModel(navigationStore, travel));
            BackCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
        }

        private void Delete(object element)
        {
            var result = MessageBoxResult.Yes;
            if (!MainWindow.LogedInUser.Pro)
            {
                result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da otkažete ovo putovanje?", "Otkazivanje","Da", "Ne");
            }

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new TravelContext())
                {
                    var currentTravel = db.Travels.Find(Id);
                    if (currentTravel.Canceled)
                    {
                        CustomMessageBox.ShowOK($"Putovanje je već otkazano", "Neuspelo brisanje", "U redu");
                        return;
                    }
                    currentTravel.Canceled = true;
                    db.SaveChanges();
                    CustomMessageBox.ShowOK("Uspešno otkazivanje putovanja.", "Otkazivanje", "U redu");
                }
            }
        }
    }
}
