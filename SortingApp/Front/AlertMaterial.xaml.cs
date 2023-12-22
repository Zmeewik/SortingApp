using HandlerSpace;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SortingApp.Files.Visuals;
using System.Runtime.CompilerServices;
using BaseSpace;
using System;

namespace SortingApp
{
    public partial class AlertMaterial : PopupPage
    {
        public event EventHandler Closed;
        public MaterialItem materialItem;

        internal AlertMaterial(MaterialItem material)
        {
            InitializeComponent();
            this.materialItem = material;
            nameEntry.Text = material.Name;
            numEntry.Text = material.Quantity.ToString();
            //descriptionEntry.Text = material.Description;
        }

        protected override void OnAppearing()
        {
            
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            // Close the popup
            materialItem.isChanged = false;
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        private void OnSave(object sender, EventArgs e)
        {
            if(materialItem.Name != nameEntry.Text)
                materialItem.isChanged = true;

            // Modify Material properties as needed
            materialItem.Name = nameEntry.Text;
            //materialItem.Description = descriptionEntry.Text;
            if (double.TryParse(numEntry.Text, out double res))
            {
                if(materialItem.Quantity != res)
                    materialItem.isChanged = true;
                materialItem.Quantity = res;
            }
            materialItem.UnusedQuantity = materialItem.Quantity - materialItem.UsedQuantity;

            // Close the popup
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CustomDialogViewModel2 : INotifyPropertyChanged
    {
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CustomDialogViewModel2()
        {
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }

        private void Save()
        {
            // Implement your save logic
            // You may close the dialog here
        }

        private void Cancel()
        {
            // Implement your cancel logic
            // You may close the dialog here
        }
    }


}
