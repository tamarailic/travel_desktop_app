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
    internal class UserTravelViewModel:ObservableObject
    {
        public NavigationStore NavigationStore { get; }
        public UserTravelViewModel(NavigationStore navigationStore)
        {
            NavigationStore = navigationStore;
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
    }
}
