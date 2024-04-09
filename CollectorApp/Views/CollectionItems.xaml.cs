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
}