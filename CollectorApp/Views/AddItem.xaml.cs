using CollectorApp.Models;
using Microsoft.Maui.Controls;
using System.Collections;

namespace CollectorApp.Views;


[QueryProperty(nameof(CollectionName), nameof(CollectionName))]
public partial class AddItem : ContentPage
{
    public string CollectionName
    {
        set { LoadItems(value); }
    }
    public AddItem()
	{
		InitializeComponent();
	}

    public void LoadItems(string name)
    {
        BindingContext = new Models.AllItems(name);
    }

    private async void Add_Item(object sender, EventArgs e)
    {
        List<string> valuesOfEditors = new List<string>();
        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
        //d.Add("Name", "");
        //d.Add("Price", 0);
        //d.Add("State", "");
        //d.Add("Rating", 0);
        //d.Add("Comment", "");
        //d.Add("Picture", "");
        //((Models.AllItems)BindingContext).AddItem(((Models.AllItems)BindingContext).getColllectionName(), d);
        //((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
        //await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={collection.CollectionName}");

        foreach(var child in itemValuesCollection.GetVisualTreeDescendants())
        {
            if(child is Editor)
            {
                valuesOfEditors.Add(((Editor)child).Text);
            }
        }
        if(valuesOfEditors.Count == ((Models.AllItems)BindingContext).ItemTypes.Count)
        {
            for (int i = 0; i < valuesOfEditors.Count; i++)
            {
                d.Add(((Models.AllItems)BindingContext).ItemTypes[i], valuesOfEditors[i]);
            }
            ((Models.AllItems)BindingContext).AddItem(((Models.AllItems)BindingContext).getColllectionName(), d);
            ((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
            await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
        }
       

        //foreach (string type in ((Models.AllItems)BindingContext).ItemTypes)
        //{

        //    d.Add(type, editor.Text);
        //    ((Models.AllItems)BindingContext).AddItem(((Models.AllItems)BindingContext).getColllectionName(), d);
        //    ((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
        //    await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");

        //}

    }

    private async void ReturnButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
    }

}