using GroceryStoreManager.Models;
using System;

namespace GroceryStoreManager.ViewModels
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        public string Quantity { get; set; }
        public int SelectedIndex { get; set; }
        public Product Product { get; set; }
        private int quantity;
        public AddInvoiceViewModel(Product p)
        {
            Product = p;
            Quantity = string.Empty;
            SelectedIndex = 0;
        }
        public AddInvoiceViewModel(InvoiceItem i)
        {
            Product = i.Item;
            Quantity = i.Quantity.ToString();
            SelectedIndex = i.Item.Units.IndexOf(i.BuyUnit);
        }
        public bool Check()
        {
            if (!Int32.TryParse(Quantity, out quantity))
            {
                return false;
            }
            return true;
        }
        public InvoiceItem Result()
        {
            return new InvoiceItem(Product, Product.Units[SelectedIndex], quantity);
        }
    }
}
