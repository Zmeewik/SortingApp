using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SortingApp.Files.Visuals
{
    public class MaterialItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int Num { get; set; }
        public double Quantity { get; set; }
        
        private double _unusedQuantity;
        public double UnusedQuantity
        {
            get { return _unusedQuantity; }
            set
            {
                if (_unusedQuantity != value)
                {
                    _unusedQuantity = value;
                    OnPropertyChanged(nameof(UnusedQuantity));
                }
            }
        }
        public double UsedQuantity { get; set; }

        public bool isChanged = false;

        public MaterialItem(string name, string description, string image, int num, double quantity) 
        {
            Name = name;
            Description = description;
            ImagePath = image;
            Num = num;
            Quantity = quantity;
        }

        public MaterialItem(string name, int num, double quantity)
        {
            Name = name;
            Num = num;
            Quantity = quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
