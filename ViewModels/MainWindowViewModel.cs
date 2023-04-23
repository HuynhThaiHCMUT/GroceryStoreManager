using Avalonia.Controls;
using GroceryStoreManager.Models;
using GroceryStoreManager.Views;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;

namespace GroceryStoreManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Inventory Inventory { get; set; }
        public ObservableCollection<Product> Query { get; set; }
        public ObservableCollection<InvoiceItem> Invoice { get; set; }
        public Window W { get; set; }
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand FinishInvoiceCommand { get; }
        public ICommand EditInvoiceCommand { get; }
        public ICommand DeleteInvoiceCommand { get; }
        private int sumTotal;
        public int SumTotal
        {
            get => sumTotal;
            set => this.RaiseAndSetIfChanged(ref sumTotal, value);
        }
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => this.RaiseAndSetIfChanged(ref searchText, value);
        }
        //Selected product in both sales and inventory panel
        private object selectedItem;
        public object SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }
        private int selectedInvoiceIndex;
        public int SelectedInvoiceIndex
        {
            get => selectedInvoiceIndex;
            set => this.RaiseAndSetIfChanged(ref selectedInvoiceIndex, value);
        }
        public MainWindowViewModel()
        {
            Inventory = new Inventory();
            Inventory.Read();
            SelectedInvoiceIndex = -1;
            SumTotal = 0;
            Query = new ObservableCollection<Product>(from p in Inventory.ProductList select p.Value);
            Invoice = new ObservableCollection<InvoiceItem>();
            //Subscribe search text change event (throttle for 300ms) to search method
            this.WhenAnyValue(x => x.SearchText).Throttle(TimeSpan.FromMilliseconds(300)).ObserveOn(RxApp.MainThreadScheduler).Subscribe(Search);
            AddProductCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                var inputForm = new AddProductView(Inventory);
                var result = await inputForm.ShowDialog<Product>(w);
                if (result != null)
                {
                    Inventory.ProductList.Add(result.Id, result);
                    Search(SearchText);
                }
            });
            //Enable inventory operation when SelectedItem (Product) is not null
            var opEnabled = this.WhenAnyValue(
                x => x.SelectedItem,
                (Func<object, bool>) (x => x != null));
            EditProductCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                Product p = SelectedItem as Product;
                var inputForm = new AddProductView(Inventory, p.Id);
                var result = await inputForm.ShowDialog<Product>(w);
                if (result != null)
                {
                    Inventory.ProductList.Remove(p.Id);
                    Inventory.ProductList.Add(result.Id, result);
                    Search(SearchText);
                }
            }, opEnabled);
            DeleteProductCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                MessageBox.MessageBoxResult result = await MessageBox.Show(w, "Xác nhận xoá sản phẩm?", "", MessageBox.MessageBoxButtons.YesNo);
                if (result == MessageBox.MessageBoxResult.Yes)
                {
                    Inventory.ProductList.Remove((SelectedItem as Product).Id);
                    Search(SearchText);
                }
            }, opEnabled);
            FinishInvoiceCommand = ReactiveCommand.Create<Window>((Window w) =>
            {
                foreach (InvoiceItem i in Invoice)
                {
                    if (Inventory.ProductList.ContainsKey(i.Item.Id))
                    {
                        Inventory.ProductList[i.Item.Id].Quantity -= i.Quantity;
                    }
                }
                Invoice.Clear();
                SumTotal = 0;
                Search(SearchText);
            });
            //Enable Invoice operation when selected index is valid
            var invoiceOpEnabled = this.WhenAnyValue(
                x => x.SelectedInvoiceIndex,
                x => x != -1);
            EditInvoiceCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                int i = SelectedInvoiceIndex;
                var inputForm = new AddInvoiceView(Invoice[i]);
                var result = await inputForm.ShowDialog<InvoiceItem>(w);
                if (result != null)
                {
                    SumTotal -= Invoice[i].Total;
                    Invoice.RemoveAt(i);
                    SumTotal += result.Total;
                    Invoice.Insert(i, result);
                }
            }, invoiceOpEnabled);
            DeleteInvoiceCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                MessageBox.MessageBoxResult result = await MessageBox.Show(w, "Xác nhận xoá sản phẩm?", "", MessageBox.MessageBoxButtons.YesNo);
                if (result == MessageBox.MessageBoxResult.Yes)
                {
                    SumTotal -= Invoice[SelectedInvoiceIndex].Total;
                    Invoice.RemoveAt(SelectedInvoiceIndex);
                }
            }, invoiceOpEnabled);
            
        }
        public async void AddInvoice(Window w)
        {
            if (SelectedItem is Product p)
            {
                var inputForm = new AddInvoiceView(p);
                var result = await inputForm.ShowDialog<InvoiceItem>(w);
                if (result != null)
                {
                    SumTotal += result.Total;
                    Invoice.Add(result);
                }
            }
        }
        private void Search(string s)
        {
            Query.Clear();
            //Open add invoice dialog if there is a barcode match
            if (Int64.TryParse(SearchText, out long result))
            {
                if (Inventory.ProductList.ContainsKey(result))
                {
                    SelectedItem = Inventory.ProductList[result];
                    AddInvoice(W);
                    SearchText = string.Empty;
                    return;
                }
            }
            //Normal search
            foreach (var item in Inventory.ProductList)
            {
                if (Filter(item.Value.Name, s))
                {
                    Query.Add(item.Value);
                }
            }
        }
        private bool Filter(string n, string s)
        {
            if (String.IsNullOrEmpty(s)) return true;
            else return (ToAscii(n.Trim()).Contains(ToAscii(s), StringComparison.OrdinalIgnoreCase));
        }
        //Copied from StackOverflow
        private string ToAscii(string unicodeStr, bool skipNonConvertibleChars = true)
        {
            if (string.IsNullOrWhiteSpace(unicodeStr))
            {
                return unicodeStr;
            }

            var normalizedStr = unicodeStr.Normalize(NormalizationForm.FormD);

            if (skipNonConvertibleChars)
            {
                return new string(normalizedStr.ToCharArray().Where(c => (int)c <= 127).ToArray());
            }

            return new string(
                normalizedStr.Where(
                    c =>
                    {
                        UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                        return category != UnicodeCategory.NonSpacingMark;
                    }).ToArray());
        }
    }
}