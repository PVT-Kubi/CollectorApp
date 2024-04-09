using Java.Lang;

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
                classCollection.SelectedItem = null;
                await Shell.Current.GoToAsync($"//{nameof(CollectionItems)}?{nameof(CollectionItems.ItemId)}={collection.CollectionName}");
            }
        }

        private async void Add_Class(object sender, EventArgs e)
        {
            int i = ((Models.AllCollections)BindingContext).AddCollection(CollectionName.Text);
            if (i != 0)
            {
                if (i == 1)
                    await DisplayAlert("Alert", "You have been alerted", "OK");
                else
                    await DisplayAlert("aler", "Cos innego sie zesralo", "OK");
            }
            ((Models.AllClasses)BindingContext).LoadClasses();
        }

        private void Delete_Class_Button_Clicked(object sender, EventArgs e)
        {
            ((Models.AllClasses)BindingContext).DeleteClass(currentClass.ClassNumber);
            ((Models.AllClasses)BindingContext).LoadClasses();
        }
    }

}
