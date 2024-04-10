



namespace CollectorApp.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class CollectionItems : ContentPage
{
    
    public string ItemId
    {
        set { LoadItems(value); }
    }
    public CollectionItems()
	{
		InitializeComponent();
	}
        
    public void LoadItems(string name)
    {
        BindingContext = new Models.AllItems(name);
        
    }

    private async void ReturnButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

    private async void Add_Item(object sender, EventArgs e)
    {
        //Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
        //d.Add("Name", "");
        //d.Add("Price", 0);
        //d.Add("State", "");
        //d.Add("Rating", 0);
        //d.Add("Comment", "");
        //d.Add("Picture", "");
        //((Models.AllItems)BindingContext).AddItem(((Models.AllItems)BindingContext).getColllectionName(), d);
        //((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
        //await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={collection.CollectionName}");
        await Shell.Current.GoToAsync($"//{nameof(Views.AddItem)}?{nameof(Views.AddItem.CollectionName)}={((Models.AllItems)BindingContext).collectionName}");
    }

    private async void AddColumnButtonClicked(object sender, EventArgs e)
    {
        ((Models.AllItems)BindingContext).AddColumn(((Models.AllItems)BindingContext).getColllectionName(), ColumnName.Text);
        ((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
    }

}