﻿using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.Store;

namespace travel_app.MVVM.ViewModel
{
    class DetailsViewModel:ObservableObject
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int Price { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public ICommand BackCommand { get; }
        public DetailsViewModel(NavigationStore navigationStore, Travel travel)
        {

            Name = travel.Name == null ? "Nedostaju podaci" : travel.Name;
            ShortDescription = travel.ShortDescription == null ? "Nedostaju podaci" : travel.ShortDescription;
            Description = travel.Description == null ? "Nedostaju podaci" : travel.Description;
            Image = travel.Image;
            Price = travel.Price;
            Start = travel.Start == null ? "Nedostaju podaci" : travel.Start;
            End = travel.End == null ? "Nedostaju podaci" : travel.End;
            BackCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));

        }

    }
}