﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.Models;
using GroceryStoreManager.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreManager.Views
{
    public partial class AddProduct : Window
    {
        private readonly AddProductViewModel vm;
        public AddProduct(Inventory inv)
        {
            InitializeComponent();
            vm = new AddProductViewModel(inv);
            DataContext = vm;
        }
        public AddProduct()
        {
            InitializeComponent();
        }
        public async void OK(object sender, RoutedEventArgs e)
        {
            int i = vm.Check();
            switch (i)
            {
                case 1:
                    await MessageBox.Show(this, "ID sản phẩm không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 2:
                    await MessageBox.Show(this, "ID đã tồn tại", "Lỗi", MessageBox.MessageBoxButtons.Ok);
                    break;
                case 3:
                    await MessageBox.Show(this, "Số lượng không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
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
