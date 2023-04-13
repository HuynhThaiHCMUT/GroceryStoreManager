using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreManager.Models
{
    internal class Unit
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
        public override bool Equals(object? obj)
        {
            if (obj is not Unit) return false;
            return (Name == ((Unit)obj).Name);
        }
        public string ToData()
        {
            return Name.Replace(' ', '_') + " " + Weight.ToString() + " " + Price.ToString() + " " + BasePrice.ToString();
        }
    }
    internal class Product
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
        public void AddUnit(Unit unit)
        {
            int i = Units.IndexOf(unit);
            if (i == -1)
            {
                Units.Add(unit);
            }
            else
            {
                Units.RemoveAt(i);
                Units.Insert(i, unit);
            }
        }
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
    internal class Inventory
    {
        public Dictionary<long, Product> ProductList { get; set; }
        public Inventory()
        {
            ProductList = new Dictionary<long, Product>();
        }
        public void Read()
        {
            string path = @"ProductData";
            System.IO.Directory.CreateDirectory(@"C:\ShopData");
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
                    Product product = new(int.Parse(arr[0]), arr[1].Replace('_', ' '), int.Parse(arr[2]));
                    for (int i = 2; i < arr.Length; i += 3)
                    {
                        product.AddUnit(new Unit(arr[i], int.Parse(arr[i + 1]), int.Parse(arr[i + 2]), int.Parse(arr[i + 3])));
                    }
                    ProductList.Add(product.Id, product);
                }
            }
        }
        public void Save()
        {
            string path = @"ProductData";
            File.WriteAllText(path, "");
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (Product product in ProductList.Values)
                {
                    sw.WriteLine(product.ToData());
                }
            }
        }
    }
}
