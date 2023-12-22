using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static SortingApp.Files.Visuals.TableItem;


namespace SortingApp.Files.Visuals
{
    public class TableItem : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public string Date { get; set; }
        public string Path { get; set; }
        public bool isSet { get; set; }

        public bool isChanged = false;

        public TableItem(string name, DateTime date)
        {
            Name = name;
            Date = date.ToString("dd.MM");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
