using System.Collections.Generic;

namespace CollectorApp.Views;

public partial class Item : ContentView
{
    public static readonly BindableProperty ItemDataProperty = BindableProperty.Create(nameof(ItemData), typeof(Dictionary<string, dynamic>), typeof(Item), null);
    public Dictionary<string, dynamic> ItemData
    {
        get => (Dictionary < string, dynamic>)GetValue(Item.ItemDataProperty);
        set => SetValue(Item.ItemDataProperty, value);
    }
    public Item()
	{
        InitializeComponent();
        
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        
        if (BindingContext != null && barcodeImage != null && ItemData != null)
        {
            barcodeImage.Source = ItemData["Picture"];
        }

        if(BindingContext != null && ItemData != null)
        {
            if (ItemData["State"] == "sprzedany")
            {
                backgroundFrame.BackgroundColor = Color.FromHex("#8ca0a3");
            }
        }
    }
}