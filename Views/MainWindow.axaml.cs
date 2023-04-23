using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.Models;
using GroceryStoreManager.ViewModels;
using System.ComponentModel;

namespace GroceryStoreManager.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel
            {
                W = this
            };
            DataContext = vm;
        }
        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            vm.Inventory.Save();
        }
        public void Item_DoubleTapped(object sender, RoutedEventArgs e)
        {
            vm.AddInvoice(this);
        }
    }
}