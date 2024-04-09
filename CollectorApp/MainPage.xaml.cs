

namespace CollectorApp
{
    public partial class MainPage : ContentPage
    {
        Models.Collection currentCollection = new Models.Collection();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new Models.AllCollections();
        }

        protected override void OnAppearing()
        {
            ((Models.AllCollections)BindingContext).LoadCollections();
        }

        private async void collectionCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                Models.Collection collection = (Models.Collection)e.CurrentSelection[0];
                currentCollection = collection;
                collectionsCollection.SelectedItem = null;
                await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={collection.CollectionName}");
            }
        }

        private async void Add_Collection(object sender, EventArgs e)
        {
            int i = ((Models.AllCollections)BindingContext).AddCollection(CollectionName.Text);
            if (i != 0)
            {
                if (i == 1)
                    await DisplayAlert("Alert", "You have been alerted", "OK");
                else
                    await DisplayAlert("aler", "Cos innego sie zesralo", "OK");
            }
            ((Models.AllCollections)BindingContext).LoadCollections();
        }

        private void Delete_Collection_Button_Clicked(object sender, EventArgs e)
        {
            ((Models.AllCollections)BindingContext).DeleteCollection(currentCollection.CollectionName);
            ((Models.AllCollections)BindingContext).LoadCollections();
        }

    }

}
