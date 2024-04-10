namespace CollectorApp.Views;

[QueryProperty(nameof(ItemData), nameof(ItemData))]
public partial class EditItem : ContentPage
{
    List<dynamic> allFields = new List<dynamic>();
    Dictionary<string, dynamic> itemData = new Dictionary<string, dynamic>();
    public int itemIndex = 0;
    public string ItemData
    {
        set { LoadItems(value); }
    }
    public EditItem()
    {
        InitializeComponent();
    }

    public void LoadItems(string itemName)
    {
        allFields.Clear();
        string[] s = itemName.Split('_');
        BindingContext = new Models.AllItems(s[0]);
        itemData = ((Models.AllItems)BindingContext).getItem(s[1]).Data;
        itemIndex = ((Models.AllItems)BindingContext).getItemIndex(s[1]);


        BatchBegin();
        VerticalStackLayout mainPageStackLayout = new VerticalStackLayout
        {
            Spacing = 25,
            Padding = new Thickness(30, 0),
        };
        foreach (var d in ((Models.AllItems)BindingContext).ItemTypes)
        {
            if (d.Value == "int" || d.Value == "string")
            {
                HorizontalStackLayout hLayout = new HorizontalStackLayout();
                Label label = new Label();
                label.Text = d.Key;
                Editor editor = new Editor();
                editor.Placeholder = $"Enter {d.Key}";
                editor.Text = itemData[d.Key];
                hLayout.Add(label);
                hLayout.Add(editor);
                allFields.Add(editor);
                mainPageStackLayout.Add(hLayout);
            }
            else if (d.Value == "picture")
            {
                HorizontalStackLayout hLayout = new HorizontalStackLayout();
                Button button = new Button();
                button.Text = "Load image";
                button.Clicked += LoadImage;
                Image image = new Image();
                image.HeightRequest = 200;
                image.Source = itemData[d.Key];
                allFields.Add(image);
                hLayout.Add(image);
                hLayout.Add(button);
                mainPageStackLayout.Add(hLayout);
            }
            else if (d.Value.Contains("_"))
            {
                HorizontalStackLayout hLayout = new HorizontalStackLayout();
                Label label = new Label();
                label.Text = d.Key;
                string[] values = d.Value.Split("_");
                Picker p = new Picker();
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == itemData[d.Key])
                    {
                        //Nie wiem czemu nie dziala, wersja z selected item tez jest zepsuta, zostawiam na koncowe poprawki, najwyzej tak zostanie 
                        p.SelectedIndex = i;
                    }
                }

                hLayout.Add(label);
                allFields.Add(p);
                p.ItemsSource = values;
                hLayout.Add(p);
                mainPageStackLayout.Add(hLayout);
            }

        }
        Button EditButton = new Button
        {
            Text = "Edit item",
        };
        Button DeleteButton = new Button { Text = "Delete", };
        Button ReturnButton = new Button { Text = "Return" };
        EditButton.Clicked += Edit_Item;
        ReturnButton.Clicked += ReturnButtonClicked;
        DeleteButton.Clicked += DeleteButtonClicked;
        mainPageStackLayout.Add(EditButton);
        mainPageStackLayout.Add(ReturnButton);
        mainPageStackLayout.Add(DeleteButton);
        ScrollView mainPageScrollView = new ScrollView { Content = mainPageStackLayout };
        Content = mainPageScrollView;
        BatchCommit();

    }

    private async void Edit_Item(object sender, EventArgs e)
    {
        List<string> valuesOfEditors = new List<string>();
        foreach (var child in allFields)
        {
            if (child is Editor)
            {
                valuesOfEditors.Add(((Editor)child).Text);
            }
            else if (child is Picker)
            {
                int selectedIndex = ((Picker)child).SelectedIndex;
                valuesOfEditors.Add(((Picker)child).Items[selectedIndex]);
            }
            else if (child is Image)
            {
                valuesOfEditors.Add(child.Source);
            }
        }
        Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();

        int i = 0;
        if (valuesOfEditors.Count == ((Models.AllItems)BindingContext).ItemTypes.Count)
        {
            foreach (var item in ((Models.AllItems)BindingContext).ItemTypes)
            {
                d.Add(item.Key, valuesOfEditors[i]);
                i++;
            }

            ((Models.AllItems)BindingContext).EditItem(((Models.AllItems)BindingContext).getColllectionName(), d, itemIndex);
            ((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
            await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
        }
    }

    public async void LoadImage(object sender, EventArgs e)
    {
        var images = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Pick Barcode/QR Code Image",
            FileTypes = FilePickerFileType.Images
        });

        var imageSource = images.FullPath.ToString();
        foreach (var child in allFields)
        {
            if (child is Image)
            {
                child.Source = imageSource;
            }

        }
    }
    private async void DeleteButtonClicked(object sender, EventArgs e)
    {
        string itemName = "";
        foreach (var child in allFields)
        {
            if (child is Editor)
            {
                itemName = ((Editor)child).Text;
                break;
            }
        }
        ((Models.AllItems)BindingContext).DeleteItem(itemName);
        ((Models.AllItems)BindingContext).LoadItems(((Models.AllItems)BindingContext).getColllectionName());
        await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
    }


    private async void ReturnButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(Views.CollectionItems)}?{nameof(Views.CollectionItems.ItemId)}={((Models.AllItems)BindingContext).collectionName}");
    }
}