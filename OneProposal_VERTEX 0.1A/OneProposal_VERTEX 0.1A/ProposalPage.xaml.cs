using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Storage;
using static OneProposal_VERTEX_0._1A.ProposalPage;

namespace OneProposal_VERTEX_0._1A;

public partial class ProposalPage : ContentPage {

    private readonly FirebaseClient firebase = new FirebaseClient("https://oneproposal-onedev-default-rtdb.asia-southeast1.firebasedatabase.app/");
    public string club = "CoPs";

    public ProposalPage() {
        InitializeComponent();
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

    //  --------------- SAVE AS DRAFT ---------------
    private void DraftCliked(object sender, EventArgs e) {
        saveToFirebase("DRAFT", "save as DRAFT");

    }

    //  --------------- INSERT ---------------
    private async void OnDoneClicked(object sender, EventArgs e) {
        saveToFirebase("SUBMITTED", "SUBMITTED");
    }

    //  --------------- OBJECTS ---------------
    public class ActivityDetails {
        public string Key { get; set; }
        public string Club { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Title { get; set; }
        public string Rationale { get; set; }
        public string Objectives { get; set; }
        public string TypeOfActivity { get; set; }
        public string Date { get; set; }
        public string Venue { get; set; }
        public string Reach { get; set; }
        public string FiledDate { get; set; }

        public string editorDate { get; set; }
        public string editorRationale { get; set; }
        public string editorObjectives { get; set; }
        public string editorActivityType { get; set; }
        public string editorVenue { get; set; }
        public string editorReach { get; set; }


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

        if (rbOther.IsChecked) selectedVenue += $" (Other: {entryOtherVenue.Text})";

        // Create object with form data
        var activity = new ActivityDetails {
            Key = "",
            Club = club,
            Status = save,
            Remarks = null,
            Title = et01_title.Text,
            Rationale = et02_rational.Text,
            Objectives = et03_objective.Text,
            TypeOfActivity = activityType,
            Date = datePicker.Date.ToString("MMMM dd, yyyy"),
            Venue = selectedVenue,
            Reach = reachType,
            FiledDate = DateTime.Now.ToString("MMMM dd, yyyy"),
            editorDate = null,
            editorRationale = null,
            editorObjectives = null,
            editorActivityType = null,
            editorVenue = null,
            editorReach = null
        };


        if (save == "SUBMITTED") {
            if (string.IsNullOrWhiteSpace(et01_title.Text) ||
                string.IsNullOrWhiteSpace(et02_rational.Text) ||
                string.IsNullOrWhiteSpace(et03_objective.Text) ||
                string.IsNullOrWhiteSpace(selectedVenue) ||
                string.IsNullOrWhiteSpace(activityType) ||
                string.IsNullOrWhiteSpace(reachType)) {
                DisplayAlert("Please Fill all Requirements", "Please check again your Activity Proposal Form", "OK");
            } else {
                // Display pop-up confirmation
                string message = $"🎭 Club: \n\n {activity.Club}\n\n\n" +
                                 $"🚀 Status: \n\n {activity.Status}\n\n\n" +
                                 $"📌 Title: \n\n {activity.Title}\n\n\n\n" +
                                 $"📝  Rationale:\n\n {activity.Rationale}\n\n\n" +
                                 $"🎯 Objectives:\n\n {activity.Objectives}\n\n\n" +
                                 $"📅 Date:\n\n {activity.Date}\n\n\n" +
                                 $"🏢 Venue: \n\n {activity.Venue}\n\n\n" +
                                 $"📊 Type of Activity:\n\n {activity.TypeOfActivity}\n\n\n" +
                                 $"🌎 Reach:\n\n {activity.Reach}";

                bool isConfirmed = await DisplayAlert("Confirm Submission", message, "Confirm", "Cancel");
                if (isConfirmed) {
                    await DisplayAlert("Success", "Activity has been " + notifMessage + " !", "OK");
                }

                var Insert = await firebase.Child("ActivityProposal_tbl").PostAsync(activity);
                activity.Key = Insert.Key;
                await firebase.Child("ActivityProposal_tbl").Child(Insert.Key).PatchAsync(activity);


                // NOTIFICATION FOR  "CLUB"
                await firebase.Child(("NotificationFor" + club) as string).PostAsync(activity.Title + " (" + activity.Club + ")  has been " + notifMessage);

                // NOTIFICATION FOR  "ADMIN"
                await firebase.Child(("NotificationForAdmin") as string).PostAsync(activity.Title + " (" + activity.Club + ")  has been " + notifMessage);


                await Shell.Current.GoToAsync("..");

               
            }
        } else if (save == "DRAFT") {
            string message = $"🎭 Club: \n\n {activity.Club}\n\n\n" +
                                 $"🚀 Status: \n\n {activity.Status}\n\n\n" +
                                 $"📌 Title: \n\n {activity.Title}\n\n\n\n" +
                                 $"📝  Rationale:\n\n {activity.Rationale}\n\n\n" +
                                 $"🎯 Objectives:\n\n {activity.Objectives}\n\n\n" +
                                 $"📅 Date:\n\n {activity.Date}\n\n\n" +
                                 $"🏢 Venue: \n\n {activity.Venue}\n\n\n" +
                                 $"📊 Type of Activity:\n\n {activity.TypeOfActivity}\n\n\n" +
                                 $"🌎 Reach:\n\n {activity.Reach}";


            bool isConfirmed = await DisplayAlert("Confirm Submission as DRAFT", message, "Confirm", "Cancel");
            if (isConfirmed) {
                await DisplayAlert("Success", "Activity has been " + notifMessage + " !", "OK");
            }
            await firebase.Child("ActivityProposal_tbl").PostAsync(activity);
            await Shell.Current.GoToAsync("..");
            
        } else {

        }
    }





}
    
