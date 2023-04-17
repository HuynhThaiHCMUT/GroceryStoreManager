using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.ViewModels;

namespace GroceryStoreManager.Views
{
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
            DataContext = new AddProductViewModel();
        }
        public void OK(object sender, RoutedEventArgs e)
        {

        }
        public void Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}
