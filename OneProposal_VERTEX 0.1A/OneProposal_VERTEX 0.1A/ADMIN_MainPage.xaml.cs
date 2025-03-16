using Firebase.Database;
using System.Collections.ObjectModel;

namespace OneProposal_VERTEX_0._1A {
    public partial class ADMIN_MainPage : ContentPage {

        private readonly FirebaseClient firebase = new FirebaseClient("https://oneproposal-onedev-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public string club = "ADMIN";
        public ObservableCollection<ProposalDetails> ActivityList { get; set; } = new ObservableCollection<ProposalDetails>();
        public ObservableCollection<Notif> NotifList { get; set; } = new ObservableCollection<Notif>();



        public ADMIN_MainPage() {
            InitializeComponent();
            BindingContext = this;
            LoadDataAll();
            LoadDataNotif();
        }

        private async void IntentMainPage(object sender, EventArgs e) {
            await Shell.Current.GoToAsync("..");
        }





        private async void LoadDataNotif() {
            var activities = await firebase.Child("NotificationForAdmin")
                                           .OnceAsync<string>();

            NotifList.Clear();

            foreach (var item in activities.OrderByDescending(item => item.Key)) // Sorting in descending order
            {
                NotifList.Add(new Notif {
                    Message = item.Object
                });
            }
        }







        private async void LoadDataAll() {
            var activities = await firebase.Child("ActivityProposal_tbl")
                                           .OnceAsync<ProposalDetails>();
            ActivityList.Clear();
            foreach (var item in activities) {
                if(item.Object.Status != "DRAFT")
                ActivityList.Add(item.Object);
            }
        }
            
            private async void LoadDataSort(string sort) {
            var activities = await firebase.Child("ActivityProposal_tbl")
                                           .OnceAsync<ProposalDetails>();
            ActivityList.Clear();
            foreach (var item in activities) {
                if (item.Object.Status == sort) 
                ActivityList.Add(item.Object);
            }
            
        }

        private async void OnActivityTapped(object sender, EventArgs e) {
            if (sender is Frame frame && frame.BindingContext is ProposalDetails selectedActivity) {
                await Navigation.PushAsync(new ActivityDetailPage(selectedActivity));
            }
        }


        public class Notif {
            public string Message { get; set; } // Notification message
        }



        public class ProposalDetails {
            public string Key { get; set; }
            public string Title { get; set; }
            public string Club { get; set; }
            public string Date { get; set; }
            public string FiledDate { get; set; }

            public string Status { get; set; }
            public string Rationale { get; set; }
            public string Objectives { get; set; }
            public string TypeOfActivity { get; set; }
            public string Venue { get; set; }
            public string Reach { get; set; }

            public string editorDate { get; set; }
            public string editorRationale { get; set; }
            public string editorObjectives { get; set; }
            public string editorActivityType { get; set; }
            public string editorVenue { get; set; }
            public string editorReach { get; set; }
        }

        private void sortAll(object sender, EventArgs e) {
            LoadDataAll();
            btn_all.BackgroundColor = (Color)Application.Current.Resources["bgYellow"];
            btn_sub.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rev.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rej.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_acc.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
        }
        private void sortSubmitted(object sender, EventArgs e) {
            LoadDataSort("SUBMITTED");
            btn_sub.BackgroundColor = (Color)Application.Current.Resources["bgYellow"];
            btn_all.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rev.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rej.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_acc.BackgroundColor = (Color)Application.Current.Resources["Gray100"];

        }
        private void sortRevision(object sender, EventArgs e) {
            LoadDataSort("REVISION");

            btn_rev.BackgroundColor = (Color)Application.Current.Resources["bgYellow"];
            btn_all.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_sub.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rej.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_acc.BackgroundColor = (Color)Application.Current.Resources["Gray100"];

        }
        private void sortRejected(object sender, EventArgs e) {
            LoadDataSort("REJECTED");
            btn_rej.BackgroundColor = (Color)Application.Current.Resources["bgYellow"];
            btn_all.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_sub.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rev.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_acc.BackgroundColor = (Color)Application.Current.Resources["Gray100"];

        }
        private void sortAccepted(object sender, EventArgs e) {
            LoadDataSort("ACCEPTED");
            btn_acc.BackgroundColor = (Color)Application.Current.Resources["bgYellow"];
            btn_all.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_sub.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rev.BackgroundColor = (Color)Application.Current.Resources["Gray100"];
            btn_rej.BackgroundColor = (Color)Application.Current.Resources["Gray100"];

        }
    }
}