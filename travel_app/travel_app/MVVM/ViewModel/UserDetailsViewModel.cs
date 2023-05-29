using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using travel_app.Core;
using travel_app.Store;
using static travel_app.MVVM.ViewModel.UserHomeViewModel;

namespace travel_app.MVVM.ViewModel
{
    class UserDetailsViewModel:ObservableObject
    {
 
        public TravelCard TravelCard { get; set; }

        public UserDetailsViewModel(NavigationStore navigationStore, TravelCard travel) { 
            
            TravelCard = travel;
        }
    }
}
