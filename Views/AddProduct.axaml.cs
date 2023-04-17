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
    }
}
