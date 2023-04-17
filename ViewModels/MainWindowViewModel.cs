using Avalonia.Controls;
using GroceryStoreManager.Models;
using GroceryStoreManager.Views;
using ReactiveUI;
using System.Windows.Input;

namespace GroceryStoreManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Inventory Inventory { get; set; }
        public ICommand AddProductCommand { get; }
        public MainWindowViewModel()
        {
            Inventory = new Inventory();
            Inventory.Read();
            AddProductCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                var inputForm = new AddProduct();
                await inputForm.ShowDialog(w);
            });
        }
    }
}