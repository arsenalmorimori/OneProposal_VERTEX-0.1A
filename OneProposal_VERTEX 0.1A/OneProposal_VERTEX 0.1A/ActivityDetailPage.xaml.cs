using System.Diagnostics;

namespace OneProposal_VERTEX_0._1A;

public partial class ActivityDetailPage : ContentPage
{
    private ADMIN_MainPage.ProposalDetails selectedActivity;

    public ActivityDetailPage(MainPage.ProposalDetails activity)
	{
		InitializeComponent();
        BindingContext = activity;
    }
    public ActivityDetailPage(ADMIN_MainPage.ProposalDetails activity)
	{
		InitializeComponent();
        BindingContext = activity;
    }

    private async void Button_Clicked_1(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("..");
    }

}