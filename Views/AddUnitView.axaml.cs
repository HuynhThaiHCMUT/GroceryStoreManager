using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.Models;
using GroceryStoreManager.ViewModels;

namespace GroceryStoreManager.Views
{
    public partial class AddUnitView : Window
    {
        private readonly AddUnitViewModel vm;
        public AddUnitView(ObservableCollection<Unit> units, int index)
        {
            InitializeComponent();
            vm = new AddUnitViewModel(units, index);
            DataContext = vm;
        }
        public AddUnitView(ObservableCollection<Unit> units)
        {
            InitializeComponent();
            vm = new AddUnitViewModel(units);
            DataContext = vm;   
        }
        public AddUnitView()
        {
            InitializeComponent();
        }
        public async void OK(object sender, RoutedEventArgs e)
        {
            int i = vm.Check();
            switch (i)
            {
                case 1:
                    await MessageBox.Show(this, "Giá không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 2:
                    await MessageBox.Show(this, "Giá gốc không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 3:
                    await MessageBox.Show(this, "Trọng số không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 4:
                    await MessageBox.Show(this, "Tên đơn vị đã tồn tại", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 0:
                    Close(vm.Result());
                    break;
            }
        }
        public void Cancel(object sender, RoutedEventArgs e)
        {
            Close(null);
        }
    }
}
