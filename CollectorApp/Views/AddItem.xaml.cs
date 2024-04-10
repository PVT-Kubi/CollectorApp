using CollectorApp.Models;
using Microsoft.Maui.Controls;
using System.Collections;

namespace CollectorApp.Views;


[QueryProperty(nameof(CollectionName), nameof(CollectionName))]
public partial class AddItem : ContentPage
{
    List<dynamic> allFields = new List<dynamic>();
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
        BatchBegin();
        VerticalStackLayout mainPageStackLayout = new VerticalStackLayout
        {
            Spacing = 25,
            Padding = new Thickness(30, 0),
        };
        foreach(var d in ((Models.AllItems)BindingContext).ItemTypes)
        {
            if(d.Value == "int" || d.Value == "string")
            {
                HorizontalStackLayout hLayout = new HorizontalStackLayout();
               Label label = new Label();
               label.Text = d.Key;
               Editor editor = new Editor();
               editor.Placeholder = $"Enter {d.Key}";
                hLayout.Add(label);
                hLayout.Add(editor);
                allFields.Add(editor);
                mainPageStackLayout.Add(hLayout);
            }else if(d.Value.Contains("_"))
            {
                HorizontalStackLayout hLayout = new HorizontalStackLayout();
                Label label = new Label();
                label.Text = d.Key;
                string[] values = d.Value.Split("_");
                Picker p = new Picker();
                hLayout.Add(label);
                allFields.Add(p);
                p.ItemsSource = values;
                hLayout.Add(p);
                mainPageStackLayout.Add(hLayout);
            }

        }
        Button AddButton = new Button
        {
            Text = "Add item",
        };
        Button ReturnButton = new Button { Text = "Return" };
        AddButton.Clicked += Add_Item;
        ReturnButton.Clicked += ReturnButtonClicked;
        mainPageStackLayout.Add(AddButton);
        mainPageStackLayout.Add(ReturnButton);
        ScrollView mainPageScrollView = new ScrollView { Content = mainPageStackLayout };
        Content = mainPageScrollView;
        BatchCommit();


    }

    private async void Add_Item(object sender, EventArgs e)
    {
        
        List<string> valuesOfEditors = new List<string>();
        foreach(var child in allFields)
        {
            if(child is Editor)
            {
                valuesOfEditors.Add(((Editor)child).Text);
            }else if(child is Picker)
            {
                int selectedIndex = ((Picker)child).SelectedIndex;
                valuesOfEditors.Add(((Picker)child).Items[selectedIndex]);
            }
        }
        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();

        int i = 0;
        if (valuesOfEditors.Count == ((Models.AllItems)BindingContext).ItemTypes.Count)
        {
            foreach(var item in ((Models.AllItems)BindingContext).ItemTypes)
            {
                d.Add(item.Key, valuesOfEditors[i]);
                i++;
            }

            ((Models.AllItems)BindingContext).AddItem(((Models.AllItems)BindingContext).getColllectionName(), d);
            ((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
            await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
        }
    }

    private async void ReturnButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
    }

}