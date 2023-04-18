using Avalonia.Controls;
using GroceryStoreManager.Models;
using GroceryStoreManager.Views;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ICommand AddProductCommand { get; }
        public ObservableCollection<Product> Query { get; set; }
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => this.RaiseAndSetIfChanged(ref searchText, value);
        }
        public MainWindowViewModel()
        {
            Inventory = new Inventory();
            Inventory.Read();
            AddProductCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                var inputForm = new AddProductView(Inventory);
                var result = await inputForm.ShowDialog<Product>(w);
                if (result != null)
                {
                    Inventory.Add(result);
                }
            });
            Query = new ObservableCollection<Product>(from p in Inventory.ProductList select p.Value);
            this.WhenAnyValue(x => x.SearchText).Throttle(TimeSpan.FromMilliseconds(300)).ObserveOn(RxApp.MainThreadScheduler).Subscribe(Search);
        }
        private void Search(string s)
        {
            Query.Clear();
            foreach (var item in Inventory.ProductList)
            {
                if (Filter(item.Value.Name, s))
                {
                    Query.Add(item.Value);
                }
            }
        }
        public bool Filter(string n, string s)
        {
            if (String.IsNullOrEmpty(s)) return true;
            else return (ToAscii(n.Trim()).Contains(ToAscii(s), StringComparison.OrdinalIgnoreCase));
        }
        //Copied from StackOverflow
        public string ToAscii(string unicodeStr, bool skipNonConvertibleChars = true)
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