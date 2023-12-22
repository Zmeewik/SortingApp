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
    public partial class DemandPage : ContentPage
    {
        public InfoHandler infoHandler;

        //Material list//
        public ObservableCollection<DemandItem> AllDemands { get; set; } = new ObservableCollection<DemandItem>();
        public ObservableCollection<MaterialItem> InnerMaterials { get; set; } = new ObservableCollection<MaterialItem>();

        private bool isSaved = true;
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
                if (isSaved) saveTextButton.Text = "Сохранить";
                else saveTextButton.Text = "Сохранить *";
            }
        }

        private int chosenTab = 0;

        public DemandPage(InfoHandler infoHandler)
        {
            InitializeComponent();
            BindingContext = new MainViewModel2(this);
            this.infoHandler = infoHandler;

            AllDemands = new ObservableCollection<DemandItem>();
            var arr = infoHandler.demand.GetAllDemands();

            foreach (var demand in arr)
            {
                AllDemands.Add(new DemandItem(demand.name, demand.num, demand.quantity, demand.occupiedMaterials));
            }

            var arrM = infoHandler.material.GetAllMaterials();
            InnerMaterials = new ObservableCollection<MaterialItem>();
            foreach (var material in arrM)
            {
                InnerMaterials.Add(new MaterialItem(material.name, material.number, material.amount));
                InnerMaterials.Last().UsedQuantity = infoHandler.demand.GetUsedMaterials(material.number);
                InnerMaterials.Last().UnusedQuantity = material.amount - InnerMaterials.Last().UsedQuantity;
            }

            demandList.ItemsSource = AllDemands;
        }

        public void UpdateMaterialList(DemandItem demand)
        {
            foreach (var item in demand.OccupiedMaterials)
            {
                foreach (var mat in InnerMaterials)
                {
                    if (item.Key == mat.Num)
                    {
                        mat.UsedQuantity -= demand.Quantity * item.Value;
                        mat.UnusedQuantity += demand.Quantity * item.Value;
                    }
                }
            }
        }

        public void UpdateNumbers(int index)
        {
            foreach (var demand in AllDemands)
            {
                if (demand.Num > index)
                {
                    demand.Num--;
                }
            }
        }

        void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SignUpPage(infoHandler));
            DisplayAlert("Greet!", "Hey, whatsup man?", "Yooo");
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

        void OnDemandAdd(object sender, EventArgs e)
        {
            if (AllDemands.Count > 0)
            {
                var i = AllDemands.Last();
                AllDemands.Add(new DemandItem("", i.Num + 1, 1, new Dictionary<int, double> { }));
            }
            else
            {
                AllDemands.Add(new DemandItem("", 1, 1, new Dictionary<int, double> { } ));
            }

            IsSaved = false;
        }
        void OnDemandDelete(object sender, EventArgs e)
        {
            AllDemands.Remove(AllDemands[0]);
            demandList.ItemsSource = AllDemands;
        }
        void OnDemandChange(object sender, EventArgs e)
        {

        }

        void OnDemandChangeNum(object sender, EventArgs e)
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
            var newList1 = new List<Material>();
            foreach (var d in InnerMaterials)
            {
                newList1.Add(new Material("", d.Num, d.Quantity, Color.White, d.Name));
            }

            var newList = new List<Demand>();
            foreach (var d in AllDemands)
            {
                newList.Add(new Demand(d.OccupiedMaterials, "", d.Num, d.Quantity, Color.White, d.Name));
            }

            infoHandler.demand.SaveAllDemands(newList);
            infoHandler.material.SaveAllMaterials(newList1);

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

        public void DeleteDemand(int index)
        {
            //materials.Remove(materials[index]);
        }
    }

    public class MainViewModel2 : INotifyPropertyChanged
    {
        private string _displayText;

        public event PropertyChangedEventHandler PropertyChanged;
        DemandPage mPage;

        public Command<DemandItem> ChangeCommand { get; set; }
        public Command<DemandItem> DeleteCommand { get; set; }

        public MainViewModel2(DemandPage page)
        {
            mPage = page;
            ChangeCommand = new Command<DemandItem>(ChangeButtonCommandClick);
            DeleteCommand = new Command<DemandItem>(DeleteButtonCommandClick);
        }

        async void DeleteButtonCommandClick(DemandItem material)
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

        void DeleteAlertButtonCommandClick(DemandItem material, bool agree)
        {
            if (agree)
            {
                var matItem = material as DemandItem;
                var index = mPage.AllDemands.IndexOf(matItem);
                mPage.AllDemands.RemoveAt(index);
                mPage.DeleteDemand(index);
                mPage.UpdateMaterialList(matItem);

                mPage.UpdateNumbers(matItem.Num);

                mPage.IsSaved = false;
            }
        }

        private void DisplayInfo(bool isAgreed)
        {
            mPage.DisplayAlert(isAgreed.ToString(), "i", "M");
        }

        void UpdateDemand(DemandItem matItem)
        {
            if (matItem.isChanged)
            {
                mPage.IsSaved = false;
                matItem.isChanged = false;
            }

            var index = mPage.AllDemands.IndexOf(matItem);
            mPage.AllDemands[index] = matItem;

            /*            mPage.AllDemands[index].Num = matItem.Num;
                        mPage.AllDemands[index].Name = matItem.Name;
                        mPage.AllDemands[index].Quantity = matItem.Quantity;
                        mPage.AllDemands[index].OccupiedMaterials = matItem.OccupiedMaterials;*/
        }

        async void ChangeButtonCommandClick(DemandItem material)
        {
            var matItem = material as DemandItem;

            var alert = new AlertDemand(matItem, mPage.InnerMaterials, mPage.infoHandler);

            alert.Closed += (sender, args) =>
            {
                // Update all materials after the popup is closed
                UpdateDemand(matItem);
            };

            await PopupNavigation.Instance.PushAsync(alert);
        }

        
        // INotifyPropertyChanged implementation goes here
    }

}
