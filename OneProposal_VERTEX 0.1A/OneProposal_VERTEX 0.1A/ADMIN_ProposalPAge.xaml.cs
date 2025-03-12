namespace OneProposal_VERTEX_0._1A;

public partial class ADMIN_ProposalPAge : ContentPage
{
	public ADMIN_ProposalPAge()
	{
		InitializeComponent();
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
}