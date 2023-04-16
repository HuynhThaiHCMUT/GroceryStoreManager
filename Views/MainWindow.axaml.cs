using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.Models;
using GroceryStoreManager.ViewModels;

namespace GroceryStoreManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        void MainWindow_Closed(object sender, System.EventArgs e)
        {
            ((MainWindowViewModel)DataContext).Inventory.Save();
        }
    }
}