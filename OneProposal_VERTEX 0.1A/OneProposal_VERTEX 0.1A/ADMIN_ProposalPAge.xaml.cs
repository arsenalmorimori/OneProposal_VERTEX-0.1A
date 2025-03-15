using System.Diagnostics;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Storage;

namespace OneProposal_VERTEX_0._1A;

public partial class ADMIN_ProposalPAge : ContentPage
{
    private MainPage.ProposalDetails selectedActivity;


    private readonly FirebaseClient firebase = new FirebaseClient("https://oneproposal-onedev-default-rtdb.asia-southeast1.firebasedatabase.app/");
    public string club = "CoPs";
    public string kkey = "";

    public ADMIN_ProposalPAge(MainPage.ProposalDetails selectedActivity) {
        InitializeComponent();
        BindingContext = selectedActivity;

        if (selectedActivity.TypeOfActivity == "Minor")
            rbMinor.IsChecked = true;
        else if (selectedActivity.TypeOfActivity == "Major")
            rbMajor.IsChecked = true;
        else if (selectedActivity.TypeOfActivity == "Other")
            rbOther.IsChecked = true;

        kkey = selectedActivity.Key;

        


        entryOther.Text = selectedActivity.TypeOfActivity.ToString();

        datePicker.Date = DateTime.Parse(selectedActivity.Date);

        switch (selectedActivity.Reach) {
            case "School-wide":
                rb2_01.IsChecked = true;
                break;
            case "Within the SHS Club": 
                rb2_02.IsChecked = true;
                break;
            case "Within subject/class":
                rb2_03.IsChecked = true;
                break;
            default:
                rb2_04.IsChecked = true;
                entryRB2.IsVisible = true;
                entryRB2.Text = selectedActivity.Reach; // Set the "Other" value
                break;
        }

        }


    private async void BackIntent(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("..");
    }


    private async void Button_Clicked_1(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("..");
    }

    private void OnOtherCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (sender is RadioButton rb) {
            entryOther.IsVisible = rb.IsChecked;
        }
    }

    private void OnOtherCheckedChanged2(object sender, CheckedChangedEventArgs e) {
        if (sender is RadioButton rb) {
            entryRB2.IsVisible = rb.IsChecked;
        }
    }

    //  --------------- VENUE ---------------
    public string[] venues = {
        "Room 201", "Room 202", "Room 203", "Room 204", "Room 205", "Room 206", "Room 207", "Room 208", "Room 209", "Room 210",
        "Room 301", "Room 302", "Room 303", "Room 304", "Room 305", "Room 306", "Room 307", "Room 308", "Room 309", "Room 310",
        "ComLab 1", "ComLab 2", "Aquarium", "Other"
    };

    protected override void OnAppearing() {
        base.OnAppearing();
        int index = 0;

        foreach (var venue in venues) {
            var checkBox = new CheckBox();
            checkBox.CheckedChanged += (s, e) => { if (venue == "Other") entryOtherVenue.IsVisible = e.Value; };

            venueGrid.Add(new HorizontalStackLayout { Children = { checkBox, new Label { Text = venue } } }, index % 3, index / 3);
            index++;
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

    private void Button_Clicked(object sender, EventArgs e) {
        saveToFirebase("REVISION","Revised");
    }




    private async void saveToFirebase(string save, string notifMessage) {
     

        // Get selected type of activity
        string activityType = rbMinor.IsChecked ? "Minor" :
                              rbMajor.IsChecked ? "Major" :
                              rbOther.IsChecked ? entryOther.Text : "N/A";

        // Get selected reach type
        string reachType = rb2_01.IsChecked ? "School-wide" :
                           rb2_02.IsChecked ? "Within the SHS Club" :
                           rb2_03.IsChecked ? "Within subject/class" :
                           rb2_04.IsChecked ? entryRB2.Text : "N/A";

        // Get selected venue
        string selectedVenue = "";
        foreach (var child in venueGrid.Children) {
            if (child is HorizontalStackLayout hsl && hsl.Children[0] is CheckBox cb && cb.IsChecked) {
                selectedVenue += ((Label)hsl.Children[1]).Text + ", ";
            }
        }
        selectedVenue = selectedVenue.TrimEnd(',', ' ');  // Remove trailing comma

        if (!string.IsNullOrWhiteSpace(entryOtherVenue.Text))
            selectedVenue += $" (Other: {entryOtherVenue.Text})";

        // Check if required fields are filled
        if (string.IsNullOrWhiteSpace(et01_title.Text) ||
            string.IsNullOrWhiteSpace(et02_rational.Text) ||
            string.IsNullOrWhiteSpace(et03_objective.Text) ||
            string.IsNullOrWhiteSpace(selectedVenue) ||
            string.IsNullOrWhiteSpace(activityType) ||
            string.IsNullOrWhiteSpace(reachType)) {
            await DisplayAlert("Please Fill all Requirements", "Please check again your Activity Proposal Form", "OK");
            return;
        }


        // Display confirmation message
        string message = $"🎭 Club: \n\n {club}\n\n\n" +
                         $"🚀 Status: \n\n {"REVISION"}\n\n\n" +
                         $"📌 Title: \n\n {et01_title.Text}\n\n\n\n" +
                         $"📝  Rationale:\n\n {et02_rational.Text}\n\n\n" +
                         $"🎯 Objectives:\n\n {et03_objective.Text}\n\n\n" +
                         $"📅 Date:\n\n {datePicker.ToString}\n\n\n" +
                         $"🏢 Venue: \n\n {datePicker.Date.ToString("MMMM dd, yyyy")}\n\n\n" +
                         $"📊 Type of Activity:\n\n {activityType}\n\n\n" +
                         $"🌎 Reach:\n\n {reachType}";

        bool isConfirmed = await DisplayAlert("Confirm Update", message, "Confirm", "Cancel");
        if (!isConfirmed) return;

        await DisplayAlert("Success", "Activity has been updated successfully!", "OK");



        // Update selected activity object
        var aSelectedActivity = new ProposalDetails {
            Title = et01_title.Text,
            Status = "REVISION",
            Rationale = et02_rational.Text,
            Objectives = et03_objective.Text,
            Venue = selectedVenue,
            TypeOfActivity = activityType,
            Reach = reachType,
            Date = datePicker.Date.ToString("MMMM dd, yyyy"), // Format date
            editorDate = null,
            editorRationale = null,
            editorObjectives = null,
            editorActivityType = null,
            editorVenue = null,
            editorReach = null
        };


        // Update the activity in Firebase using its Key
        await firebase.Child("ActivityProposal_tbl")
                        .Child(kkey)
                        .PatchAsync(aSelectedActivity);



        //// Send Notifications
        //await firebase.Child("NotificationFor" + selectedActivity.Club).PostAsync(et01_title.Text + " (" + selectedActivity.Club + ") has been Revised.");
        //await firebase.Child("NotificationForAdmin").PostAsync(selectedActivity.Title + " (" + selectedActivity.Club + ") has been Revised.");

        // Navigate back
        await Shell.Current.GoToAsync("..");
    }


}

