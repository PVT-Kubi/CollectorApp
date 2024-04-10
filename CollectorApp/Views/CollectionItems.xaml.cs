



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

    private void columnTypeSelectionChanged(object sender, EventArgs e)
    {
        if(ColumnType.SelectedIndex != 2)
        {
            types.IsEnabled = false;
        }
        else
        {
            types.IsEnabled= true;
        }
    }

    private async void selectedItem(object sender, SelectionChangedEventArgs e)
    {
        
        if (e.CurrentSelection.Count > 0)
        {
            
            Models.Item item = (Models.Item)e.CurrentSelection[0];
            colletionsCollection.SelectedItem = null;
            await Shell.Current.GoToAsync($"//{nameof(Views.EditItem)}?{nameof(Views.EditItem.ItemData)}={((Models.AllItems)BindingContext).collectionName}_{item.Data["Name"]}");
        }
    }

    private async void Add_Item(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(Views.AddItem)}?{nameof(Views.AddItem.CollectionName)}={((Models.AllItems)BindingContext).collectionName}");
    }

    private async void AddColumnButtonClicked(object sender, EventArgs e)
    {
        string clType = "";
        if(types.IsEnabled)
        {
            clType = types.Text.Replace(' ', '_');
        }
        else
        {
            clType = ColumnType.Items[ColumnType.SelectedIndex];    
        }
        ((Models.AllItems)BindingContext).AddColumn(((Models.AllItems)BindingContext).getColllectionName(), ColumnName.Text, clType);
        ((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
    }
    private async void SummaryButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(Views.CollectionSummary)}?{nameof(Views.CollectionSummary.CollectionName)}={((Models.AllItems)BindingContext).collectionName}");
    }


}