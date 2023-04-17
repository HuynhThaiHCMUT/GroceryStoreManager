﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GroceryStoreManager.Models;
using ReactiveUI;
using Tmds.DBus;

namespace GroceryStoreManager.ViewModels
{
    public class AddUnitViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string BasePrice { get; set; }
        public string Weight { get; set; }
        private int price;
        private int basePrice;
        private int weight;
        public AddUnitViewModel()
        {
            Name = string.Empty;
            Price = string.Empty;
            BasePrice = string.Empty;
            Weight = string.Empty;
        }
        public int Check()
        {
            if (!Int32.TryParse(Price, out price))
            {
                return 1;
            }
            if (!Int32.TryParse(BasePrice, out basePrice))
            {
                return 2;
            }
            if (!Int32.TryParse(Weight, out weight))
            {
                return 3;
            }
            return 0;
        }
        public Unit Result()
        {
            return new Unit(Name, weight, price, basePrice);
        }   
    }
}
