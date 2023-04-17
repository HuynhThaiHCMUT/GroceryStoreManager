﻿using System;
using System.Collections.Generic;
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
        public List<Unit> Units { get; set; }
        public ICommand AddUnitCommand { get; }
        public AddProductViewModel()
        {
            ProductId = "";
            ProductName = "";
            Units = new List<Unit>();
            AddUnitCommand = ReactiveCommand.Create<Window>(async (Window w) =>
            {
                var inputForm = new AddUnit();
                await inputForm.ShowDialog(w);
            });
        }
    }
}