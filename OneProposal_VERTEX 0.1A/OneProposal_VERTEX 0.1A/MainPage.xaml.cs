namespace OneProposal_VERTEX_0._1A {
    public partial class MainPage : ContentPage {
        public MainPage() {
            InitializeComponent();

        }

        private async void IntentProposalPage(object sender, EventArgs e) {
            await Navigation.PushAsync(new ProposalPage());
        }
    }
}
