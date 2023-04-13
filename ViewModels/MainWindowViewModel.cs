using Avalonia.Interactivity;
using GroceryStoreManager.Models;
using Microsoft.VisualBasic;

namespace GroceryStoreManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Inventory Inventory { get; set; }
        public MainWindowViewModel()
        {
            Inventory = new Inventory();
            Inventory.Read();
        }
    }
}