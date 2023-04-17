using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.Models;
using GroceryStoreManager.ViewModels;

namespace GroceryStoreManager.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            DataContext = vm;
        }
        void MainWindow_Closed(object sender, System.EventArgs e)
        {
            vm.Inventory.Save();
        }
    }
}