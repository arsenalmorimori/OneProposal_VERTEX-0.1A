using System.Collections.ObjectModel;
using Firebase.Database;

namespace OneProposal_VERTEX_0._1A {
    public partial class MainPage : ContentPage {

        private readonly FirebaseClient firebase = new FirebaseClient("https://oneproposal-onedev-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public string club = "CoPs";
        public ObservableCollection<ProposalDetails> ActivityList { get; set; } = new ObservableCollection<ProposalDetails>();
        public ObservableCollection<ProposalDetails> DraftList { get; set; } = new ObservableCollection<ProposalDetails>();


        public MainPage() {
            InitializeComponent();
            BindingContext = this;
            LoadData();
            LoadDataDraft();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            LoadData();
            LoadDataDraft();
        }


        private async void IntentProposalPage(object sender, EventArgs e) {
            await Navigation.PushAsync(new ProposalPage());
        }
        private async void IntentAdminMainPage(object sender, EventArgs e) {
            await Navigation.PushAsync(new ADMIN_MainPage());
        }

        private async void LoadData() {
            var activities = await firebase.Child("ActivityProposal_tbl")
                                           .OnceAsync<ProposalDetails>();

            ActivityList.Clear();
            //ONLY SUBMITTED
            foreach (var item in activities.Where(a => a.Object.Status == "SUBMITTED")) {
                ActivityList.Add(item.Object);
            
            }foreach (var item in activities.Where(a => a.Object.Status == "REVISED")) {
                ActivityList.Add(item.Object); 
            }
        }

        private async void LoadDataDraft() {
            
                var activities = await firebase.Child("ActivityProposal_tbl")
                                               .OnceAsync<ProposalDetails>();

                DraftList.Clear();
                //ONLY SUBMITTED
                foreach (var item in activities.Where(a => a.Object.Status == "DRAFT")) {
                    DraftList.Add(item.Object); 
                // .Where(a => a.Object.Status == "DRAFT")
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

    }


}
