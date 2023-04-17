using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.ViewModels;

namespace GroceryStoreManager.Views
{
    public partial class AddUnit : Window
    {
        private readonly AddUnitViewModel vm;
        public AddUnit()
        {
            InitializeComponent();
            vm = new AddUnitViewModel();
            DataContext = vm;
            
        }
        public async void OK(object sender, RoutedEventArgs e)
        {
            int i = vm.Check();
            switch (i)
            {
                case 0:
                    Close(vm.Result());
                    break;
                case 1:
                    await MessageBox.Show(this, "Giá không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 2:
                    await MessageBox.Show(this, "Giá gốc không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 3:
                    await MessageBox.Show(this, "Trọng số không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
            }
        }
        public void Cancel(object sender, RoutedEventArgs e)
        {
            Close(null);
        }
    }
}
