using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.Store;
using WPFCustomMessageBox;
using static System.Net.Mime.MediaTypeNames;

namespace travel_app.MVVM.ViewModel
{
    internal class EditTravelViewModel : ObservableObject
    {
        public RelayCommand SaveCommand { get; set; }
        public ICommand BackCommand { get; }
        public Travel currentTravel { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string Price { get; set; }
        public string Date { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public List<Attractions> Attractions { get; set; }
        public List<Hotels> Hotels { get; set; }
        public List<Restaurants> Restaurants { get; set; }
        public NavigationStore NavigationStore { get; }

        public EditTravelViewModel(NavigationStore navigationStore, Travel travel)
        {
            
            NavigationStore = navigationStore;
            currentTravel = travel;
            Id = travel.Id;
            Name = travel.Name == null ? "Nedostaju podaci" : travel.Name;
            ShortDescription = travel.ShortDescription == null ? "Nedostaju podaci" : travel.ShortDescription;
            Description = travel.Description == null ? "Nedostaju podaci" : travel.Description;
            Image = travel.Image;
            Price = travel.Price.ToString();
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

            SaveCommand = new RelayCommand(SaveChanges);
            BackCommand = new NavigateCommand<DetailsViewModel>(navigationStore, () => new DetailsViewModel(navigationStore, currentTravel));
        }

        private bool ValidateData()
        {   
        
            if (Name.Length == 0)
            {
                CustomMessageBox.ShowOK("Niste uneli naziv putovanja.", "Greška", "U red");
                return false;
            }

            if (Start.Length == 0)
            {
                CustomMessageBox.ShowOK("Niste uneli polazište putovanja.", "Greška", "U redu");
                return false;
            }

            if (End.Length == 0)
            {
                CustomMessageBox.ShowOK("Niste uneli destinaciju putovanja.", "Greška", "U redu");
                return false;
            }

            if (ShortDescription.Length == 0)
            {
                CustomMessageBox.ShowOK("Niste uneli kraći opis putovanja.", "Greška", "U red");
                return false;
            }

            if (Description.Length == 0)
            {
                CustomMessageBox.ShowOK("Niste uneli duži opis putovanja.", "Greška", "U red");
                return false;
            }

            if (Price.Length == 0)
            {
                CustomMessageBox.ShowOK("Niste uneli cenu putovanja.", "Greška", "U red");
                return false;
            }

            if (!int.TryParse(Price, out int price)) {
                CustomMessageBox.ShowOK("Cena putovanja mora da bude broj.", "Greška", "U red");
                return false;
            }

            if (price < 0)
            {
                CustomMessageBox.ShowOK("Cena putovanja mora biti pozitivan ceo broj.", "Greška", "U red");
                return false;
            }

            DateTime? date = DateTime.Parse(Date) as DateTime?;
            if (date == null)
            {
                CustomMessageBox.ShowOK("Niste uneli datum putovanja.", "Greška", "U red");
                return false;
            }

            return true;

        }

        private void SaveChanges(object element)
        {
            if (ValidateData()) {
                var result = MessageBoxResult.Yes;
                if (!MainWindow.LogedInUser.Pro)
                {
                    result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da izmenite ovo putovanje?", "Izmena putovanja", "Da", "Ne");
                }

                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new TravelContext())
                    {
                        currentTravel = db.Travels.Include("Attractions").Include("Hotels").Include("Restaurants").Single(item => item.Id == Id);
                        currentTravel.Name = Name;
                        currentTravel.Description = Description;
                        currentTravel.ShortDescription = ShortDescription;
                        currentTravel.Price = int.Parse(Price);
                        currentTravel.Start = Start;
                        currentTravel.End = End;
                        db.SaveChanges();

                        CustomMessageBox.ShowOK("Izmena uspešno odrađena :)", "Izmena", "U redu");

                    }
                }
            }
            
        }
    }
}
