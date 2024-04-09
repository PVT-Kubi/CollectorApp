using System.Collections.Generic;

namespace CollectorApp.Views;

public partial class Item : ContentView
{
    public static readonly BindableProperty ItemDataProperty = BindableProperty.Create(nameof(ItemData), typeof(Dictionary<string, dynamic>), typeof(Item), null);
    public Dictionary<string, dynamic> ItemData
    {
        get => (Dictionary < string, dynamic>)GetValue(ItemDataProperty);
        set => SetValue(ItemDataProperty, value);
    }
    public Item()
	{
		InitializeComponent();
	}
}