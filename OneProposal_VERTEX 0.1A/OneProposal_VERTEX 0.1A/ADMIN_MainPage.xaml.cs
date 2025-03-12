using Firebase.Database;
using System.Collections.ObjectModel;

namespace OneProposal_VERTEX_0._1A {
    public partial class ADMIN_MainPage : ContentPage {

        private readonly FirebaseClient firebase = new FirebaseClient("https://oneproposal-onedev-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public string club = "CoPs";
        public ObservableCollection<ProposalDetails> ActivityList { get; set; } = new ObservableCollection<ProposalDetails>();

        public ADMIN_MainPage() {
            InitializeComponent();
            BindingContext = this;
            LoadData();
        }

        private async void IntentMainPage(object sender, EventArgs e) {
            await Shell.Current.GoToAsync("..");
        }
        private async void LoadData() {
            var activities = await firebase.Child("ActivityProposal_tbl")
                                           .OnceAsync<ProposalDetails>();
            ActivityList.Clear();
            foreach (var item in activities) {
                ActivityList.Add(item.Object);
            }
        }

        private async void OnActivityTapped(object sender, EventArgs e) {
            if (sender is Frame frame && frame.BindingContext is ProposalDetails selectedActivity) {
                await Navigation.PushAsync(new ActivityDetailPage(selectedActivity));
            }
        }

        public class ProposalDetails {
            public string Title { get; set; }
            public string Club { get; set; }
            public string Date { get; set; }
            public string Status { get; set; }
            public string Rationale { get; set; }
            public string Objectives { get; set; }
            public string TypeOfActivity { get; set; }
            public string Venue { get; set; }
            public string Reach { get; set; }
        }
    }
}