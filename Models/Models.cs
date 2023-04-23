using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreManager.Models
{
    public class Unit
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public int BasePrice { get; set; }
        public Unit(string name, int weight, int price, int basePrice)
        {
            Name = name;
            Weight = weight;
            Price = price;
            BasePrice = basePrice;
        }
        //Only compare unit's name
        public override bool Equals(object? obj)
        {
            if (obj is not Unit) return false;
            return (Name == ((Unit)obj).Name);
        }
        //Convert to saved text form
        public string ToData()
        {
            return Name.Replace(' ', '_') + " " + Weight.ToString() + " " + Price.ToString() + " " + BasePrice.ToString();
        }
        public override string ToString()
        {
            return Name + ": " + Price.ToString();
        }
    }
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public List<Unit> Units { get; set; }
        public Product(long id, string name, int quantity)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Units = new List<Unit>();
        }
        //Convert to saved text form
        public string ToData()
        {
            string s = "";
            foreach (Unit u in Units)
            {
                s += " " + u.ToData();
            }
            return Id.ToString() + " " + Name.Replace(' ', '_') + " " + Quantity.ToString() + s;
        }
    }
    public class Inventory
    {
        public Dictionary<long, Product> ProductList { get; set; }
        public Inventory()
        {
            ProductList = new Dictionary<long, Product>();
        }
        //Read inventory data from text file
        public void Read()
        {
            string path = @"ProductData";
            if (!File.Exists(path))
            {
                using StreamWriter sw = File.CreateText(path); ;

            }
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    string[] arr = s.Split(' ');
                    Product product = new(Int64.Parse(arr[0]), arr[1].Replace('_', ' '), Int32.Parse(arr[2]));
                    for (int i = 3; i < arr.Length; i += 4)
                    {
                        product.Units.Add(new Unit(arr[i].Replace('_', ' '), Int32.Parse(arr[i + 1]), Int32.Parse(arr[i + 2]), Int32.Parse(arr[i + 3])));
                    }
                    ProductList.Add(product.Id, product);
                }
                
            }
        }
        //Save current data to text file
        public void Save()
        {
            string path = @"ProductData";
            File.WriteAllText(path, "");
            using (StreamWriter sw = new(path))
            {
                foreach (Product product in ProductList.Values)
                {
                    sw.WriteLine(product.ToData());
                }
            }
        }
    }
    public class InvoiceItem
    {
        public Product Item { get; set; }
        public Unit BuyUnit { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public InvoiceItem(Product item, Unit unit, int quantity)
        {
            Item = item;
            BuyUnit = unit;
            Quantity = quantity;
            Total = BuyUnit.Price * Quantity;
        }
        public int Revenue()
        {
            return (BuyUnit.Price - BuyUnit.BasePrice) * Quantity;
        }
    }
}
