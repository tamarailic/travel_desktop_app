using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.Store;
using static travel_app.MVVM.ViewModel.UserHomeViewModel;

namespace travel_app.MVVM.ViewModel
{
    internal class UserTravelViewModel:ObservableObject
    {
        public NavigationStore NavigationStore { get; }
        public UserTravelViewModel(NavigationStore navigationStore)
        {
            NavigationStore = navigationStore;
        }

        public List<TravelCard> AllTravels
        {
            get
            {
                using (var db = new TravelContext())
                {

                    User currentUser = db.Users.Include("Travels").Single(el => el.Id == UserMainWindow.LogedInUser.Id);
                    List<TravelCard> travels = new List<TravelCard>();
                    currentUser.Travels.ForEach(t => travels.Add(new TravelCard(t, NavigationStore)));
                    return travels;
                }

            }
        }
    }
}
