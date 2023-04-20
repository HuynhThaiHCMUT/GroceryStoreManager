using Avalonia.Controls;
using Avalonia.Interactivity;
using GroceryStoreManager.Models;
using GroceryStoreManager.ViewModels;
using System.Threading;

namespace GroceryStoreManager.Views
{
    public partial class AddInvoiceView : Window
    {
        private readonly AddInvoiceViewModel vm;
        public AddInvoiceView(Product p)
        {
            InitializeComponent();
            vm = new AddInvoiceViewModel(p);
            DataContext = vm;
        }
        public AddInvoiceView(InvoiceItem i)
        {
            InitializeComponent();
            vm = new AddInvoiceViewModel(i);
            DataContext = vm;
        }
        public AddInvoiceView()
        {
            InitializeComponent();
        }
        public void OK(object sender, RoutedEventArgs e)
        {
            if (vm.Check())
            {
                Close(vm.Result());
            }
            else
            {
                MessageBox.Show(this, "Số lượng không hợp lệ", "Lỗi", MessageBox.MessageBoxButtons.Ok);
            }
        }
        public void Cancel(object sender, RoutedEventArgs e)
        {
            Close(null);
        }
    }
}
