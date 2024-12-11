namespace MoldovanMariaKarinaLab7.Models;

public partial class ListPage : ContentPage
{
   
    public ListPage()
	{
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ShopID = selectedShop.ID;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
       this.BindingContext)
        {
            BindingContext = new Product()
        });

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    
    private Product _tappedProduct;

    void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        _tappedProduct = e.Item as Product;
    }
    async void OnDeleteSelectedItemClicked(object sender, EventArgs e)
    {
        if (_tappedProduct != null)
        {
            await App.Database.DeleteProductAsync(_tappedProduct);
            var shopList = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
            _tappedProduct = null;
            await DisplayAlert("Success", "Product deleted successfully.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Please tap on a product first before deleting.", "OK");
        }
    }

}