using GroceryStoreManager.Models;
using GroceryStoreManager.Views;
using ReactiveUI;
using System.Windows.Input;

namespace GroceryStoreManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Inventory Inventory { get; set; }
        public AddProduct AddProduct { get; set; }
        public MainWindowViewModel()
        {
            Inventory = new Inventory();
            AddProduct= new AddProduct();
            Inventory.Read();
        }
        public void AddProductCommand()
        {

        }
    }
}