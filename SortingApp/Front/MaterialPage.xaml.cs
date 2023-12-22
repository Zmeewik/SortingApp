using BaseSpace;
using HandlerSpace;
using Rg.Plugins.Popup.Services;
using SortingApp.Files.Visuals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SortingApp
{
    public partial class MaterialPage : ContentPage
    {
        InfoHandler infoHandler;

        //Material list//
        public ObservableCollection<MaterialItem> AllMaterials { get; set; } = new ObservableCollection<MaterialItem>();
        public ObservableCollection<DemandItem> AllDemands { get; set; } = new ObservableCollection<DemandItem>();

        public List<int> deletedItems = new List<int>();
        private bool isSaved = true;
        public bool IsSaved {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
                if(isSaved) saveTextButton.Text = "Сохранить";
                else saveTextButton.Text = "Сохранить *";
            }
        }

        private int chosenTab = 0;

        internal MaterialPage(InfoHandler infoHandler)
        {
            InitializeComponent();
            BindingContext = new MainViewModel(this);
            this.infoHandler = infoHandler;

            var arr = infoHandler.material.GetAllMaterials();
            AllMaterials = new ObservableCollection<MaterialItem>();
            foreach (var material in arr)
            {
                AllMaterials.Add(new MaterialItem(material.name, material.number, material.amount));
                AllMaterials.Last().UsedQuantity = infoHandler.demand.GetUsedMaterials(material.number);
                AllMaterials.Last().UnusedQuantity = material.amount - AllMaterials.Last().UsedQuantity;
            }

            var arr1 = infoHandler.demand.GetAllDemands();
            AllDemands = new ObservableCollection<DemandItem>();
            foreach (var demand in arr1)
            {
                AllDemands.Add(new DemandItem(demand.name, demand.num, demand.quantity, new Dictionary<int, double>()));

                var copy = new Dictionary<int, double>();
                foreach (var entry in demand.occupiedMaterials)
                {
                    copy.Add(entry.Key, entry.Value);
                }
                AllDemands.Last().OccupiedMaterials = copy;
            }

            materialList.ItemsSource = AllMaterials;
        }

        public void UpdateNumbers(int index)
        {
            foreach (var material in AllMaterials)
            {
                if (material.Num > index)
                {
                    material.Num--;
                }
            }
        }

        public void DeleteDependencies(int index)
        {
            foreach (var demand in AllDemands)
            {
                if (demand.OccupiedMaterials.ContainsKey(index))
                {
                    demand.OccupiedMaterials.Remove(index);
                }

                // Create a new dictionary to store the modified key-value pairs
                var newOccupiedMaterials = new Dictionary<int, double>();

                // Adjust the keys and add the remaining items to the new dictionary
                foreach (var d in demand.OccupiedMaterials)
                {
                    if (d.Key > index)
                    {
                        newOccupiedMaterials.Add(d.Key - 1, d.Value);
                    }
                    else
                    {
                        newOccupiedMaterials.Add(d.Key, d.Value);
                    }
                }

                // Assign the new dictionary back to OccupiedMaterials
                demand.OccupiedMaterials = newOccupiedMaterials;
            }
        }

        async void OnFrameEmail_Tapped(object sender, EventArgs e)
        {
            _menuVisible = true;
            swipeMenuGrid.IsVisible = true;
            //await swipeMenu.TranslateTo(100, 0, 250, Easing.SinOut);
            await Task.WhenAll(
                swipeMenu.TranslateTo(100, 0, 250, Easing.SinOut),
                swipeMenuGrid.FadeTo(1, 250, Easing.SinOut)
            );
        }

        void OnMaterialAdd(object sender, EventArgs e)
        {
            if (AllMaterials.Count > 0)
            {
                var i = AllMaterials.Last();
                AllMaterials.Add(new MaterialItem("", i.Num + 1, 0));
                AllMaterials.Last().UnusedQuantity = 0;
                AllMaterials.Last().UsedQuantity = 0;
            }
            else
            {
                AllMaterials.Add(new MaterialItem("", 1, 0));
            }

            IsSaved = false;
        }
        void OnMaterialDelete(object sender, EventArgs e)
        {
            AllMaterials.Remove(AllMaterials[0]);
            materialList.ItemsSource = AllMaterials;
        }
        void OnMaterialChange(object sender, EventArgs e)
        {

        }

        void OnMaterialChangeNum(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            
        }

        //Menu//

        private double _previousX;
        private bool _menuVisible;

        private void SwipeMenuLeft()
        {

        }

        private void SwipeMenuRight()
        {

        }

        private async void OnOpenMenuClicked(object sender, EventArgs e)
        {
            _menuVisible = true;
            swipeMenuGrid.IsVisible = true;
            //await swipeMenu.TranslateTo(100, 0, 250, Easing.SinOut);
            await Task.WhenAll(
                swipeMenu.TranslateTo(100, 0, 250, Easing.SinOut),
                swipeMenuGrid.FadeTo(1, 250, Easing.SinOut)
            );
        }

        private async void OnSwipeMenuBackgroundTapped(object sender, EventArgs e)
        {
            if (_menuVisible)
            {
                _menuVisible = false;
                await Task.WhenAll(
                    swipeMenu.TranslateTo(400, 0, 250, Easing.SinOut),
                    swipeMenuGrid.FadeTo(0, 250, Easing.SinOut)
                );
                /*await swipeMenu.TranslateTo(400, 0, 250, Easing.SinOut);*/
                swipeMenuGrid.IsVisible = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (_menuVisible)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    _menuVisible = false;
                    await Task.WhenAll(
                        swipeMenu.TranslateTo(400, 0, 250, Easing.SinOut),
                        swipeMenuGrid.FadeTo(0, 250, Easing.SinOut)
                    );
                    swipeMenuGrid.IsVisible = false;
                });

                return true; // Indicate that the back button press is handled
            }

            return base.OnBackButtonPressed();
        }

        private void DoNothing(object sender, EventArgs e)
        { 
            
        }

        private void GoToProfile(object sender, EventArgs e)
        {

        }

        private void GoToUpload(object sender, EventArgs e)
        {

        }

        private void SaveInfo(object sender, EventArgs e)
        {
            SaveInfo();
            IsSaved = true;
        }

        private void SaveInfo()
        {
            var newList = new List<Material>();
            foreach (var d in AllMaterials)
            {
                newList.Add(new Material("", d.Num, d.Quantity, Color.White, d.Name));
            }

            var newListD = new List<Demand>();
            foreach (var d in AllDemands)
            {
                newListD.Add(new Demand(d.OccupiedMaterials, "", d.Num, d.Quantity, Color.White, d.Name));
            }

            infoHandler.material.SaveAllMaterials(newList);
            infoHandler.demand.SaveAllDemands(newListD);

            infoHandler.excel.WriteAllInformation(infoHandler.material.GetAllMaterials(), infoHandler.demand.GetAllDemands());
        }

        private void GoToTablese(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 0;
                NeedSave();
            }
            else
                Application.Current.MainPage = new TablesPage(infoHandler);
        }

        private void GoToProducts(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 1;
                NeedSave();
            }
            else
                Application.Current.MainPage = new DemandPage(infoHandler);
        }

        private void GoTomaterials(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 2;
                NeedSave();
            }
            else
                Application.Current.MainPage = new MaterialPage(infoHandler);
        }

        private async void CloseApp(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                chosenTab = 3;
                NeedSave();
            }
            else
                await Navigation.PopToRootAsync();
        }

        private async void NeedSave()
        {
            InfoTransfer isAgreed = new InfoTransfer() { isAgree = false };
            var alert = new SaveAlert(isAgreed);

            alert.Closed += (sender, args) =>
            {
                // Update all materials after the popup is closed
                OpenNext(isAgreed.isAgree);
            };
            await PopupNavigation.Instance.PushAsync(alert);
        }

        private async void OpenNext(bool isSaving)
        {
            if (isSaving)
            {
                SaveInfo();
            }
            switch (chosenTab)
            {
                case 0:
                    Application.Current.MainPage = new TablesPage(infoHandler);
                    break;
                case 1:
                    Application.Current.MainPage = new DemandPage(infoHandler);
                    break;
                case 2:
                    Application.Current.MainPage = new MaterialPage(infoHandler);
                    break;
                case 3:
                    await Navigation.PopToRootAsync();
                    break;
                case 4:
                    break;
            }
        }

        //Material interactions

        private void UpdateMaterialList(List<Material> materials)
        { 
            //this.materials = materials;
        }

        private void UpdateDemandList(List<Demand> demands)
        {
            //this.demands = demands;
        }

        public void DeleteMaterial(int index)
        {
            //materials.Remove(materials[index]);
        }

        public void ChangeMaterial(int index, MaterialItem mat)
        {
            //materials[index].name = mat.Name;
            //materials[index].amount = mat.Quantity;
        }

    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private string _displayText;

        public event PropertyChangedEventHandler PropertyChanged;
        MaterialPage mPage;

        public Command<MaterialItem> ChangeCommand { get; set; }
        public Command<MaterialItem> DeleteCommand { get; set; }

        public MainViewModel(MaterialPage page)
        {
            mPage = page;
            ChangeCommand = new Command<MaterialItem>(ChangeButtonCommandClick);
            DeleteCommand = new Command<MaterialItem>(DeleteButtonCommandClick);
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
                var index = mPage.AllMaterials.IndexOf(matItem);
                mPage.AllMaterials.RemoveAt(index);
                mPage.DeleteMaterial(index);

                mPage.deletedItems.Add(matItem.Num);
                mPage.DeleteDependencies(matItem.Num);
                mPage.UpdateNumbers(matItem.Num);

                mPage.IsSaved = false;
            }
        }

        private void DisplayInfo(bool isAgreed)
        {
            mPage.DisplayAlert(isAgreed.ToString(), "i", "M");
        }

        void UpdateMaterial(MaterialItem matItem)
        {
            var index = mPage.AllMaterials.IndexOf(matItem);

            if (matItem.isChanged == true)
            {
                mPage.IsSaved = false;
                matItem.isChanged = false;
            }

            mPage.AllMaterials[index] = matItem;
            mPage.ChangeMaterial(index, matItem);
        }

        async void ChangeButtonCommandClick(MaterialItem material)
        {
            var matItem = material as MaterialItem;

            var alert = new AlertMaterial(matItem);

            alert.Closed += (sender, args) =>
            {
                // Update all materials after the popup is closed
                UpdateMaterial(matItem);
            };

            await PopupNavigation.Instance.PushAsync(alert);
        }
        
        // INotifyPropertyChanged implementation goes here
    }

}
