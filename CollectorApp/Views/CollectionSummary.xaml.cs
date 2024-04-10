using CollectorApp.Models;
using System.Collections;

namespace CollectorApp.Views;

[QueryProperty(nameof(CollectionName), nameof(CollectionName))]
public partial class CollectionSummary : ContentPage
{
    public string CollectionName
    {
        set { LoadItems(value); }
    }
    public CollectionSummary()
	{
		InitializeComponent();
	}

    public void LoadItems(string name)
    {
        BindingContext = new Models.AllItems(name);
        int selledCounter = 0;
        int toSellCounter = 0;
        itemsCount.Text = $"Ilosc przedmiotow w kolekcji: {((Models.AllItems)BindingContext).Items.Count}";
        foreach(Models.Item item in ((Models.AllItems)BindingContext).Items)
        {
            if (item.Data["State"] == "na-sprzedaż")
                toSellCounter++;
            else if (item.Data["State"] == "sprzedany")
                selledCounter++;
        }
        selledCount.Text = $"Ilosc przedmiotow sprzedanych w kolekcji: {selledCounter}";
        toSellCount.Text = $"Ilosc przedmiotow do sprzedania w kolekcji: {toSellCounter}";
    }
    private async void ReturnButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
    }
    
}