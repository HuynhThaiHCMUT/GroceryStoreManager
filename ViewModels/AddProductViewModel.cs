using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using GroceryStoreManager.Models;
using GroceryStoreManager.Views;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;

namespace GroceryStoreManager.ViewModels
{
    public class AddProductViewModel: ViewModelBase
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductQuantity { get; set; }
        public ObservableCollection<Unit> Units { get; set; }
        public ICommand AddUnitCommand { get; }
        public ICommand EditUnitCommand { get; }
        public ICommand DeleteUnitCommand { get; }
        private readonly Inventory invRef;
        private long productId;
        private int productQuantity;
        private int selectedIndex;
        public int SelectedIndex 
        { 
            get => selectedIndex;
            set => this.RaiseAndSetIfChanged(ref selectedIndex, value);
        }
        public AddProductViewModel(Inventory inv)
        {
            invRef = inv;
            selectedIndex = -1;
            ProductId = string.Empty;
            ProductName = string.Empty;
            ProductQuantity = string.Empty;
            Units = new ObservableCollection<Unit>();
            AddUnitCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                var inputForm = new AddUnit(Units);
                var result = await inputForm.ShowDialog<Unit>(w);
                if (result != null)
                {
                    Units.Add(result);
                }
            });
            var opEnabled = this.WhenAnyValue(
                x => x.SelectedIndex,
                x => x != -1);
            EditUnitCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                int s = SelectedIndex;
                var inputForm = new AddUnit(Units, SelectedIndex);
                var result = await inputForm.ShowDialog<Unit>(w);
                if (result != null)
                {
                    Units.RemoveAt(s);
                    Units.Insert(s, result);
                }
            }, opEnabled);
            DeleteUnitCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                MessageBox.MessageBoxResult result = await MessageBox.Show(w, "Xác nhận xoá đơn vị?", "", MessageBox.MessageBoxButtons.YesNo);
                if (result == MessageBox.MessageBoxResult.Yes)
                {
                    Units.RemoveAt(SelectedIndex);
                }
            }, opEnabled);
        }
        public int Check()
        {
            if (!Int64.TryParse(ProductId, out productId))
            {
                return 1;
            }
            if (invRef.ContainId(productId))
            {
                return 2;
            }
            if (!Int32.TryParse(ProductQuantity, out productQuantity))
            {
                return 3;
            }
            return 0;
        }
        public Product Result()
        {
            Product re = new(productId, ProductName, productQuantity);
            foreach (Unit unit in Units)
            {
                re.AddUnit(unit);
            }
            return re;
        }
    }
}
