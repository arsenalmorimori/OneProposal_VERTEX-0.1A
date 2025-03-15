using Firebase.Database;
using Firebase.Database.Query; 
using Microsoft.Maui.Storage;
using System.Diagnostics;

namespace OneProposal_VERTEX_0._1A;

public partial class ADMIN_RevisedControls : ContentPage {


    private readonly FirebaseClient firebase = new FirebaseClient("https://oneproposal-onedev-default-rtdb.asia-southeast1.firebasedatabase.app/");
    public string club = "CoPs";
    
    private ADMIN_MainPage.ProposalDetails receivedActivity;


    public ADMIN_RevisedControls(ADMIN_MainPage.ProposalDetails activity) {
        InitializeComponent();
        receivedActivity = activity; // Store received data
        BindingContext = receivedActivity; // Bind to UI if needed
    }

    private async void Button_Clicked_1(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("..");
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e) {

        if (sender == chkDate) editorDate.IsVisible = e.Value;
        else if (sender == chkRationale) editorRationale.IsVisible = e.Value;
        else if (sender == chkObjectives) editorObjectives.IsVisible = e.Value;
        else if (sender == chkActivityType) editorActivityType.IsVisible = e.Value;
        else if (sender == chkVenue) editorVenue.IsVisible = e.Value;
        else if (sender == chkReach) editorReach.IsVisible = e.Value;
    }

    public class remarkET {
        public string editorDate { get; set; }
        public string editorRationale { get; set; }
        public string editorObjectives { get; set; }
        public string editorActivityType { get; set; }
        public string editorVenue { get; set; }
        public string editorReach { get; set; }
    }

    private async void revisedClicked(object sender, EventArgs e) {
        addRemarkToDB(club, "Request for a Revision");

    }

    private async void addRemarkToDB(string save, string notifMessage) {
        var addRemark = new remarkET {
            editorDate = editorDate.Text,
            editorRationale = editorRationale.Text,
            editorObjectives = editorObjectives.Text,
            editorActivityType = editorActivityType.Text,
            editorVenue = editorVenue.Text,
            editorReach = editorReach.Text
        };

        bool isConfirmed = await DisplayAlert("Confirm Remarks for Revision ", "The remarks will be sent to the CLUB", "Confirm", "Cancel");
        if (isConfirmed) {
            await firebase.Child("ActivityProposal_tbl")
              .Child(receivedActivity.Key)
              .PatchAsync(addRemark);
            
            await firebase.Child("ActivityProposal_tbl")
              .Child(receivedActivity.Key)
              .PatchAsync(new {Status = "REVISED"});

            await Shell.Current.GoToAsync("..");


            // NOTIFICATION FOR  "CLUB"
            await firebase.Child(("NotificationFor" + club) as string).PostAsync("ADMIN NOTE  :  "+receivedActivity.Title + " (" + receivedActivity.Club + ")  has been " + notifMessage);

            // NOTIFICATION FOR  "ADMIN"
            await firebase.Child(("NotificationForAdmin") as string).PostAsync(receivedActivity.Title + " (" + receivedActivity.Club + ")  has been " + notifMessage);
            await DisplayAlert("Success", "Activity has been " + notifMessage + " !", "OK");
        }

    }
}