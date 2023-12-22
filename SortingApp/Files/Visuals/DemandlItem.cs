using System;
using System.Collections.Generic;
using System.Text;

namespace SortingApp.Files.Visuals
{
    public class DemandItem 
    {
        public string Name { get; set; }
        public int Num { get; set; }
        public int Quantity { get; set; }

        public Dictionary<int, double> OccupiedMaterials { get; set; }

        public bool isChanged = false; 

        public DemandItem(string name, int num, int quantity, Dictionary<int, double> mats) 
        {
            Name = name;
            Num = num;
            Quantity = quantity;
            OccupiedMaterials = mats;
        }
    }
}
