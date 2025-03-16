using System.Diagnostics;
using Firebase.Database;
using Firebase.Database.Query;

namespace OneProposal_VERTEX_0._1A;

public partial class ActivityDetailPage : ContentPage {
    private ADMIN_MainPage.ProposalDetails selectedActivity;


    private readonly FirebaseClient firebase = new FirebaseClient("https://oneproposal-onedev-default-rtdb.asia-southeast1.firebasedatabase.app/");
    public string club = "CoPs";
    public string kkey = "";
    public string title = "";


    public ActivityDetailPage(MainPage.ProposalDetails activity) {
        InitializeComponent();
        BindingContext = activity;
        ButtonStatusUpdate.IsVisible = false;

        ButtonRevised.IsVisible = false;

        if(activity.Status == "REVISION") {
            ButtonRevised.IsVisible =true;
        } else {
            ButtonRevised.IsVisible = false;
        }

    }
    public ActivityDetailPage(ADMIN_MainPage.ProposalDetails activity) {
        InitializeComponent();
        BindingContext = activity;

        kkey = activity.Key;
        title = activity.Title;
        ButtonStatusUpdate.IsVisible = true;
   }



    private async void Button_Clicked_1(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("..");
    }



    private async void AdminAcceptButton(object sender, EventArgs e) {

        UpdateStatus("ACCEPTED","Accepted!, GOODLUCK!");
    }

    private async void AdminRejectButton(object sender, EventArgs e) {
        UpdateStatus("REJECTED", "Rejected, Try again");


    }

    private async void UpdateStatus(string save, string notifMessage) {
        await DisplayAlert("Confirm", "Activity (" + kkey + ") will be update!", "OK");

        await firebase.Child("ActivityProposal_tbl")
              .Child(kkey)
              .PatchAsync(new {Status = save});

        await Shell.Current.GoToAsync("..");

        // NOTIFICATION FOR  "CLUB"
        await firebase.Child(("NotificationFor" + club) as string).PostAsync("ADMIN NOTE  :  " + title + " (" + club + ")  has been " + notifMessage);

        // NOTIFICATION FOR  "ADMIN"
        await firebase.Child(("NotificationForAdmin") as string).PostAsync(title + " (" + club + ")  has been " + notifMessage);
        await DisplayAlert("Success", "Activity ("+ kkey +") has been " + notifMessage + " !", "OK");

    }




    private async void AdminReviseButton(object sender, EventArgs e) {
        if (BindingContext is ADMIN_MainPage.ProposalDetails selectedActivity) {
            await Navigation.PushAsync(new ADMIN_RevisedControls(selectedActivity));
        }
    
    }




    private async void AdminReviseButtonMember(object sender, EventArgs e) {
        if (BindingContext is MainPage.ProposalDetails selectedActivity) {
            await Navigation.PushAsync(new ADMIN_ProposalPAge(selectedActivity));
        }
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

}
