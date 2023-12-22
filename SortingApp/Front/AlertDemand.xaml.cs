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
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SortingApp
{
    public partial class AlertDemand : PopupPage
    {
        InfoHandler infoHandler;
        public event EventHandler Closed;
        public DemandItem demandItem;
        public List<Material> allMItem;
        public int newDemandQuantity;

        public DemandItem savedItem;
        public ObservableCollection<MaterialItem> AllMaterials { get; set; } = new ObservableCollection<MaterialItem>();

        public Dictionary<int, double> OccupiedMaterials;
        public ObservableCollection<MaterialItem> UsedMaterials { get; set; } = new ObservableCollection<MaterialItem>();

        Dictionary<int, double> beforeAfter = new Dictionary<int, double>();

        internal AlertDemand(DemandItem demandItem, ObservableCollection<MaterialItem> InnerMaterials, InfoHandler infoHandler)
        {
            InitializeComponent();

            BindingContext = new CustomDialogViewModelAlertDemand(this);

            this.AllMaterials = InnerMaterials;
            this.demandItem = demandItem;
            savedItem = new DemandItem(demandItem.Name, demandItem.Num, demandItem.Quantity, demandItem.OccupiedMaterials);
            nameEntry.Text = demandItem.Name;
            numEntry.Text = demandItem.Quantity.ToString();
            OccupiedMaterials = demandItem.OccupiedMaterials;
            this.infoHandler = infoHandler;

            newDemandQuantity = demandItem.Quantity;

            foreach (var entry in OccupiedMaterials)
            {
                int occupiedMaterialNumber = entry.Key;
                MaterialItem matchingMaterial = AllMaterials.FirstOrDefault(mat => mat.Num == occupiedMaterialNumber);

                if (matchingMaterial != null)
                {
                    UsedMaterials.Add(new MaterialItem(matchingMaterial.Name, matchingMaterial.Num, matchingMaterial.Quantity));
                    UsedMaterials.Last().Quantity = entry.Value;
                    UsedMaterials.Last().UnusedQuantity = matchingMaterial.UnusedQuantity;
                    UsedMaterials.Last().UsedQuantity = matchingMaterial.UsedQuantity;
                    beforeAfter.Add(UsedMaterials.Last().Num, UsedMaterials.Last().Quantity);
                }
            }

            materialsOccupied.ItemsSource = UsedMaterials;
        }

        void OnQuantityEntryChange(object sender, EventArgs e)
        { 
            if(Int32.TryParse(numEntry.Text, out int res))
            {
                
                foreach(var used in UsedMaterials) 
                {
                    MaterialItem neededItem = AllMaterials.Last();
                    foreach (var mat in AllMaterials)
                    {
                        if (mat.Num == used.Num)
                        {
                            neededItem = mat;
                            break;
                        }
                    }

                    var befAf = new KeyValuePair<int, double>(used.Num, beforeAfter[used.Num]);

                    used.UnusedQuantity = neededItem.UnusedQuantity + befAf.Value * newDemandQuantity - befAf.Value * res;
                }

                newDemandQuantity = res;
            }
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
        }

        private void OnMaterialTapped(object sender, ItemTappedEventArgs e)
        {
            // Handle selecting a material from the list here
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            // Close the popup
            demandItem = savedItem;

            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        private void OnSave(object sender, EventArgs e)
        {
            if(demandItem.Name != nameEntry.Text)
                demandItem.isChanged = true;

            // Modify Material properties as needed
            demandItem.Name = nameEntry.Text;
            if (Int32.TryParse(numEntry.Text, out int res))
            {
                if(demandItem.Quantity != res)
                    demandItem.isChanged = true;
                demandItem.Quantity = res;
            }
            demandItem.OccupiedMaterials = new Dictionary<int, double>();
            foreach (var dems in UsedMaterials)
            {
                demandItem.OccupiedMaterials.Add(dems.Num, dems.Quantity);
            }

            foreach (var mat in UsedMaterials)
            {
                foreach (var Amat in AllMaterials)
                {
                    if (mat.Num == Amat.Num)
                    {
                        Amat.UnusedQuantity = mat.UnusedQuantity;
                        Amat.UsedQuantity = Amat.Quantity - Amat.UnusedQuantity;
                    }
                }
            }


            // Close the popup
            OnClosed();
            PopupNavigation.Instance.PopAsync();
        }

        protected virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private async void Addmaterial(object sender, EventArgs e)
        {
            var mat = new MaterialInfo();
            var alert = new AlertDemandSearch(AllMaterials, mat);

            alert.Closed += (senders, args) =>
            {
                // Update all materials after the popup is closed
               
                Addmaterial(mat);
            };

            await PopupNavigation.Instance.PushAsync(alert);
        }

        private void Addmaterial(MaterialInfo mat)
        {
            if (mat.cancel == true)
                return;
            foreach (var matI in UsedMaterials)
            {
                if (matI.Num == mat.Num)
                {
                    return;
                }
            }
            foreach (var matI in AllMaterials)
            {
                if (matI.Num == mat.Num)
                {
                    UsedMaterials.Add(new MaterialItem(matI.Name, matI.Num, 0));
                    UsedMaterials.Last().Quantity = 0;
                    UsedMaterials.Last().UnusedQuantity = matI.UnusedQuantity;
                    UsedMaterials.Last().UsedQuantity = matI.UsedQuantity;
                    beforeAfter.Add(UsedMaterials.Last().Num, UsedMaterials.Last().Quantity);
                }
            }

            demandItem.isChanged = true;

        }


        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry && entry.BindingContext is MaterialItem material)
            {
                if (double.TryParse(e.NewTextValue, out double newQuantity))
                {
                    MaterialItem neededItem = AllMaterials.Last();
                    foreach (var mat in AllMaterials)
                    {
                        if (mat.Num == material.Num)
                        {
                            neededItem = mat;
                            break;
                        }
                    }
                    var befAf = new KeyValuePair<int, double>(0, 0);
                    if (beforeAfter.ContainsKey(material.Num))
                    {
                        befAf = new KeyValuePair<int, double>(material.Num, beforeAfter[material.Num]);
                        
                    }

                    material.UnusedQuantity += (befAf.Value - newQuantity) * newDemandQuantity;
                    material.Quantity = newQuantity;

                    if (beforeAfter.ContainsKey(material.Num))
                    {
                        if (beforeAfter[material.Num] != newQuantity) demandItem.isChanged = true;
                    }
                    else
                    {
                        demandItem.isChanged = true;
                    }
                    beforeAfter[material.Num] = newQuantity;

                }
            }
        }

        public void ChangeMaterial(int index, int num, double newNum)
        {
            //materials.Remove(materials[index]);
            //OccupiedMaterials[num] = newNum;
            UsedMaterials[index].Quantity = newNum;
            if(UsedMaterials[index].Quantity != newNum) demandItem.isChanged = true;
        }

        public void DeleteMaterial(int index)
        {
            //materials.Remove(materials[index]);
            //OccupiedMaterials.Remove(index);
        }

    }

    public class CustomDialogViewModelAlertDemand : INotifyPropertyChanged
    {
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Command<MaterialItem> AddCommand { get; set; }
        public Command<MaterialItem> RemoveCommand { get; set; }
        public Command<MaterialItem> DeleteCommand { get; set; }
        public Command<MaterialItem> ChangeCommand { get; set; }
        AlertDemand mPage;

        public event PropertyChangedEventHandler PropertyChanged;

        public CustomDialogViewModelAlertDemand(AlertDemand mPage)
        {
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);

            AddCommand = new Command<MaterialItem>(AddMaterialTo);
            RemoveCommand = new Command<MaterialItem>(RemoveMaterialFrom);
            DeleteCommand = new Command<MaterialItem>(DeleteButtonCommandClick);
            ChangeCommand = new Command<MaterialItem>(OnEntryTextChaneged);

            this.mPage = mPage;
        }

        private void AddMaterialTo(MaterialItem material)
        {
            material.Quantity += 1;
            var matItem = material as MaterialItem;
            var index = mPage.UsedMaterials.IndexOf(matItem);
            mPage.ChangeMaterial(index, matItem.Num, material.Quantity);
        }

        private void RemoveMaterialFrom(MaterialItem material)
        {
            material.Quantity -= 1;
            var matItem = material as MaterialItem;
            var index = mPage.UsedMaterials.IndexOf(matItem);
            mPage.ChangeMaterial(index, matItem.Num, material.Quantity);
        }

        private void OnEntryTextChaneged(MaterialItem material)
        {
/*            if (Double.TryParse())
            { 
                
            }*/
        }

        async void DeleteButtonCommandClick(MaterialItem material)
        {
            InfoTransfer isAgreed = new InfoTransfer() { isAgree = false };
            var alert = new SureAlert(isAgreed);

            alert.Closed += (sender, args) =>
            {
                // Update all materials after the popup is closed
                DeleteAlertButtonCommandClick(material, isAgreed.isAgree);
            };
            await PopupNavigation.Instance.PushAsync(alert);
        }

        void DeleteAlertButtonCommandClick(MaterialItem material, bool agree)
        {
            if (agree)
            {
                var matItem = material as MaterialItem;
                var index = mPage.UsedMaterials.IndexOf(matItem);
                mPage.UsedMaterials.RemoveAt(index);
                mPage.DeleteMaterial(matItem.Num);

                mPage.demandItem.isChanged = true;
            }
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
