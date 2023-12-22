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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SortingApp
{
    public partial class AlertDemandSearch : PopupPage
    {
        public event EventHandler Closed;
        public MaterialInfo Item;
        public ObservableCollection<MaterialItem> Materials;

        internal AlertDemandSearch(ObservableCollection<MaterialItem> materials, MaterialInfo Item)
        {
            InitializeComponent();
            this.Materials = materials;
            this.Item = Item;
            materialListSearch.ItemsSource = Materials;
        }

        protected override void OnAppearing()
        {
            
        }

        private void OnChangeDeleteMaterialClicked(object sender, EventArgs e)
        {
            // Handle changing or deleting material here
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            // Handle searching for materials here based on the search bar input
            string searchText = searchBar.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                // If the search string is empty, reset the displayed materials
                materialListSearch.ItemsSource = Materials;
            }
            else
            {
                // Filter materials based on the search string
                var searchResults = Materials
                    .Where(material => material.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
                    .ToList();

                // Update the displayed materials
                materialListSearch.ItemsSource = new ObservableCollection<MaterialItem>(searchResults);
            }
        }

        private void OnMaterialTapped(object sender, ItemTappedEventArgs e)
        {
            // Handle selecting a material from the list here
            if (e.Item is MaterialItem tappedMaterial)
            {
                Item.Name = tappedMaterial.Name;
                Item.Num = tappedMaterial.Num;
            }
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            // Close the popup
            Item.cancel = true;
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        private void OnSave(object sender, EventArgs e)
        {
            // Close the popup
            Item.cancel = false;
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        private void OnMaterialAdd(object sender, EventArgs e)
        { 
            
        }

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class MaterialInfo
    { 
        public int Num;
        public string Name;
        public bool cancel = true;
    }

}
